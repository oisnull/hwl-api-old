using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient.Default
{
    public class DefaultClientConnectListener : IClientConnectListener
    {
        public void onBuildConnectionFailure(string clientAddress, string errorInfo)
        {
            Console.WriteLine("onBuildConnectionFailure : {0},{1}", clientAddress, errorInfo);
        }

        public void onBuildConnectionSuccess(string clientAddress, string serverAddress)
        {
            Console.WriteLine("onBuildConnectionSuccess : {0},{1}", clientAddress, serverAddress);
        }

        public void onClosed(string clientAddress)
        {
            Console.WriteLine("onClosed : " + clientAddress);
        }

        public void onConnected(string clientAddress, string serverAddress)
        {
            Console.WriteLine("onConnected : {0},{1}", clientAddress, serverAddress);
        }

        public void onDisconnected(string clientAddress)
        {
            Console.WriteLine("onDisconnected : " + clientAddress);
        }

        public void onError(string clientAddress, string errorInfo)
        {
            Console.WriteLine("onError : {0},{1}", clientAddress, errorInfo);
        }
    }
}
