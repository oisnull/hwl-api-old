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

        public static void SendNearGroupWelcome(int toUserId, string gruopGruid, List<string> groupUserImages, string content)
        {
            ChatGroupMessageBean model = new ChatGroupMessageBean()
            {
                groupGuid = gruopGruid,
                groupUserImages = groupUserImages,
                groupName = "我的附近",
                content = content,
                contentType = MQConstant.CHAT_MESSAGE_CONTENT_TYPE_WELCOME_TIP,
                fromUserHeadImage = string.Empty,
                fromUserId = 0,
                fromUserName = string.Empty,
                sendTime = DateTime.Now
            };

            byte[] userIdBytes = BitConverter.GetBytes(model.fromUserId);
            byte[] groupIdBytes = Encoding.UTF8.GetBytes(model.groupGuid);
            byte[] contentBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            byte[] messageBytes = new byte[3 + userIdBytes.Length + groupIdBytes.Length + contentBytes.Length];

            messageBytes[0] = MQConstant.CHAT_GROUP_MESSAGE;
            messageBytes[1] = (byte)userIdBytes.Length;
            messageBytes[2] = (byte)groupIdBytes.Length;
            userIdBytes.CopyTo(messageBytes, 3);
            groupIdBytes.CopyTo(messageBytes, 3 + userIdBytes.Length);
            contentBytes.CopyTo(messageBytes, 3 + userIdBytes.Length + groupIdBytes.Length);

            MQManager.SendMessage(GetQueueName(toUserId), messageBytes);
        }

        public static void SendLogoutMessage(int userId, string token, string hintContent)
        {
            var model = new
            {
                userId = userId,
                token = token,
                loginTime = DateTime.Now,
                hintContent = hintContent
            };

            byte[] contentBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            byte[] messageBytes = new byte[1 + contentBytes.Length];

            messageBytes[0] = MQConstant.USER_LOGOUT_MESSAGE;
            contentBytes.CopyTo(messageBytes, 1);

            MQManager.SendMessage(GetQueueName(userId), messageBytes);
        }

    }
}
