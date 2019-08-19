using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Send
{
    public class ClientAckMessageSend : AbstractMessageSendExecutor
    {
        ulong fromUserId = 0;
        string messageid = "";

        public ClientAckMessageSend(ulong fromUserId, string messageid)
        {
            this.fromUserId = fromUserId;
            this.messageid = messageid;
        }

        public override ImMessageRequest getMessageRequest()
        {
            return new ImMessageRequest()
            {
                AckMessageRequest = new ImAckMessageRequest()
                {
                    FromUserId = fromUserId,
                    Messageid = messageid,
                }
            };
        }

        public override ImMessageType getMessageType()
        {
            return ImMessageType.ClientAckMessage;
        }
    }
}
