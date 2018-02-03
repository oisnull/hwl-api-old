using HWL.MQGroupDistribute.message;
using HWL.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;

namespace HWL.MQGroupDistribute
{
    class Program
    {
        static void Main(string[] args)
        {
            double currTmr = 0;

            MQManager.registerConnectionStatusEvent(new ConnectionStatus());
            UserSource us = new UserSource();
            MessageSource ms = new MessageSource();
            int i = 1;
            ms.Listener((msgModel) =>
            {
                if (msgModel != null)
                {
                    var userQueueSymbols = us.GetUserQueueSymbolList(msgModel.GroupId);

                    DateTime beforDT = System.DateTime.Now;
                    ms.Distribute(userQueueSymbols);
                    DateTime afterDT = System.DateTime.Now;
                    TimeSpan ts = afterDT.Subtract(beforDT);
                    currTmr += ts.TotalMilliseconds;
                    Console.WriteLine(i + " current " + ts.TotalMilliseconds + " ms,total " + currTmr + " ms");
                }
                i++;
            });

            Console.ReadLine();
        }
    }

    public class ConnectionStatus : IConnectionStatus
    {
        public void OnBlocked(string exceptionInfo)
        {
            Console.WriteLine("OnBlocked : " + exceptionInfo);
        }

        public void OnBuildChannelError(string exceptionInfo)
        {
            Console.WriteLine("OnBuildChannelError : " + exceptionInfo);
        }

        public void OnBuildConnError(string exceptionInfo)
        {
            Console.WriteLine("OnBuildConnError : " + exceptionInfo);
        }

        public void OnClosed(Connection conn, int replyCode, string replyMessage)
        {
            Console.WriteLine("OnClosed : " + replyMessage);
        }

        public void OnConnectionSuccess(IConnection connection)
        {
            Console.WriteLine("OnConnectionSuccess : "+ connection.Endpoint.HostName+ "连接成功");
        }

        public void OnDisconnected(string exceptionInfo)
        {
            Console.WriteLine("OnDisconnected : " + exceptionInfo);
        }

        public void OnError(AutorecoveringConnection conn, Exception e)
        {
            Console.WriteLine("OnError : " + e.Message);
        }

        public void OnReconnected(AutorecoveringConnection conn)
        {
            Console.WriteLine("OnConnectionSuccess : 重新连接");
        }
    }
}
