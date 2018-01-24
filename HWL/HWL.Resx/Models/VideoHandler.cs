using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

namespace HWL.Resx.Models
{

    public class VideoHandler
    {
        public string ffmpegtool = "~/content/ffmpeg/ffmpeg.exe";
        public string sizeOfImg = "240x180";

        public VideoHandler()
        {
            ffmpegtool = HttpContext.Current.Server.MapPath(ffmpegtool);
        }

        /// <summary>
        /// 获取文件的名字
        /// </summary>
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        /// <summary>
        /// 返回文件名称(文件名,视频时长)
        /// </summary>
        /// <returns></returns>
        public Tuple<string, double> CatchImg(string videoFullPath, string imgSaveDir)
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
                sizeString = this.sizeOfImg;
            }

            string flv_img = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
            string imgFullPath = imgSaveDir + flv_img;

            ProcessStartInfo pro = new ProcessStartInfo(this.ffmpegtool);
            pro.UseShellExecute = false;
            pro.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //获取一张缩略图
            pro.Arguments = "   -i   " + videoFullPath + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + sizeString + "   " + imgFullPath;
            try
            {
                System.Diagnostics.Process.Start(pro);
                return Tuple.Create(flv_img, time);
            }
            catch (Exception) { }

            return null;
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