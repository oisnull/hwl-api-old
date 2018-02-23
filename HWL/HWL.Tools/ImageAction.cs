using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace HWL.Tools
{
    public class ImageAction
    {
        public class ThumbnailResult
        {
            public bool Success { get; set; }
            public long NewImageSize { get; set; }
            public int ImageWidth { get; set; }
            public int ImageHeight { get; set; }
        }

        /// 压缩图片    
        /// <param name="orgImagePath">原图片</param>    
        /// <param name="newImagePath">压缩后保存位置</param>    
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>    
        /// <returns></returns>    
        public static ThumbnailResult ThumbnailImage(string orgImagePath, string newImagePath, int flag = 50)
        {
            Image iSource = Image.FromFile(orgImagePath);
            int dHeight = iSource.Height;
            int dWidth = iSource.Width;
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放  
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(newImagePath, jpegICIinfo, ep);//newImagePath是压缩后的新路径    
                }
                else
                {
                    ob.Save(newImagePath, tFormat);
                }
                return new ThumbnailResult()
                {
                    Success = true,
                    ImageHeight = dHeight,
                    ImageWidth = dWidth,
                    NewImageSize = 0,
                };
            }
            catch
            {
                return new ThumbnailResult()
                {
                    Success = false,
                    ImageHeight = dHeight,
                    ImageWidth = dWidth,
                    NewImageSize = 0,
                };
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

    }
}
