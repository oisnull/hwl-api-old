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
        // GET: Home
        public ActionResult Index()
        {
            //RabbitMQ.MQManager.SendMessage("user-2-queue", GetChatGroupMessageBean());
            RabbitMQ.MQManager.SendMessage("user-2-queue", GetChatUserMessageBean());
            //RabbitMQ.MQManager.SendMessage("user-2-queue", GetChatFriendRequestBean());
            //RabbitMQ.MQManager.SendMessage("user-2-queue", GetAddFriendBean());
            
            return View();
        }

        public byte[] GetChatGroupMessageBean()
        {
            //var model = new ChatGroupMessageBean()
            //{
            //    groupGuid = "group-guid-1",
            //    groupImage = "",
            //    groupName = "group-guid-1",
            //    fromUserId = 1,
            //    fromUserName = "2536",
            //    fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
            //    content = "你现在在哪里？",
            //    sendTime = DateTime.Now,
            //    contentType = 1,
            //};
            var model = new ChatGroupMessageBean()
            {
                groupGuid = "group-guid-1",
                groupImage = "",
                groupName = "group-guid-1",
                fromUserId = 3,
                fromUserName = "",
                fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
                content = "hello every one ...",
                sendTime = DateTime.Now,
                contentType = 1,
            };
            String json = JsonConvert.SerializeObject(model);
            return mergeToStart(MessageType.CHAT_GROUP_MESSAGE, Encoding.UTF8.GetBytes(json));
        }

        public byte[] GetChatUserMessageBean()
        {
            var model = new ChatUserMessageBean()
            {
                toUserId = 2,
                fromUserId = 1,
                fromUserName = "2536",
                fromUserHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
                content = "你现在在哪里？",
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
                friendId = 3,
                remark = "我是 liyyy",
                userHeadImage = "http://192.168.1.4:8033//upload/user-head/2018//2018012613243120180126212432.jpg",
                userId = 1,
                userName = "liyyy"
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
    }
}