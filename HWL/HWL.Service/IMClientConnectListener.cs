using HWL.IMClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Service
{
    public class IMClientConnectListener : IClientConnectListener
    {
        public void onBuildConnectionFailure(string clientAddress, string errorInfo)
        {
        }

        public void onBuildConnectionSuccess(string clientAddress, string serverAddress)
        {
        }

        public void onClosed(string clientAddress)
        {
        }

        public void onConnected(string clientAddress, string serverAddress)
        {
        }

        public void onDisconnected(string clientAddress)
        {
        }

        public void onError(string clientAddress, string errorInfo)
        {
        }
    }
}