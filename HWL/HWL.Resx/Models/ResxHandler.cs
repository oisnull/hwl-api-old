using HWL.Entity;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HWL.Resx.Models
{
    //public class ResxHandler2
    //{
    //    public static readonly string[] FILETYPES = { ".gif", ".jpg", ".jpeg", ".png", ".bmp", ".apk", ".amr", ".wav", ".mp4" };//定义允许上传的文件扩展名
    //    public static readonly int MAXSIZE = 30 * 1024 * 1024;//最大文件大小,默认30M
    //    public static readonly int MAXFILECOUNT = 1;

    //    private string rootDir;
    //    private string accessDir;
    //    private HttpRequestMessage request;
    //    private MultipartFormDataMemoryStreamProvider provider;
    //    private List<string> accessUrls;

    //    public ResxHandler2(HttpRequestMessage request, ResxType resxType = ResxType.Other)
    //    {
    //        this.request = request;
    //        string dir = ResxConfigManager.UploadDirectory + CustomerEnumDesc.GetResxTypePath(resxType);
    //        rootDir = AppDomain.CurrentDomain.BaseDirectory + dir;
    //        accessDir = ResxConfigManager.FileAccessUrl + dir;

    //        if (!Directory.Exists(rootDir))
    //        {
    //            Directory.CreateDirectory(rootDir);
    //        }
    //    }

    //    public bool IsMultipartContent()
    //    {
    //        //检查该请求是否含有multipart/form-data
    //        if (!request.Content.IsMimeMultipartContent())
    //        {
    //            return false;
    //            //throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
    //        }
    //        return true;
    //    }

    //    public async Task SaveStream()
    //    {
    //        provider = new MultipartFormDataMemoryStreamProvider();
    //        try
    //        {
    //            await request.Content.ReadAsMultipartAsync(provider);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        if (provider == null ||
    //            provider.FileData == null ||
    //            provider.FileData.Count <= 0)
    //        {
    //            throw new Exception("上传资源不能为空");
    //        }

    //        var fileCount = provider.FileData.Count();
    //        if (fileCount > MAXFILECOUNT)
    //        {
    //            throw new Exception("上传资源数量超过大小");
    //        }
    //        var count = provider.FileData.Where(f => !FILETYPES.Contains(Path.GetExtension(f.Headers.ContentDisposition.FileName))).Count();
    //        if (count > 0)
    //        {
    //            throw new Exception("上传包含未知资源类型");
    //        }

    //        List<Stream> streamList = new List<Stream>();
    //        List<string> streamNames = new List<string>();
    //        foreach (MultipartFileData file in provider.FileData)
    //        {
    //            Stream stream = ((MultipartFileDataStream)file).Stream;
    //            if (stream.Length > MAXSIZE)
    //            {
    //                throw new Exception("上传资源超过大小");
    //            }
    //            else
    //            {
    //                streamList.Add(stream);
    //                streamNames.Add(file.Headers.ContentDisposition.FileName);
    //            }
    //        }

    //        int i = 0;
    //        accessUrls = new List<string>();
    //        foreach (Stream stream in streamList)
    //        {
    //            using (StreamWriter sw = new StreamWriter(Path.Combine(rootDir, streamNames[i])))
    //            {
    //                stream.CopyTo(sw.BaseStream);
    //                sw.Flush();

    //                accessUrls.Add(Path.Combine(accessDir, streamNames[i]));
    //            }
    //            i++;
    //        }
    //    }

    //    public List<string> GetAccessUrls()
    //    {
    //        return this.accessUrls;
    //    }

    //}

    public class ResxHandler
    {
        /// <summary>
        /// 暂时只能一个一个上传,后面再优化
        /// </summary>
        private static readonly int MAX_RESX_COUNT = 1;//限制上传资源的数量
        private static readonly long PREVIEW_IMAGE_SIZE = 100 * 1024;//当图片达到指定值时,需要压缩

        private HttpRequestMessage request;
        private ResxModel ResxModel;
        private MultipartFormDataMemoryStreamProvider provider;
        private string rootDir;
        private string accessDir;
        LogAction log = new LogAction("api-" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt");

        public ResxHandler(HttpRequestMessage request, ResxModel resxModel)
        {
            this.request = request;
            this.ResxModel = resxModel;

            Init();
        }

        public bool IsMultipartContent()
        {
            //检查该请求是否含有multipart/form-data
            if (!request.Content.IsMimeMultipartContent())
            {
                return false;
                //throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            return true;
        }

        public async Task<ResxResult> SaveStream()
        {
            provider = new MultipartFormDataMemoryStreamProvider();
            await request.Content.ReadAsMultipartAsync(provider);

            if (provider == null ||
                provider.FileData == null ||
                provider.FileData.Count <= 0)
            {
                throw new Exception("上传资源不能为空");
            }

            var fileCount = provider.FileData.Count();
            if (fileCount > MAX_RESX_COUNT)
            {
                throw new Exception("上传资源数量超过大小");
            }

            var count = provider.FileData.Where(f => !this.ResxModel.ResxTypes.Contains(Path.GetExtension(f.Headers.ContentDisposition.FileName.Replace("\"", "")))).Count();
            if (count > 0)
            {
                throw new Exception("上传包含未知资源类型");
            }

            List<Stream> streamList = new List<Stream>();
            List<string> streamNames = new List<string>();
            foreach (MultipartFileData file in provider.FileData)
            {
                Stream stream = ((MultipartFileDataStream)file).Stream;
                if (stream.Length > this.ResxModel.ResxSize && this.ResxModel.ResxSize > 0)
                {
                    throw new Exception("上传资源超过大小");
                }
                else
                {
                    streamList.Add(stream);
                    streamNames.Add(file.Headers.ContentDisposition.FileName.Replace("\"", ""));
                }
            }

            int i = 0;
            ResxResult result = new ResxResult() { Success = false };
            foreach (Stream stream in streamList)
            {
                string localPath = Path.Combine(rootDir, streamNames[i]);
                using (StreamWriter sw = new StreamWriter(localPath))
                {
                    stream.CopyTo(sw.BaseStream);
                    sw.Flush();

                    string orgUrl = Path.Combine(accessDir, streamNames[i]);

                    result.Success = true;
                    result.OriginalUrl = orgUrl;
                    result.OriginalSize = sw.BaseStream.Length;

                    //检测是否要生成预览图片
                    if (this.ResxModel.IsPreview() && sw.BaseStream.Length > PREVIEW_IMAGE_SIZE)
                    {
                        log.WriterLog("检测是否要生成预览图片sw.BaseStream.Length=" + sw.BaseStream.Length + "   PREVIEW_IMAGE_SIZE=" + PREVIEW_IMAGE_SIZE);
                        string ext = Path.GetExtension(streamNames[i]);
                        string newLocalPath = localPath.Replace(ext, "_p" + ext);

                        //压缩图片
                        ImageAction.ThumbnailResult ret = ImageAction.ThumbnailImage(localPath, newLocalPath);
                        if (ret.Success)
                        {
                            result.PreviewUrl = orgUrl.Replace(ext, "_p" + ext);
                            result.PreviewSize = ret.NewImageSize;
                            result.Width = ret.ImageWidth;
                            result.Height = ret.ImageHeight;
                            log.WriterLog("压缩成功："+result.PreviewUrl);
                        }
                        else
                        {
                            result.PreviewUrl = orgUrl;
                            result.PreviewSize = sw.BaseStream.Length;
                            log.WriterLog("压缩失败：" + result.PreviewUrl);
                        }
                    }

                    //检测是否是语音文件
                    //.....

                    //检测是否是视频文件
                    //.....

                }
                i++;
            }

            return result;
        }

        private void Init()
        {
            if (this.ResxModel == null)
            {
                throw new Exception("资源属性不能为空");
            }

            if (this.ResxModel.ResxTypes == null || this.ResxModel.ResxTypes.Length <= 0)
            {
                throw new Exception("需要指定上传资源的类型");
            }

            string dir = ResxConfigManager.UploadDirectory + CustomerEnumDesc.GetResxTypePath(this.ResxModel.ResxType, this.ResxModel.UserId);
            rootDir = AppDomain.CurrentDomain.BaseDirectory + dir;
            accessDir = ResxConfigManager.FileAccessUrl + dir;

            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }
        }
    }

    public class ResxModel
    {
        public int UserId { get; set; }
        public ResxType ResxType { get; set; } = ResxType.Other;
        public string[] ResxTypes { get; set; }
        public int ResxSize { get; set; }

        /// <summary>
        /// 是否压缩成预览图
        /// </summary>
        /// <returns></returns>
        public bool IsPreview()
        {
            if (this.ResxType == ResxType.ChatImage || this.ResxType == ResxType.CirclePostImage)
                return true;

            return false;
        }
    }

    public class ResxResult
    {
        public bool Success { get; set; }
        public string OriginalUrl { get; set; }
        public long OriginalSize { get; set; }
        public string PreviewUrl { get; set; }
        public long PreviewSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long PlayTime { get; set; }
    }
}