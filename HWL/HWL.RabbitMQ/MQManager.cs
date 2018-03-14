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
        public readonly static string GROUP_QUEUE_NAME = "group-queue";
        public readonly static string HWL_DEFAULT_EXCHANGE = "hwl.amq.direct";
        public readonly static string HWL_EXCHANGE_MODEL = "direct";

        private static MQConnection mqconn;
        private static IModel sendChannel;
        private static IModel receiveChannel;
        private static IModel channel;
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

        public static void SendMessage(string queueName, byte[] messageBytes)
        {
            GetSendChannel().BasicPublish(HWL_DEFAULT_EXCHANGE, queueName, false, null, messageBytes);
        }

        public static void BindQueue(string queueName)
        {
            QueueDeclareOk queueInfo = GetReceiveChannel().QueueDeclare(queueName, true, false, false, null);
            receiveChannel.QueueBind(queueName, HWL_DEFAULT_EXCHANGE, queueName, null);
        }

        public static void ReceiveGroupMessage(Action<int, string, byte[]> succCallBack, Action<string> errorCallBackk = null)
        {
            BindQueue(GROUP_QUEUE_NAME);
            ReceiveMessage(GROUP_QUEUE_NAME, succCallBack, errorCallBackk);
        }

        public static void ReceiveMessage(string queueName, Action<int, string, byte[]> succCallBack, Action<string> errorCallBackk = null)
        {
            var consumer = new EventingBasicConsumer(GetReceiveChannel());
            receiveChannel.BasicConsume(queueName, false, consumer);
            //receiveChannel.BasicQos(0, 1, false);//这里指示一条条处理
            string groupId = string.Empty;
            byte[] groupIdBytes = null;
            byte[] userIdBytes = null;
            //byte[] msgBytes = null;

            consumer.Received += (model, e) =>
            {
                receiveChannel.BasicAck(e.DeliveryTag, false);
                if (e != null && e.Body != null && e.Body.Length > 0)
                {
                    try
                    {
                        //组消息格式：byte[]={chat-message-type,chat-send-user-id-length(byte),chat-group-guid-lenght(byte),chat-send-user-id(byte[]),chat-group-guid(byte[]),chat-message-content(byte[])}

                        //byte messageType = e.Body[0];
                        int fromUserIdLength = e.Body[1];
                        int groupIdLength = e.Body[2];

                        userIdBytes = new byte[fromUserIdLength];
                        groupIdBytes = new byte[groupIdLength];
                        //msgBytes = new byte[e.Body.Length - fromUserIdLength - groupIdLength - 3];

                        Array.Copy(e.Body, 3, userIdBytes, 0, fromUserIdLength);
                        Array.Copy(e.Body, 3 + fromUserIdLength, groupIdBytes, 0, groupIdLength);
                        //Array.Copy(e.Body, 3 + fromUserIdLength + groupIdLength, msgBytes, 0, msgBytes.Length);

                        succCallBack(BitConverter.ToInt32(userIdBytes, 0), Encoding.UTF8.GetString(groupIdBytes), e.Body);
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
                //mqconn.CloseChannel(channel);
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
                sendChannel.BasicReturn += Channel_BasicReturn;
            }
            return sendChannel;
        }

        private static void Channel_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            if (e.ReplyText == "NO_ROUTE")
            {
                //GetChannel().QueueDeclare(e.RoutingKey, HWL_DEFAULT_EXCHANGE, e.RoutingKey, null);
                ////重新发送
                //SendMessage(e.RoutingKey, e.Body);
            }

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

        public static IModel GetChannel()
        {
            if (mqconn == null)
            {
                InitConnection();
            }
            if (channel == null)
            {
                channel = mqconn.GetChannel();
            }
            return channel;
        }

        public static void RegisterConnectionStatusEvent(IConnectionStatus connectionStatus)
        {
            if (connectionStatus == null)
            {
                connectionStatus = new DefaultConnectionStatus();
            }
            try
            {
                connStatus = connectionStatus;
                GetReceiveChannel().ExchangeDeclare(HWL_DEFAULT_EXCHANGE, HWL_EXCHANGE_MODEL);
                GetSendChannel().ExchangeDeclare(HWL_DEFAULT_EXCHANGE, HWL_EXCHANGE_MODEL);
            }
            catch (Exception)
            {
            }
        }
    }
}
