using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Unit.Models
{
    public class ChatFriendRequestBean
    {
        public int fromUserId { get; set; }
        public String fromUserName { get; set; }
        public String fromUserHeadImage { get; set; }
        public int toUserId { get; set; }
        public String toUserName { get; set; }
        public String toUserHeadImage { get; set; }
        public String content { get; set; }
        public DateTime sendTime { get; set; }
        public int contentType { get; set; }
    }
}