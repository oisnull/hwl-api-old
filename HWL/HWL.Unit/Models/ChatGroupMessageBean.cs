using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Unit.Models
{
    public class ChatGroupMessageBean
    {
        public String groupGuid { get; set; }
        public String groupName { get; set; }
        public String groupImage { get; set; }
        public int fromUserId { get; set; }
        public String fromUserName { get; set; }
        public String fromUserHeadImage { get; set; }
        public int contentType { get; set; }
        public String content { get; set; }
        public DateTime sendTime { get; set; }
    }
}