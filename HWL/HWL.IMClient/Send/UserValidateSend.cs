using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Send
{
    public class UserValidateSend : AbstractMessageSendExecutor
    {
        ulong userId = 0L;
        string token = "";

        public UserValidateSend(ulong userId, string token)
        {
            this.userId = userId;
            this.token = token;
        }

        public override ImMessageType getMessageType()
        {
            return ImMessageType.UserValidate;
        }

        public override ImMessageRequest getMessageRequest()
        {
            return new ImMessageRequest()
            {
                UserValidateRequest = new ImUserValidateRequest()
                {
                    Messageid = "",
                    UserId = this.userId,
                    Token = this.token,
                }
            };
        }

        //public override void success()
        //{

        //}

        //public override void failure(string message)
        //{
        //}
    }
}
