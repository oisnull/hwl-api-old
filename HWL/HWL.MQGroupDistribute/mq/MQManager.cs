using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace HWL.MQGroupDistribute.mq
{

    public class MQManager
    {

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

        public static IConnection buildConnection()
        {
            if (mqconn == null)
            {
                InitConnection();
            }
            return mqconn.GetConnection();
        }


        public static void SendMessage(String queueName, String messageContent)
        {
            GetSendChannel().QueueDeclare(queueName, true, false, false, null);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(messageContent);
            sendChannel.BasicPublish("", queueName, null, messageBodyBytes);
        }

        public static void ReceiveMessage(String queueName, Action<string> succCallBack)
        {
            GetReceiveChannel().QueueDeclare(queueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(receiveChannel);
            receiveChannel.BasicConsume(queueName, false, consumer);
            //receiveChannel.BasicQos(0, 1, false);//这里指示一条条处理
            consumer.Received += (model, e) =>
            {
                receiveChannel.BasicAck(e.DeliveryTag, false);
                if (e != null && e.Body != null && e.Body.Length > 0)
                {
                    string msg = Encoding.UTF8.GetString(e.Body);
                    succCallBack(msg);
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
