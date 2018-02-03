using HWL.RabbitMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute.message
{
    public class MQGroupMessageUnit
    {
        public static void AddGroupMessage()
        {
            int groupCount = 5;
            int groupMessageCount = 10;

            for (int i = 1; i < groupCount; i++)
            {
                for (int j = 1; j <= groupMessageCount; j++)
                {
                    var model = new MessageModel()
                    {
                        GroupId = "group-guid-" + i,
                        Content = "group message test " + j,
                        ContentType = 1,
                        FromUserId = 1,
                        MessageType = 1,
                    };
                    byte[] groupIdBytes = Encoding.UTF8.GetBytes(model.GroupId);
                    byte[] msgBytes1 = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
                    byte[] msgBytes = new byte[msgBytes1.Length + groupIdBytes.Length + 1];
                    msgBytes[0] = (byte)groupIdBytes.Length;
                    groupIdBytes.CopyTo(msgBytes, 1);
                    msgBytes1.CopyTo(msgBytes, groupIdBytes.Length + 1);
                    MQManager.SendMessage(MQManager.GROUPQUEUENAME, msgBytes);
                }

            }
        }
    }

    public class MessageModel
    {
        public string GroupId { get; set; }
        public int FromUserId { get; set; }
        public int MessageType { get; set; }
        public int ContentType { get; set; }
        public string Content { get; set; }
    }
}
