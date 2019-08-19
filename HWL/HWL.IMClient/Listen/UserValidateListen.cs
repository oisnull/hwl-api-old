using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Listen
{
    public class UserValidateListen : AbstractMessageListenExecutor<ImUserValidateResponse>
    {
        Action<string> succCallback;
        public UserValidateListen(Action<string> succCallback)
        {
            this.succCallback = succCallback;
        }

        public override void executeCore(ImMessageType messageType, ImUserValidateResponse response)
        {
            Console.WriteLine("UserValidateListen : " + response?.ToString());
        }

        public override ImUserValidateResponse getResponse(ImMessageResponse response)
        {
            return response.UserValidateResponse;
        }

        public override void sessionidInvalid()
        {
            base.sessionidInvalid();
        }

        public override void success(ImUserValidateResponse response)
        {
            MessageRequestHeadManager.setSessionId(response.Sessionid);
            succCallback(response.Sessionid);
        }

        public override void failure(uint code, string message)
        {
            base.failure(code, message);
        }
    }
}
