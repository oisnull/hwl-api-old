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

    }
}