using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute.mq
{
    public class MQConnection
    {

        private IConnectionStatus connectionStatus;
        private ConnectionFactory factory;
        private IConnection currConnection;

        public MQConnection(ConnectionFactory factory, IConnectionStatus connectionStatus)
        {
            this.connectionStatus = connectionStatus;
            this.factory = factory;
            if (this.connectionStatus == null)
                throw new Exception("连接状态监听处理过程不能为空");
        }

        public IConnection GetConnection()
        {
            if (currConnection == null)
            {
                try
                {
                    currConnection = factory.CreateConnection();
                    connectionStatus.OnConnectionSuccess(currConnection);
                    BindConnectionEvent();
                }
                catch (Exception e)
                {
                    connectionStatus.OnBuildConnError(e.Message);
                }
            }
            return currConnection;
        }

        public IModel GetChannel()
        {
            if (currConnection == null)
            {
                currConnection = GetConnection();
            }
            else if (!currConnection.IsOpen)
            {
                connectionStatus.OnBuildConnError("MQ连接已经断开");
                return null;
            }

            IModel channel = null;
            try
            {
                channel = currConnection.CreateModel();
            }
            catch (Exception e)
            {
                connectionStatus.OnBuildChannelError(e.Message);
            }
            return channel;
        }

        public void BindConnectionEvent()
        {
            currConnection.CallbackException += (sender, e) =>
            {
                connectionStatus.OnError(null, e.Exception);
            };
            currConnection.ConnectionBlocked += (sender, e) =>
            {
                if (connectionStatus != null)
                {
                    //connStatus.OnDisconnected();
                }
            };
            currConnection.ConnectionRecoveryError += (sender, e) =>
            {
                if (connectionStatus != null)
                {
                    connectionStatus.OnError((AutorecoveringConnection)sender, e.Exception);
                }
            };
            currConnection.ConnectionShutdown += (sender, e) =>
            {
                if (connectionStatus != null)
                {
                    connectionStatus.OnClosed((Connection)sender, e.ReplyCode, e.ReplyText);
                }
            };
            currConnection.RecoverySucceeded += (sender, e) =>
            {
                if (connectionStatus != null)
                {
                    connectionStatus.OnReconnected((AutorecoveringConnection)sender);
                }
            };
            currConnection.ConnectionUnblocked += (sender, e) =>
            {
                if (connectionStatus != null)
                {
                    //connStatus.OnSuccess();
                }
            };
        }

        public void CloseChannel(IModel channel)
        {
            if (channel != null)
            {
                if (channel.IsOpen)
                {
                    try
                    {
                        channel.Close();
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        channel = null;
                    }
                }
                else
                {
                    channel = null;
                }
            }
        }

        public void closeConnection()
        {
            if (currConnection != null)
            {
                if (currConnection.IsOpen)
                {
                    try
                    {
                        currConnection.Close();
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        currConnection = null;
                    }
                }
                else
                {
                    currConnection = null;
                }
            }
        }

    }
}
