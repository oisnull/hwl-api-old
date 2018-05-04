using System;
using System.Configuration;

namespace HWL.Manage.Service
{
    public class ConfigService
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
    }
}
