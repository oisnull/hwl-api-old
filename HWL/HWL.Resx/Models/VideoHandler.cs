using HWL.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace HWL.Resx.Models
{
    public class VideoHandler2 : UpfileHandler
    {
        const string CHUNK_EXT_ID = ".tmp";
        HttpPostedFile file = null;
        bool isExistsTempFile = false;
        //string orgFileName;
        string tempFileLocalPath;
        string tempFileName;
        int chunkIndex = 0;
        int chunkCount = 0;

        public VideoHandler2(HttpFileCollection fileKeys, ResxModel resxModel, int chunkIndex = 1, int chunkCount = 1, string tempFileUrl = null) : base(fileKeys, resxModel)
        {
            file = base.GetUpFile();
            this.chunkIndex = chunkIndex;
            this.chunkCount = chunkCount;
            this.tempFileName = Path.GetFileName(tempFileUrl);

            this.Init();
        }

        //first upload video
        private void Init()
        {
            if (chunkIndex <= 0) throw new Exception("数据流索引参数错误");
            if (chunkCount <= 0) throw new Exception("数据流数量参数错误");

            if (file == null)
                throw new Exception("上传文件是空的");

            //orgFileName = file.FileName;
            if (string.IsNullOrEmpty(file.FileName))
                throw new Exception("文件名参数错误");

            if (string.IsNullOrEmpty(tempFileName))
            {
                isExistsTempFile = false;
                this.tempFileName = base.GetNewFileName(Path.GetExtension(file.FileName)) + CHUNK_EXT_ID;
            }
            else
            {
                isExistsTempFile = true;
                tempFileLocalPath = rootDir + this.tempFileName;
            }
        }

        private Tuple<string, long> ResetFileName()
        {
            FileInfo fi = new FileInfo(tempFileLocalPath);
            if (!fi.Exists)
            {
                throw new Exception("数据流文件不存在");
            }
            fi.MoveTo(tempFileLocalPath.Replace(CHUNK_EXT_ID, ""));
            return new Tuple<string, long>(fi.Name, fi.Length);
        }

        protected override string GetNewFileName(string ext)
        {
            if (!isExistsTempFile && chunkCount == chunkIndex)
            {
                return base.GetNewFileName(ext);
            }
            return this.tempFileName;//添加临时文件名的后缀
        }

        public override ResxResult SaveStream()
        {
            ResxResult resx = null;
            if (!isExistsTempFile)//如果不存在则直接保存
            {
                resx = base.SaveStream();
                return resx;
            }
            else
            {
                if (string.IsNullOrEmpty(tempFileName))
                    throw new Exception("未找到需要合并的文件");
            }

            //如果存在则将上传的分片追加到临时文件末尾
            FileStream appendFileStream = null;
            BinaryWriter writer = null;
            BinaryReader reader = null;

            try
            {
                appendFileStream = new FileStream(tempFileLocalPath, FileMode.Append, FileAccess.Write);
                writer = new BinaryWriter(appendFileStream);
                reader = new BinaryReader(file.InputStream);
                writer.Write(reader.ReadBytes((int)file.InputStream.Length));

                resx = new ResxResult()
                {
                    Success = true,
                    OriginalUrl = accessDir + tempFileName,
                    OriginalSize = file.InputStream.Length,
                };
            }
            catch (Exception e)
            {
                resx = new ResxResult() { Success = false, Message = e.Message };
            }
            finally
            {
                if (appendFileStream != null)
                {
                    appendFileStream.Close();
                    appendFileStream.Dispose();
                }
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
                if (file.InputStream != null)
                {
                    file.InputStream.Close();
                    file.InputStream.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }

            if (chunkIndex >= chunkCount)//说明是最后一块数据流
            {
                var fileInfo = ResetFileName();
                resx.OriginalUrl = accessDir + fileInfo.Item1;
                resx.OriginalSize = fileInfo.Item2;

                //生成视频预览图片
                string videoLocalPath = rootDir + fileInfo.Item1;
                string videoPreviewSavePath = videoLocalPath.Replace(Path.GetExtension(videoLocalPath), "_p.jpg");
                var tuple = new VideoImageHanlder().CatchImg(videoLocalPath, videoPreviewSavePath);
                if (tuple.Item1)
                {
                    resx.PreviewUrl = accessDir + Path.GetFileName(videoPreviewSavePath);
                    resx.PlayTime = long.Parse(tuple.Item2 + "");
                }
            }

            return resx;
        }
    }

    public class VideoImageHanlder
    {
        private string ffmpegtool = "~/content/ffmpeg/ffmpeg.exe";
        public string sizeOfImg = ImageAction.FIX_HEIGHT + "x" + ImageAction.FIX_HEIGHT;

        public VideoImageHanlder()
        {
            ffmpegtool = HttpContext.Current.Server.MapPath(ffmpegtool);
        }

        /// <summary>
        /// 返回文件名称(文件名,视频时长)
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, double> CatchImg(string videoFullPath, string saveLocalPath)
        {
            string output;
            this.ExecuteCommand("\"" + this.ffmpegtool + "\"" + " -i " + "\"" + videoFullPath + "\"", out output);

            //通过正则表达式获取信息里面的宽度信息

            //获取视频的时长
            string timeSize = output.Substring(output.IndexOf("Duration: ") + ("Duration: ").Length, ("00:00:00").Length);
            double time = 0;
            if (!string.IsNullOrEmpty(timeSize))
            {
                time = TimeSpan.Parse(timeSize).TotalSeconds;
            }

            //获取视频的高度和宽度
            string sizeString = Regex.Match(output, "(\\d{2,4})x(\\d{2,4})").Value;
            if (string.IsNullOrEmpty(sizeString))
            {
                sizeString = sizeOfImg;
            }

            //string flv_img = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
            //string imgFullPath = imgSaveDir + flv_img;

            ProcessStartInfo pro = new ProcessStartInfo(this.ffmpegtool);
            pro.UseShellExecute = false;
            pro.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //获取一张缩略图
            pro.Arguments = "   -i   " + videoFullPath + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + sizeString + "   " + saveLocalPath;
            try
            {
                System.Diagnostics.Process.Start(pro);
                return Tuple.Create(true, time);
            }
            catch (Exception) { }

            return Tuple.Create(false, time);
        }

        public void ExecuteCommand(string command, out string output)
        {
            try
            {
                //创建一个进程
                Process pc = new Process();
                pc.StartInfo.FileName = "cmd.exe";
                pc.StartInfo.UseShellExecute = false;
                pc.StartInfo.RedirectStandardInput = true;
                pc.StartInfo.RedirectStandardOutput = true;
                pc.StartInfo.RedirectStandardError = true;
                pc.StartInfo.CreateNoWindow = true;

                //启动进程
                pc.Start();
                //执行命令
                pc.StandardInput.WriteLine(command);

                //等待退出
                pc.StandardInput.WriteLine("exit");

                //获取所有输出
                //output = pc.StandardOutput.ReadToEnd();
                output = pc.StandardError.ReadToEnd();
                //关闭进程
                pc.Close();
            }
            catch (Exception)
            {
                //output = null;
                output = null;
            }
        }
    }
}