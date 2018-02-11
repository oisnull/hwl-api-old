using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HWL.RabbitMQ.android_message
{
    public class AndroidChatMessage
    {
        public static string GetQueueName(int userId)
        {
            if (userId <= 0) return "user-none-queue";
            return string.Format("user-{0}-queue", userId);
        }

        public static void SendGroup(int toUserId, string gruopGruid, string content)
        {
            ChatGroupMessageBean model = new ChatGroupMessageBean()
            {
                groupGuid = gruopGruid,
                content = content,
                contentType = MQ_Constant.CHAT_MESSAGE_CONTENT_TYPE_WORD,
                fromUserHeadImage = gruopGruid,
                fromUserId = 0,
                fromUserName = string.Empty,
                groupImage = string.Empty,
                groupName = string.Empty,
                sendTime = DateTime.Now
            };

            //[message_type,message_content]
            byte[] bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            byte[] messageBytes = new byte[1 + bodyBytes.Length + 1];
            messageBytes[0] = MQ_Constant.CHAT_GROUP_MESSAGE;
            bodyBytes.CopyTo(messageBytes, 1);
            MQManager.SendMessage(GetQueueName(toUserId), messageBytes);
        }

    }
}
