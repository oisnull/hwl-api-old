using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Send
{
    public class HeartBeatMessageSend : AbstractMessageSendExecutor
    {
        private ulong userId;
        public HeartBeatMessageSend(ulong userId)
        {
            this.userId = userId;
        }

        public override ImMessageRequest getMessageRequest()
        {
            return new ImMessageRequest()
            {
                HeartBeatMessageRequest = new ImHeartBeatMessageRequest()
                {
                    CurrentTime = (ulong)DateTime.Now.Ticks,
                    FromUserId = userId
                }
            };
        }

        public override ImMessageType getMessageType()
        {
            return ImMessageType.HeartBeat;
        }
    }
}
