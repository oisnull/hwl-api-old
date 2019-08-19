using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Listen
{
    public class ChatGroupMessageListen : AbstractMessageListenExecutor<ImChatGroupMessageResponse>
    {
        public override void executeCore(ImMessageType messageType, ImChatGroupMessageResponse response)
        {
            Console.WriteLine(response.ToString());
        }

        public override ImChatGroupMessageResponse getResponse(ImMessageResponse response)
        {
            return response.ChatGroupMessageResponse;
        }
    }
}
