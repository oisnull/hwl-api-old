using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute.message
{
    public class MessageBase
    {
        public string GroupId { get; set; }
    }

    public class MessageModel : MessageBase
    {
        public int FromUserId { get; set; }
        public int MessageType { get; set; }
        public int ContentType { get; set; }
        public string Content { get; set; }
    }

    public class MessageSource
    {
        /// <summary>
        /// 要与mq中的组队列相对应
        /// </summary>
        public readonly static string messageQueueName = "group_queue";

        private MessageModel currMessageModel = null;

        public void Listener(Action<MessageModel> callBack)
        {
            MQManager.ReceiveContent(messageQueueName, (msg) =>
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    currMessageModel = JsonConvert.DeserializeObject<MessageModel>(msg);
                    callBack?.Invoke(currMessageModel);
                }
            });
        }

        public void Recovery()
        {
            if (currMessageModel == null) return;
            string msg = JsonConvert.SerializeObject(currMessageModel);
            MQManager.SendContent(messageQueueName, msg);
        }

        public void Distribute(List<string> queueNames)
        {
            if (currMessageModel == null) return;
            if (queueNames == null || queueNames.Count <= 0) return;

            foreach (var item in queueNames)
            {
                MQManager.SendContent(item, JsonConvert.SerializeObject(currMessageModel));
            }
        }
    }

    public class MQGroupMessageUnit
    {
        public static void AddGroupMessage()
        {
            int groupCount = 5;
            int groupMessageCount = 1000;

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
                    MQManager.SendContent(MessageSource.messageQueueName, JsonConvert.SerializeObject(model));
                }

            }
        }
    }
}
