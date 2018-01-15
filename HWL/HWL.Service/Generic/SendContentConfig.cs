using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Generic
{
    public class SendContentConfig
    {

        /// <summary>
        /// title,content
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, string> EmailRegisterDesc(string code)
        {
            string title = "好无聊注册";
            string content = "您当前的注册验证码是：" + code;
            return new Tuple<string, string>(title, content);
        }

        /// <summary>
        /// title,content
        /// </summary>
        /// <returns></returns>
        public static string SMSRegisterDesc(string code)
        {
            string content = "您当前的好无聊注册验证码是：" + code;
            return content;
        }

    }
}
