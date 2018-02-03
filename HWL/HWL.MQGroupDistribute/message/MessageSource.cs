using HWL.RabbitMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute.message
{
   

    //public class MessageSource
    //{
    //    /// <summary>
    //    /// 要与mq中的组队列相对应
    //    /// </summary>
    //    public readonly static string messageQueueName = "group-queue";

    //    private MessageModel currMessageModel = null;

    //    public void Listener(Action<string, byte[]> succCallBack, Action<string> errorCallBackk = null)
    //    {
    //        MQManager.ReceiveMessage(messageQueueName, succCallBack, errorCallBackk);
    //    }

    //    public void Recovery()
    //    {
    //        if (currMessageModel == null) return;
    //        string msg = JsonConvert.SerializeObject(currMessageModel);
    //        MQManager.SendMessage(messageQueueName, msg);
    //    }

    //    public void Distribute(List<string> queueNames)
    //    {
    //        if (currMessageModel == null) return;
    //        if (queueNames == null || queueNames.Count <= 0) return;

    //        foreach (var item in queueNames)
    //        {
    //            MQManager.SendMessage(item, JsonConvert.SerializeObject(currMessageModel));
    //        }
    //    }
    //}

    //public class MQGroupMessageUnit
    //{
    //    public static void AddGroupMessage()
    //    {
    //        int groupCount = 5;
    //        int groupMessageCount = 1000;

    //        for (int i = 1; i < groupCount; i++)
    //        {
    //            for (int j = 1; j <= groupMessageCount; j++)
    //            {
    //                var model = new MessageModel()
    //                {
    //                    GroupId = "group-guid-" + i,
    //                    Content = "group message test " + j,
    //                    ContentType = 1,
    //                    FromUserId = 1,
    //                    MessageType = 1,
    //                };
    //                MQManager.SendMessage(MQManager.GROUPQUEUENAME, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
    //            }

    //        }
    //    }
    //}
}
