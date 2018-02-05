using HWL.RabbitMQ;
using HWL.Redis;
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
        const int GROUP_COUNT = 5;
        const int GROUP_USER_COUNT = 500;//每个组里面的人数
        const int GROUP_MESSAGE_COUNT = 1000;//每个组里面的消息数量

        public static void AddGroupMessage()
        {
            for (int i = 1; i < GROUP_COUNT; i++)
            {
                for (int j = 1; j <= GROUP_MESSAGE_COUNT; j++)
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
                    MQManager.SendMessage(MQManager.GROUP_QUEUE_NAME, msgBytes);
                }

            }
        }
        public static void AddGroupUser()
        {
            GroupAction act = new GroupAction();
            for (int i = 1; i <= GROUP_COUNT; i++)
            {
                List<int> users = new List<int>();
                for (int j = 1; j <= GROUP_USER_COUNT; j++)
                {
                    users.Add(j);
                }
                act.SaveGroupUser("group-guid-" + i, users.ToArray());
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
