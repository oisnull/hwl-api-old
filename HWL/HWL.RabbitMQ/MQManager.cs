using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.RabbitMQ
{
    public class MQManager
    {
        /// <summary>
        /// 要与mq中的组队列相对应
        /// </summary>
        public readonly static string GROUPQUEUENAME = "group-queue";

        private static MQConnection mqconn;
        private static IModel sendChannel;
        private static IModel receiveChannel;
        private static IConnectionStatus connStatus;
        private static object obj = new object();

        private MQManager()
        {
            InitConnection();
        }

        private static ConnectionFactory defaultConnectionFactory()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = MQConfigService.GroupRabbitMQHosts;
            factory.Port = MQConfigService.GroupRabbitMQHostsPort;
            factory.UserName = MQConfigService.GroupRabbitMQHostsUser;
            factory.Password = MQConfigService.GroupRabbitMQHostsPassword;
            return factory;
        }

        /**
         * 一个客户端只能维护一个长连接
         */
        private static void InitConnection()
        {
            if (mqconn == null)
            {
                lock (obj)
                {
                    if (mqconn == null)
                    {
                        mqconn = new MQConnection(defaultConnectionFactory(), connStatus);
                    }
                }
            }
        }

        public static IConnection BuildConnection()
        {
            if (mqconn == null)
            {
                InitConnection();
            }
            return mqconn.GetConnection();
        }

        public static void SendMessage(List<string> queueNames, byte[] messageBytes)
        {
            if (messageBytes == null || messageBytes.Length <= 0) return;
            if (queueNames == null || queueNames.Count <= 0) return;

            foreach (var item in queueNames)
            {
                SendMessage(item, messageBytes);
            }
        }

        public static void SendMessage(String queueName, byte[] messageBytes)
        {
            //GetSendChannel().QueueDeclare(queueName, true, false, false, null);

            GetSendChannel().BasicPublish("", queueName, null, messageBytes);
        }

        public static void ReceiveGroupMessage(Action<string, byte[]> succCallBack, Action<string> errorCallBackk = null)
        {
            ReceiveMessage(GROUPQUEUENAME, succCallBack, errorCallBackk);
        }

        public static void ReceiveMessage(String queueName, Action<string, byte[]> succCallBack, Action<string> errorCallBackk = null)
        {
            GetReceiveChannel().QueueDeclare(queueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(receiveChannel);
            receiveChannel.BasicConsume(queueName, false, consumer);
            //receiveChannel.BasicQos(0, 1, false);//这里指示一条条处理

            string groupId = string.Empty;
            byte[] groupIdBytes = null;
            byte[] msgBytes = null;

            consumer.Received += (model, e) =>
            {
                receiveChannel.BasicAck(e.DeliveryTag, false);
                if (e != null && e.Body != null && e.Body.Length > 0)
                {
                    try
                    {
                        //byte[]的第一位是groupid的长度
                        int groupIdLength = e.Body[0];
                        groupIdBytes = new byte[groupIdLength];
                        msgBytes = new byte[e.Body.Length - groupIdLength - 1];
                        Array.Copy(e.Body, 1, groupIdBytes, 0, groupIdLength);
                        Array.Copy(e.Body, groupIdBytes.Length + 1, msgBytes, 0, msgBytes.Length);

                        succCallBack(Encoding.UTF8.GetString(groupIdBytes), msgBytes);
                    }
                    catch (Exception ex)
                    {
                        if (errorCallBackk != null)
                            errorCallBackk(ex.Message);
                    }
                }
            };
        }

        public static void CloseMQConnection()
        {
            if (mqconn != null)
            {
                mqconn.CloseChannel(sendChannel);
                mqconn.CloseChannel(receiveChannel);
                mqconn.closeConnection();
                mqconn = null;
            }
        }

        //在当前连接下创建一个发送消息的通道
        public static IModel GetSendChannel()
        {
            if (mqconn == null)
            {
                InitConnection();
            }
            if (sendChannel == null)
            {
                sendChannel = mqconn.GetChannel();
            }
            return sendChannel;
        }

        //在当前连接下创建一个接收消息的通道
        public static IModel GetReceiveChannel()
        {
            if (mqconn == null)
            {
                InitConnection();
            }
            if (receiveChannel == null)
            {
                receiveChannel = mqconn.GetChannel();
            }
            return receiveChannel;
        }

        public static void registerConnectionStatusEvent(IConnectionStatus connectionStatus)
        {
            if (connStatus == null && connectionStatus != null)
            {
                connStatus = connectionStatus;
            }
        }
    }
}
