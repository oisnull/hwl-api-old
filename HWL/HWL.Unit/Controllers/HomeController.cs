using HWL.Unit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HWL.Unit.Controllers
{
    public class HomeController : Controller
    {
        static int count = 0;

        // GET: Home
        public ActionResult Index()
        {
            RabbitMQ.MQManager.SendMessage("group-queue", GetChatGroupMessageBean());
            RabbitMQ.MQManager.SendMessage("user-3-queue", GetAddFriendBean());
            //RabbitMQ.MQManager.SendMessage("user-1-queue", GetChatUserMessageBean());
            //RabbitMQ.MQManager.SendMessage("user-2-queue", GetChatFriendRequestBean());
            //RabbitMQ.MQManager.SendMessage("user-1-queue", GetAddFriendBean());

            return View();
        }

        public byte[] GetChatGroupMessageBean()
        {
            count++;
            var model = new ChatGroupMessageBean()
            {
                groupGuid = "efd00305-043f-4ace-b114-98f360559d91",
                groupImage = "",
                groupName = "group-guid-1",
                fromUserId = 2,
                fromUserName = "liy",
                fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018021009583820180210175841.jpg",
                content = "你现在在哪里 - " + count,
                sendTime = DateTime.Now,
                contentType = 1,
            };
            //var model = new ChatGroupMessageBean()
            //{
            //    groupGuid = "group-guid-1",
            //    groupImage = "",
            //    groupName = "group-guid-1",
            //    fromUserId = 3,
            //    fromUserName = "",
            //    fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
            //    content = "hello every one ...",
            //    sendTime = DateTime.Now,
            //    contentType = 1,
            //};

            byte[] userIdBytes = BitConverter.GetBytes(model.fromUserId);
            byte[] groupIdBytes = Encoding.UTF8.GetBytes(model.groupGuid);
            byte[] contentBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            byte[] messageBytes = new byte[3 + userIdBytes.Length + groupIdBytes.Length + contentBytes.Length];

            messageBytes[0] = MessageType.CHAT_GROUP_MESSAGE;
            messageBytes[1] = (byte)userIdBytes.Length;
            messageBytes[2] = (byte)groupIdBytes.Length;
            setPositionBytes(3, userIdBytes, messageBytes);
            setPositionBytes(3 + userIdBytes.Length, groupIdBytes, messageBytes);
            setPositionBytes(3 + userIdBytes.Length + groupIdBytes.Length, contentBytes, messageBytes);

            return messageBytes;
        }

        public byte[] GetChatUserMessageBean()
        {
            count++;
            var model = new ChatUserMessageBean()
            {
                toUserId = 1,
                fromUserId = 2,
                fromUserName = "liy",
                fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018021009583820180210175841.jpg",
                content = "你现在在哪里 - "+ count,
                sendTime = DateTime.Now,
                contentType = 1,
            };
            String json = JsonConvert.SerializeObject(model);
            return mergeToStart(MessageType.CHAT_USER_MESSAGE, Encoding.UTF8.GetBytes(json));
        }

        public byte[] GetChatFriendRequestBean()
        {
            var model = new ChatFriendRequestBean()
            {
                toUserId = 2,
                toUserName = "liy",
                toUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
                fromUserId = 1,
                fromUserName = "2536",
                fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
                content = "我们已经成功好友了",
                sendTime = DateTime.Now,
                contentType = 1,
            };
            String json = JsonConvert.SerializeObject(model);
            return mergeToStart(MessageType.CHAT_FRIEND_REQUEST, Encoding.UTF8.GetBytes(json));
        }

        public byte[] GetAddFriendBean()
        {
            var model = new AddFriendBean()
            {
                friendId = 1,
                remark = "我是 liy",
                userHeadImage = "http://172.16.21.38:8033//upload/user-head/2018//2018021510415620180215184156.jpg",
                userId = 2,
                userName = "liyang"
            };
            //var model = new AddFriendBean()
            //{
            //    friendId = 1,
            //    remark = "我是 2536",
            //    userHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
            //    userId = 1,
            //    userName = "2536"
            //};

            String json = JsonConvert.SerializeObject(model);
            return mergeToStart(MessageType.FRIEND_REQUEST, Encoding.UTF8.GetBytes(json));
        }

        public static byte[] mergeToStart(byte headByte, byte[] bodyBytes)
        {
            byte[] resultBytes = new byte[bodyBytes.Length + 1];
            resultBytes[0] = headByte;
            for (int i = 1; i < resultBytes.Length; i++)
            {
                resultBytes[i] = bodyBytes[i - 1];
            }
            return resultBytes;
        }

        //从start位置开始，不包含start,1表示第一个位置
        public static void setPositionBytes(int start, byte[] addBytes, byte[] newBytes)
        {
            if (newBytes == null || addBytes == null || newBytes.Length < addBytes.Length)
                return;

            for (int i = 0; i < addBytes.Length; i++)
            {
                newBytes[start + i] = addBytes[i];
            }
        }
    }
}