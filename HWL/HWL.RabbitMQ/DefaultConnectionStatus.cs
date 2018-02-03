using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;

namespace HWL.RabbitMQ
{
    public class DefaultConnectionStatus : IConnectionStatus
    {
        public void OnBlocked(string exceptionInfo)
        {
        }

        public void OnBuildChannelError(string exceptionInfo)
        {
        }

        public void OnBuildConnError(string exceptionInfo)
        {
        }

        public void OnClosed(Connection conn, int replyCode, string replyMessage)
        {
        }

        public void OnConnectionSuccess(IConnection connection)
        {
        }

        public void OnDisconnected(string exceptionInfo)
        {
        }

        public void OnError(AutorecoveringConnection conn, Exception e)
        {
        }

        public void OnReconnected(AutorecoveringConnection conn)
        {
        }
    }
}
