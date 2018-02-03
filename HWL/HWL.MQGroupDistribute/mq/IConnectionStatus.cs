using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;
using System;

namespace HWL.MQGroupDistribute.mq
{
    public interface IConnectionStatus
    {
        void OnBuildConnError(String exceptionInfo);//建立连接失败

        void OnBuildChannelError(String exceptionInfo);//建立通道失败

        void OnDisconnected(String exceptionInfo);//连接断开后

        void OnBlocked(String exceptionInfo);

        void OnConnectionSuccess(IConnection connection);
        void OnError(AutorecoveringConnection conn, Exception e);//连接错误或者连接恢复错误

        void OnClosed(Connection conn, int replyCode, string replyMessage);//连接关闭后
        void OnReconnected(AutorecoveringConnection conn);//重新连接后
    }
}
