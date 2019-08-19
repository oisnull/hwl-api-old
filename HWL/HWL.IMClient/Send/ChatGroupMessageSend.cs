using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Send
{
    public class ChatGroupMessageSend : AbstractMessageSendExecutor
    {
        ulong fromUserId;
        string groupGuid;
        string content;

        public ChatGroupMessageSend(ulong fromUserId, string groupGuid, string content)
        {
            this.fromUserId = fromUserId;
            this.groupGuid = groupGuid;
            this.content = content;
        }

        public override ImMessageRequest getMessageRequest()
        {
            return new ImMessageRequest()
            {
                ChatGroupMessageRequest = new ImChatGroupMessageRequest()
                {
                    ChatGroupMessageContent = new ImChatGroupMessageContent()
                    {
                        Content = content,
                        ContentType = 0,
                        FromUserId = fromUserId,
                        FromUserName = "group-user-" + fromUserId,
                        ToGroupGuid = groupGuid
                    }
                }
            };
        }

        public override ImMessageType getMessageType()
        {
            return ImMessageType.ChatGroup;
        }
    }
}
