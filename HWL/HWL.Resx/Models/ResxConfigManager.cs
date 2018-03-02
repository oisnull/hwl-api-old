using System.Configuration;

namespace HWL.Resx.Models
{
    public class ResxConfigManager
    {
        /// <summary>
        /// 文件上传目录配置,值为当前计算机的绝对地址
        /// </summary>
        public static string UploadDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["UploadDirectory"];
            }
        }

        /// <summary>
        /// 文件对外访问的地址配置,值以http://或者https://开头
        /// </summary>
        public static string FileAccessUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["FileAccessUrl"];
            }
        }

        /// <summary>
        /// 暂时只能一个一个上传,后面再优化
        /// </summary>
        public static readonly int RESX_MAX_COUNT = 1;//限制上传资源的数量
        public static readonly long PREVIEW_IMAGE_SIZE = 30 * 1024;//当图片达到指定值时,需要压缩

        /// <summary>
        /// 定义图片允许上传的文件扩展名
        /// </summary>
        public readonly static string[] IMAGE_FILE_TYPES = { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };
        public readonly static int IMAGE_MAX_SIZE = 1 * 1024 * 1024;//默认1M

        /// <summary>
        /// 定义语音允许上传的文件扩展名
        /// </summary>
        public readonly static string[] SOUND_FILE_TYPES = { ".amr" };
        public readonly static int SOUND_MAX_SIZE = 2 * 1024 * 1024;//默认2M
        public readonly static int SOUND_MAX_TIME = 60;//默认60s

        /// <summary>
        /// 定义视频允许上传的文件扩展名
        /// </summary>
        public readonly static string[] VIDEO_FILE_TYPES = { ".mp4", ".tmp" };
        public readonly static int VIDEO_MAX_SIZE = 20 * 1024 * 1024;//默认20M
    }
}
