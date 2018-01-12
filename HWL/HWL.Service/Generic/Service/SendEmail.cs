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
    public class SendEmail : ServiceHandler<SendEmailRequestBody, SendEmailResponseBody>
    {
        public SendEmail(SendEmailRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (string.IsNullOrEmpty(this.request.Email))
            {
                throw new Exception("邮箱格式错误");
            }
        }

        public override SendEmailResponseBody ExecuteCore()
        {
            string randText = RandomText.GetNum(6);//生成6位验证码

            var emailInfo = SendContentConfig.EmailRegisterDesc(randText);//组织发送内容

            string error = "";
            bool succ = EmailAction.SendEmail(emailInfo.Item1, emailInfo.Item2, this.request.Email, out error);//开始发送
            if (!succ) throw new Exception(error);

            int codeId = User.UserUtility.AddCode(CodeType.Register, randText, emailInfo.Item2, this.request.Email); //发送成功后记录验证码
            if (codeId <= 0)
            {
                throw new Exception("验证码发送失败");
            }

            return new SendEmailResponseBody() { CheckCode = randText };
        }
    }
}
