using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.RabbitMQ.android_message
{
    public class MQ_Constant
    {

        public readonly static byte FRIEND_REQUEST = 1;//添加好友的请求验证
        public readonly static byte CHAT_FRIEND_REQUEST = 2;//添加成功后的消息
        public readonly static byte CHAT_USER_MESSAGE = 3;
        public readonly static byte CHAT_GROUP_MESSAGE = 4;

        public readonly static int CHAT_RECORD_TYPE_USER = 1;
        public readonly static int CHAT_RECORD_TYPE_GROUP = 2;

        public readonly static int CHAT_MESSAGE_CONTENT_TYPE_WORD = 1;
        public readonly static int CHAT_MESSAGE_CONTENT_TYPE_IMAGE = 2;
        public readonly static int CHAT_MESSAGE_CONTENT_TYPE_WELCOME_TIP = 3;

        public readonly static int CHAT_SEND_SENDING = 1;
        public readonly static int CHAT_SEND_SUCCESS = 2;
        public readonly static int CHAT_SEND_FAILD = 3;

        //    //好友请求验证状态定义
        //    public final static int FRIEND_REQUEST_STATUS_UNTREATED = 0;//未处理
        //    public final static int FRIEND_REQUEST_STATUS_AGREED = 1;//已同意
        //    public final static int FRIEND_REQUEST_STATUS_REFUSED = 2;//已拒绝
    }
}
