using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HWL.Tools
{
    public class Base64ImageAction
    {
        /// <summary>
        /// 文件保存的目录
        /// </summary>
        public string SaveDirectory { get; set; }

        /// <summary>
        /// 文件保存名称,不带后缀格式
        /// </summary>
        public string SaveFileName { get; set; }

        //data:image/png;base64,
        private string _base64Head;
        //******
        private string _base64Body;

        public Base64ImageAction(string base64ImageString)
        {
            if (string.IsNullOrEmpty(base64ImageString)) throw new Exception("图片字符流是空的");
            if (!base64ImageString.StartsWith("data:image")) throw new Exception("字符流格式错误");

            string[] ary = base64ImageString.Split(',');
            _base64Head = ary[0];
            _base64Body = ary[1];
        }

        public void Save()
        {
            this.Init();

            byte[] bt = Convert.FromBase64String(_base64Body);
            MemoryStream stream = new System.IO.MemoryStream(bt);
            Bitmap bitmap = new Bitmap(stream);
            if (bitmap == null) return;

            try
            {
                bitmap.Save(this.SaveDirectory + this.SaveFileName, GetImageFormat());
                bitmap.Dispose();
            }
            catch
            {
                throw new Exception("图片保存错误");
            }
        }

        /// <summary>
        /// 初始化默认设置
        /// </summary>
        private void Init()
        {
            if (string.IsNullOrEmpty(this.SaveDirectory))
            {
                this.SaveDirectory = "c:/hwl-upload-temp/";
            }

            if (!Directory.Exists(this.SaveDirectory))
            {
                Directory.CreateDirectory(this.SaveDirectory);
            }

            if (string.IsNullOrEmpty(this.SaveFileName))
            {
                this.SaveFileName = Guid.NewGuid().ToString();
            }

            this.SaveFileName = this.SaveFileName + "." + this.GetImageFormatString();
        }

        public string GetImageFormatString()
        {
            Regex reg = new Regex("data:image/(.*);base64,");
            var m = reg.Match(_base64Head);
            if (m != null && m.Groups != null && m.Groups.Count >= 2)
            {
                return m.Groups[1].Value;
            }
            return "jpg";
        }

        /// <summary>
        /// 获取图片的格式
        /// </summary>
        /// <returns></returns>
        public ImageFormat GetImageFormat()
        {
            switch (this.GetImageFormatString())
            {
                case "bmp":
                    return ImageFormat.Bmp;
                case "emf":
                    return ImageFormat.Emf;
                case "exif":
                    return ImageFormat.Exif;
                case "gif":
                    return ImageFormat.Gif;
                case "ico":
                    return ImageFormat.Icon;
                case "png":
                    return ImageFormat.Png;
                case "tif":
                    return ImageFormat.Tiff;
                case "tiff":
                    return ImageFormat.Tiff;
                case "wmf":
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        public static string ImageToBase64String(string imagePath)
        {
            try
            {
                Bitmap bmp = new Bitmap(imagePath);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
