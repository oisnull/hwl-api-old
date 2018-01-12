using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace HWL.Tools
{

    public class BaiduIPAdressAPI
    {
        private const string AK_KEY = "1feTsY4ne4jaaFqU6kpvapY9lPxN3RQB"; //AK密钥

        /// <summary>返回UTF-8编码服务地址</summary>
        /// <returns>服务地址</returns>
        public static string GetPostUrl(string ip)
        {
            string postUrl = "http://api.map.baidu.com/location/ip?ak=" + AK_KEY + "&ip=" + ip + "&coor=bd09ll";
            return postUrl;
        }

        /// <summary>
        /// 返回结果（地址解析的结果）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetInfoByUrl(string ip)
        {
            //调用时只需要把拼成的URL传给该函数即可。判断返回值即可
            string strRet = null;
            string url = GetPostUrl(ip);
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception)
            {
                strRet = null;
            }
            return strRet;
        }

        public static string GetAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return "";

            string json = GetInfoByUrl(ip);
            if (string.IsNullOrEmpty(json)) return "";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                AddressRoot root = jss.Deserialize<AddressRoot>(json);
                if (root != null && root.content != null)
                    return root.content.address;
            }
            catch (Exception)
            {
            }
            return "";
        }

        #region address class

        //{
        //    "address":"CN|四川|绵阳|None|CHINANET|0|0",
        //    "content":{
        //        "address":"四川省绵阳市",
        //        "address_detail":{
        //            "city":"绵阳市",
        //            "city_code":240,
        //            "district":"",
        //            "province":"四川省",
        //            "street":"",
        //            "street_number":""
        //        },
        //        "point":{
        //            "x":"104.70551898",
        //            "y":"31.50470126"
        //        }
        //    },
        //    "status":0
        //}
        public class AddressRoot
        {
            public string address { get; set; }
            public Content content { get; set; }
            /// <summary>
            /// 0,表示成功 1,表示失败
            /// </summary>
            public int status { get; set; }
        }

        public class Content
        {
            public string address { get; set; }
            public Address_Detail address_detail { get; set; }
            public Point point { get; set; }
        }

        public class Address_Detail
        {
            public string city { get; set; }
            public int city_code { get; set; }
            public string district { get; set; }
            public string province { get; set; }
            public string street { get; set; }
            public string street_number { get; set; }
        }

        public class Point
        {
            public string x { get; set; }
            public string y { get; set; }
        }
        #endregion

        /*
            string theIP = YMethod.GetHostAddress();
            string msg = YMethod.GetInfoByUrl(GetPostUrl(theIP));
            msg = System.Text.RegularExpressions.Regex.Unescape(msg);
            Response.Write(msg);
         */
    }
}
