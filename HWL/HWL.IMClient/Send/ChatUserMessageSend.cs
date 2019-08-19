using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Send
{
    public class ChatUserMessageSend : AbstractMessageSendExecutor
    {
        ulong fromUserId, toUserId = 0L;
        string content = "";

        public ChatUserMessageSend(ulong fromUserId, ulong toUserId, string content)
        {
            this.fromUserId = fromUserId;
            this.toUserId = toUserId;
            this.content = content;
        }

        public override ImMessageRequest getMessageRequest()
        {
            return new ImMessageRequest()
            {
                ChatUserMessageRequest = new ImChatUserMessageRequest()
                {
                    ChatUserMessageContent = new ImChatUserMessageContent()
                    {
                        Content = content,
                        ContentType = 0,
                        FromUserId = fromUserId,
                        FromUserName = "user-" + fromUserId,
                        ToUserId = toUserId,
                    }
                }
            };
        }

        public override ImMessageType getMessageType()
        {
            return ImMessageType.ChatUser;
        }

        public override void success()
        {
            base.success();
        }

        public override void failure(string message)
        {
            base.failure(message);
        }
    }
}
