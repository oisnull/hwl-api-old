using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HWL.Tools
{


    public class CommonCs
    {
        //public static string MD5(string str)
        //{
        //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
        //}

        /// <summary> 
        /// 把对象转成十进制 不成功返回0
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            decimal result = 0M;
            if (obj == null) return result;
            decimal.TryParse(obj.ToString(), out result);
            return result;

        }

        /// <summary>
        /// 把对象转换成双精度数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetObjToDouble(object obj)
        {
            double returnValue;
            if (obj == null) return 0;
            Double.TryParse(obj.ToString(), out returnValue);
            return returnValue;
        }

        /// <summary>
        /// 把对象转换成日期,如果为null则返回当前日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetObjToDateTime(string str)
        {
            if (str == null || str == "")
            {
                return DateTime.Now;
            }
            try
            {
                return DateTime.Parse(str);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 把对象转换成日期,如果为null则返回null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? GetObjToDateTime2(string str)
        {
            if (str == null || str == "")
            {
                return null;
            }
            try
            {
                return DateTime.Parse(str);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 把对象转换成数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetObjToInt(object obj)
        {
            int returnValue;
            if (obj == null) return 0;
            Int32.TryParse(obj.ToString(), out returnValue);
            return returnValue;
        }

        #region "3des加密字符串"


        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptString(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptString(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        #endregion

        #region md5 16位 32位 加密
        /// <summary>
        /// MD5 16位加密 加密后密码为小写
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd5Str16(string ConvertString)
        {
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
                    return t2.Replace("-", "").ToLower();
                }
            }
            catch { return null; }
        }

        /// <summary>
        /// MD5 32位加密 加密后密码为小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMd5Str32(string text)
        {
            try
            {
                using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
                {
                    byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    return sBuilder.ToString().ToLower();
                }
            }
            catch { return null; }
        }
        #endregion

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="RecordCount">记录总数</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageList">循环页码个数</param>
        /// <param name="strWhere">参数</param>
        /// <param name="cssClass">样式</param>
        /// <returns>分页字符串</returns>
        public static string GetPageHtmlStr(int RecordCount, int PageSize, int PageIndex, int PageList, string strWhere, string urlParam)
        {
            string cssClass = "";
            StringBuilder strPage = new StringBuilder();
            if (PageSize <= 0) PageSize = 1;
            int PageCount = (RecordCount + PageSize - 1) / PageSize;
            int PageTemp = 0;
            if (PageIndex > PageCount)
            {
                PageIndex = PageCount;
            }
            else if (PageIndex <= 0)
            {
                PageIndex = 1;
            }
            strPage.AppendFormat("共有{0}条记录 每页{1}条 {2}/{3} &nbsp;", RecordCount, PageSize, PageIndex, PageCount);
            strPage.AppendFormat("<a href=\"{0}?page=1{1}\" data-ajax='true' data-ajax-mode='replace' data-ajax-update='#main_content' class=\"\" onfocus=\"this.blur()\">首页</a>&nbsp;", strWhere, urlParam);
            strPage.AppendFormat("<a href=\"{1}?page={0}{3}\" data-ajax='true' data-ajax-mode='replace' data-ajax-update='#main_content' class=\"{2}\" onfocus=\"this.blur()\">上一页</a> &nbsp;", PageIndex - 1 <= 0 ? 1 : PageIndex - 1, strWhere, cssClass, urlParam);

            PageTemp = ((PageIndex - 1) / PageList) * PageList + 1;
            int i = 1;
            while (i <= PageList && PageTemp <= PageCount)
            {
                i++;
                strPage.AppendFormat(PageTemp == PageIndex ? "<b>{0}</b>&nbsp;" : "<a href=\"{1}?page={0}{3}\" data-ajax='true' data-ajax-mode='replace' data-ajax-update='#main_content' class=\"{2}\" onfocus=\"this.blur()\">{0}</a>&nbsp;\n", PageTemp++, strWhere, cssClass, urlParam);
            }
            strPage.AppendFormat("<a href=\"{1}?page={0}{3}\" data-ajax='true' data-ajax-mode='replace' data-ajax-update='#main_content' class=\"{2}\" onfocus=\"this.blur()\">下一页</a>&nbsp;\n", PageIndex + 1 > PageCount ? PageCount : PageIndex + 1, strWhere, cssClass, urlParam);
            strPage.AppendFormat("<a href=\"{1}?page={0}{3}\" data-ajax='true' data-ajax-mode='replace' data-ajax-update='#main_content' class=\"{2}\" onfocus=\"this.blur()\">尾页</a>", PageCount, strWhere, cssClass, urlParam);
            //strPage.AppendFormat(" <input type=\"text\" align=\"absmiddle\" id=\"goPage\" name=\"goPage\" class=\"input_blue\" size=\"2\" Width=\"50px\" value=\"{0}\" />\n", PageIndex);
            //strPage.AppendFormat(" <input type=\"button\" name=\"go\" value=\"go\"   style=\"border:#999999 1px solid;\" onfocus=\"this.blur()\" onClick=\"JavaScript:window.location.href='{0}?page='+document.getElementById('goPage').value +'{1}'\" />\n", strWhere, urlParam);
            return strPage.ToString();

        }

        #region 从html中提取数据
        /// <summary>
        /// 获得图片的路径并存放
        /// </summary>
        /// <param name="content">要检索的内容</param>
        /// <returns>List</returns>
        public static List<string> GetImageSrcList(string content)
        {
            List<string> im = new List<string>();//定义一个泛型字符类
            if (string.IsNullOrEmpty(content)) return im;
            Regex reg = new Regex("<img [^>]*src=['\"]([^'\"]+)[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(content); //设定要查找的字符串
            foreach (Match m in mc)
            {
                im.Add(m.Groups["src"].Value);
            }
            return im;
        }

        /// <summary>
        /// 获得内容中第一张图片的路径
        /// </summary>
        /// <param name="content">要检索的内容</param>
        /// <returns>List</returns>
        public static string GetImageSrc(string content)
        {
            if (string.IsNullOrEmpty(content)) return "";

            Regex reg = new Regex("<img [^>]*src=['\"]([^'\"]+)[^>]*>", RegexOptions.IgnoreCase);
            Match m = reg.Match(content); //设定要查找的字符串
            return m.Groups[1].Value;
        }

        /// <summary>
        /// 获取中文字符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="count">count=0时，表示提取所有的</param>
        /// <returns></returns>
        public static string GetChineseWord(string content, int count = 100)
        {
            if (string.IsNullOrEmpty(content)) return content;

            string x = @"[\u4E00-\u9FFF]+";
            MatchCollection Matches = Regex.Matches(content, x, RegexOptions.IgnoreCase);
            StringBuilder sb = new StringBuilder();
            foreach (Match NextMatch in Matches)
            {
                if (count > 0 && sb.Length >= count - 1)
                {
                    sb.Append("...");
                    break;
                }
                else
                {
                    sb.AppendFormat("{0},", NextMatch.Value);
                }
            }
            return sb.ToString();
        }
        #endregion



        #region IP地址
        /// <summary>
        /// 获取客户端IP地址（无视代理）若失败则返回回送地址
        /// </summary>
        /// <returns></returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        //获取客户端IP地址
        public static string GetIP()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (null == result || result == String.Empty)
            {
                return "0.0.0.0";
            }
            return result;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateFormatToString(DateTime dt)
        {
            TimeSpan span = (DateTime.Now - dt).Duration();
            if (span.TotalDays > 60)
            {
                return dt.ToString("yyyy-MM-dd");
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }

        public static string GetTicks(DateTime time)
        {
            DateTime dt = Convert.ToDateTime(time.ToString("yyyy-MM-dd HH:mm:ss"));
            return ((dt - Convert.ToDateTime("1970-1-1 0:0:0")).Ticks / 1000).ToString();
        }

        /// <summary>
        /// 计算指定时间到当前时间的总毫秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string CalcTotalMilliseconds(DateTime time)
        {
            DateTime old = Convert.ToDateTime("2017-01-01");
            DateTime newTime = Convert.ToDateTime(time.ToString("yyyy-MM-dd HH:mm:ss"));
            return (newTime - old).TotalMilliseconds.ToString();
        }
    }
}
