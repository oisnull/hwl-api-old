using HWL.MQGroupDistribute.message;
using HWL.RabbitMQ;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;

namespace HWL.MQGroupDistribute
{
    class Program
    {


        static void Main(string[] args)
        {
            //ByteTest();

            Listener();

            //AddTestData();

            Console.ReadLine();
        }

        private static void AddTestData()
        {
            try
            {
                MQGroupMessageUnit.AddGroupUser();
                MQGroupMessageUnit.AddGroupMessage();
                Console.WriteLine("测试数据添加完成...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Listener()
        {
            double currTmr = 0;
            MQManager.RegisterConnectionStatusEvent(new ConnectionStatus());
            UserSource us = new UserSource();
            int i = 1;
            MQManager.ReceiveGroupMessage((fromUserId, groupId, messageBytes) =>
            {
                if (!string.IsNullOrEmpty(groupId) && messageBytes != null && messageBytes.Length > 0)
                {
                    DateTime beforDT = System.DateTime.Now;

                    var userQueueSymbols = us.GetUserQueueSymbolList(fromUserId,groupId);
                    MQManager.SendMessage(userQueueSymbols, messageBytes);

                    DateTime afterDT = System.DateTime.Now;
                    TimeSpan ts = afterDT.Subtract(beforDT);
                    currTmr += ts.TotalMilliseconds;
                    Console.WriteLine(i + " current " + ts.TotalMilliseconds + " ms,total " + currTmr + " ms");
                }
                i++;
            }, (error) =>
            {
                Console.WriteLine(error);
            });
        }

        private static void ByteTest()
        {
            byte[] first = { 22, 33 };
            byte[] second = { 44, 55, 66 };
            //resArr为合并后数组
            byte[] resArr = new byte[first.Length + second.Length + 1];
            resArr[0] = 11;
            first.CopyTo(resArr, 1);
            second.CopyTo(resArr, first.Length + 1);
            foreach (var item in resArr)
            {
                Console.WriteLine(item);
            }

            byte[] b = { 11, 22, 33, 44, 55, 66, 77, 88 };
            int gidlen = 3;
            byte[] gid = new byte[gidlen];
            byte[] ret = new byte[b.Length - gid.Length - 1];
            Array.Copy(b, 1, gid, 0, gidlen);
            Array.Copy(b, gid.Length + 1, ret, 0, ret.Length);

            foreach (var item in ret)
            {
                Console.WriteLine(item);
            }
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
            Console.WriteLine("OnConnectionSuccess : " + connection.Endpoint.HostName + " 连接成功");
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
