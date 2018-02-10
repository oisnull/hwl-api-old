using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Unit.Models
{
    public class ChatUserMessageBean
    {
        public int fromUserId { get; set; }
        public String fromUserName { get; set; }
        public String fromUserHeadImage { get; set; }
        public int toUserId { get; set; }
        public int contentType { get; set; }
        public String content { get; set; }
        public DateTime sendTime { get; set; }
    }
}