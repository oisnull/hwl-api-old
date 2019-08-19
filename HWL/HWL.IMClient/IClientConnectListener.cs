using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient
{
    public interface IClientConnectListener : IClientChannelListener
    {
        void onBuildConnectionSuccess(string clientAddress, string serverAddress);

        void onBuildConnectionFailure(string clientAddress,  string errorInfo);

        void onClosed(string clientAddress);
    }
}
