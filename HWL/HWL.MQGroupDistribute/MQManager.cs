using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;

namespace HWL.MQGroupDistribute
{
    public class MQManager
    {
        private static IConnection conn;
        private static IModel sendChannel;
        private static IModel receiveChannel;
        private static object obj = new object();
        private static IConnectionStatus connStatus;

        static MQManager()
        {
            BuildConnection();
            InitChannel();
        }

        private MQManager() { }

        private static ConnectionFactory GetDefaultConnectionFactory()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = MQConfigService.GroupRabbitMQHosts;
            return factory;
        }

        public static bool SendContent(string queueName, string message)
        {
            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
            {
                return false;
            }

            var body = Encoding.UTF8.GetBytes(message);
            try
            {
                sendChannel.BasicPublish(exchange: "",
                                    routingKey: queueName,
                                    basicProperties: null,
                                    body: body);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //sendChannel.QueueDeclare(queueName, true, false, false, null);
                //return false;
            }
        }

        public static void ReceiveContent(string queueName, Action<string> succCallBack)
        {
            receiveChannel.QueueDeclare(queueName, true, false, false, null);
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

        public static void RegisterConnectionStatusEvent(IConnectionStatus connectionStatus)
        {
            if (connStatus == null && connectionStatus != null)
            {
                connStatus = connectionStatus;
            }
        }

        /// <summary>
        /// 一个客户端只能维护一个长连接
        /// </summary>
        /// <returns></returns>
        private static IConnection BuildConnection()
        {
            if (conn == null)
            {
                lock (obj)
                {
                    if (conn == null)
                    {
                        conn = GetDefaultConnectionFactory().CreateConnection();
                        BindConnectionEvent();

                    }
                }
            }

            return conn;
        }

        /// <summary>
        /// 一个客户端只能有一个数据通道
        /// </summary>
        /// <returns></returns>
        private static void InitChannel()
        {
            if (receiveChannel == null)
            {
                lock (obj)
                {
                    if (receiveChannel == null)
                    {
                        receiveChannel = BuildConnection().CreateModel();
                    }
                }
            }
            if (sendChannel == null)
            {
                lock (obj)
                {
                    if (sendChannel == null)
                    {
                        sendChannel = BuildConnection().CreateModel();
                    }
                }
            }

        }

        public static void CloseChannel()
        {
            if (sendChannel != null)
            {
                if (!sendChannel.IsOpen)
                {
                    sendChannel.Close();
                }
                else
                {
                    sendChannel = null;
                }
            }
            if (receiveChannel != null)
            {
                if (!receiveChannel.IsOpen)
                {
                    receiveChannel.Close();
                }
                else
                {
                    receiveChannel = null;
                }
            }
        }

        public static void CloseConnection()
        {
            //先关闭通道
            CloseChannel();

            //再关闭连接
            if (conn != null)
            {
                if (conn.IsOpen)
                {
                    conn.Close();
                }
                else
                {
                    conn = null;
                }
            }
        }

        private static void BindConnectionEvent()
        {
            if (conn != null)
            {
                conn.CallbackException += (sender, e) =>
                {
                    if (connStatus != null)
                    {
                        //connStatus.OnError();
                    }
                };
                conn.ConnectionBlocked += (sender, e) =>
                {
                    if (connStatus != null)
                    {
                        //connStatus.OnDisconnected();
                    }
                };
                conn.ConnectionRecoveryError += (sender, e) =>
                {
                    if (connStatus != null)
                    {
                        connStatus.OnError((AutorecoveringConnection)sender, e.Exception);
                    }
                };
                conn.ConnectionShutdown += (sender, e) =>
                {
                    if (connStatus != null)
                    {
                        connStatus.OnClosed((Connection)sender, e.ReplyCode, e.ReplyText);
                    }
                };
                conn.RecoverySucceeded += (sender, e) =>
                {
                    if (connStatus != null)
                    {
                        connStatus.OnReconnected((AutorecoveringConnection)sender);
                    }
                };
                conn.ConnectionUnblocked += (sender, e) =>
                {
                    if (connStatus != null)
                    {
                        //connStatus.OnSuccess();
                    }
                };
            }
        }
    }

    public interface IConnectionStatus
    {
        void OnError(AutorecoveringConnection conn, Exception e);//连接错误或者连接恢复错误
        void OnDisconnected();//连接断开后
        void OnClosed(Connection conn, int replyCode, string replyMessage);//连接关闭后
        void OnSuccess();//连接成功后
        void OnReconnected(AutorecoveringConnection conn);//重新连接后
    }
}
