using GMSF;
using HWL.Entity;
using HWL.Service.Generic.Body;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Generic.Service
{
    public class SendSMS : ServiceHandler<SendSMSRequestBody, SendSMSResponseBody>
    {
        public SendSMS(SendSMSRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (string.IsNullOrEmpty(this.request.Mobile))
            {
                throw new Exception("手机号码不能为空");
            }
        }

        public override SendSMSResponseBody ExecuteCore()
        {
            string randText = RandomText.GetNum(6);//生成6位验证码

            var codeInfo = SendContentConfig.SMSRegisterDesc(randText);//组织发送内容

            string error = "";
            bool succ = SmsSend.ToUser(this.request.Mobile, codeInfo, out error);//开始发送
            if (!succ) throw new Exception(error);

            int codeId = User.UserUtility.AddCode(CodeType.Register, randText, codeInfo, this.request.Mobile); //发送成功后记录验证码
            if (codeId <= 0)
            {
                throw new Exception("验证码发送失败");
            }

            return new SendSMSResponseBody()
            {
                Status = ResultStatus.Success,
                //CheckCode = randText
            };
        }
    }
}
