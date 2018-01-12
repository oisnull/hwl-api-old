using HWL.Entity;
using System;

namespace HWL.Service.Generic.Body
{

    public class SendEmailRequestBody
    {
        /// <summary>
        /// 接收者的邮件
        /// </summary>
        public string Email { get; set; }
    }

    public class SendEmailResponseBody
    {
        public string CheckCode { get; set; }


    }
}
