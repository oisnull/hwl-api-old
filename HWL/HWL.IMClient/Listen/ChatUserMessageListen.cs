using HWL.IMClient.Core;
using HWL.IMCore.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Listen
{
    public class ChatUserMessageListen : AbstractMessageListenExecutor<ImChatUserMessageResponse>
    {
        public override void executeCore(ImMessageType messageType, ImChatUserMessageResponse response)
        {
            Console.WriteLine(response.ToString());
        }

        public override ImChatUserMessageResponse getResponse(ImMessageResponse response)
        {
            return response.ChatUserMessageResponse;
        }
    }
}
