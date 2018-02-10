using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Unit.Models
{
    public class MessageType
    {
        public readonly static byte FRIEND_REQUEST = 1;//添加好友的请求验证
        public readonly static byte CHAT_FRIEND_REQUEST = 2;//添加成功后的消息
        public readonly static byte CHAT_USER_MESSAGE = 3;
        public readonly static byte CHAT_GROUP_MESSAGE = 4;

        public readonly static int CHAT_RECORD_TYPE_USER = 1;
        public readonly static int CHAT_RECORD_TYPE_GROUOP = 2;

        public readonly static int CHAT_MESSAGE_CONTENT_TYPE_WORD = 1;
        public readonly static int CHAT_MESSAGE_CONTENT_TYPE_IMAGE = 2;

    }
}