using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Tools
{
    public class LogAction
    {
        private static object obj = new object();
        private string logDir = "/log/";
        public LogAction()
        {
            logDir = System.Web.HttpContext.Current.Server.MapPath(logDir);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
        }

        /// <summary>
        /// path为相对路径
        /// </summary>
        public LogAction(string fileName)
            : this()
        {
            this._fileName = fileName;
        }

        private string _fileName;

        public string FileName
        {
            get { return logDir + _fileName; }
            set { _fileName = value; }
        }

        public void WriterLog(string content)
        {
            lock (obj)
            {
                if (!File.Exists(this.FileName))
                {
                    File.Create(this.FileName).Close();
                }
                FileInfo finfo = new FileInfo(this.FileName);
                using (FileStream fs = finfo.OpenWrite())
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
        }

        public string ReadLog()
        {
            string s = "";
            StreamReader sr = new StreamReader(this.FileName, Encoding.UTF8);
            s = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return s;
        }

        //public static void ClearLog(string path)
        //{
        //    if (File.Exists(path))
        //    {
        //        FileInfo finfo = new FileInfo(path);
        //        using (FileStream fs = finfo.OpenWrite())
        //        {
        //            StreamWriter sw = new StreamWriter(fs);
        //            sw.WriteLine("");
        //            sw.Flush();
        //            sw.Close();
        //            fs.Close();
        //        }
        //    }
        //}
    }
}
