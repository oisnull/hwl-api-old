using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.RabbitMQ.android_message
{
    public class ChatGroupMessageBean
    {
        public string groupGuid { get; set; }
        public string groupName { get; set; }
        public List<string> groupUserImages { get; set; }
        public int fromUserId { get; set; }
        public string fromUserName { get; set; }
        public string fromUserHeadImage { get; set; }
        public int contentType { get; set; }
        public string content { get; set; }
        public DateTime sendTime { get; set; }
    }
}
