using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HWL.H5.Models
{
    public class ConfigManager
    {
        //public static string AppDownloadUrl
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["AppDownloadUrl"];
        //    }
        //}

        public static string SaveQRPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
        }
    }
}