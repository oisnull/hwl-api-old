using HWL.Entity;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HWL.Resx.Models
{
    public class ResxBase
    {
        protected ResxModel resxModel;
        protected string rootDir;
        protected string accessDir;

        private string newLocalPath;

        public ResxBase(ResxModel resxModel)
        {
            this.resxModel = resxModel;

            Init();
        }

        private void Init()
        {
            if (this.resxModel == null)
            {
                throw new Exception("资源属性不能为空");
            }

            if (this.resxModel.ResxTypes == null || this.resxModel.ResxTypes.Length <= 0)
            {
                throw new Exception("需要指定上传资源的类型");
            }

            string dir = ResxConfigManager.UploadDirectory + CustomerEnumDesc.GetResxTypePath(this.resxModel.ResxType, this.resxModel.UserId);
            rootDir = AppDomain.CurrentDomain.BaseDirectory + dir;
            accessDir = ResxConfigManager.FileAccessUrl + dir;

            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }
        }

        public long GetResxLength()
        {
            if (string.IsNullOrEmpty(newLocalPath))
                return 0;
            return new FileInfo(newLocalPath).Length;
        }

        public string GenerateImagePreview(string localPath)
        {
            string ext = Path.GetExtension(localPath);
            newLocalPath = string.Empty;
            newLocalPath = localPath.Replace(ext, "_p" + ext);

            //压缩图片
            ImageAction.ThumbnailResult ret = ImageAction.ThumbnailImage(localPath, newLocalPath);
            if (ret.Success)
            {
                return accessDir + Path.GetFileName(newLocalPath);
            }

            return null;
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