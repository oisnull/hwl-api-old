using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Generic.Body
{
    public class SendSMSRequestBody
    {
        /// <summary>
        /// 接收者的手机号码
        /// </summary>
        public string Mobile { get; set; }
    }

    public class SendSMSResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
