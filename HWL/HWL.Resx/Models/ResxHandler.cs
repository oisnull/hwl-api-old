using HWL.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HWL.Resx.Models
{
    public class ResxHandler
    {
        public static readonly string[] FILETYPES = { ".gif", ".jpg", ".jpeg", ".png", ".bmp", ".apk", ".amr", ".wav", ".mp4" };//定义允许上传的文件扩展名
        public static readonly int MAXSIZE = 30 * 1024 * 1024;//最大文件大小,默认30M
        public static readonly int MAXFILECOUNT = 1;

        private string rootDir;
        private string accessDir;
        private HttpRequestMessage request;
        private MultipartFormDataMemoryStreamProvider provider;
        private List<string> accessUrls;

        public ResxHandler(HttpRequestMessage request, ResxType resxType = ResxType.Other)
        {
            this.request = request;
            string dir = ResxConfigManager.UploadDirectory + CustomerEnumDesc.GetResxTypePath(resxType);
            rootDir = AppDomain.CurrentDomain.BaseDirectory + dir;
            accessDir = ResxConfigManager.FileAccessUrl + dir;

            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }
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

        public async Task SaveStream()
        {
            provider = new MultipartFormDataMemoryStreamProvider();
            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (provider == null ||
                provider.FileData == null ||
                provider.FileData.Count <= 0)
            {
                throw new Exception("上传资源不能为空");
            }

            var fileCount = provider.FileData.Count();
            if (fileCount > MAXFILECOUNT)
            {
                throw new Exception("上传资源数量超过大小");
            }
            var count = provider.FileData.Where(f => !FILETYPES.Contains(Path.GetExtension(f.Headers.ContentDisposition.FileName))).Count();
            if (count > 0)
            {
                throw new Exception("上传包含未知资源类型");
            }

            List<Stream> streamList = new List<Stream>();
            List<string> streamNames = new List<string>();
            foreach (MultipartFileData file in provider.FileData)
            {
                Stream stream = ((MultipartFileDataStream)file).Stream;
                if (stream.Length > MAXSIZE)
                {
                    throw new Exception("上传资源超过大小");
                }
                else
                {
                    streamList.Add(stream);
                    streamNames.Add(file.Headers.ContentDisposition.FileName);
                }
            }

            int i = 0;
            accessUrls = new List<string>();
            foreach (Stream stream in streamList)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(rootDir, streamNames[i])))
                {
                    stream.CopyTo(sw.BaseStream);
                    sw.Flush();

                    accessUrls.Add(Path.Combine(accessDir, streamNames[i]));
                }
                i++;
            }
        }

        public List<string> GetAccessUrls()
        {
            return this.accessUrls;
        }

    }
}