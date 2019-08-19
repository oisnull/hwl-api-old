using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient
{
    public interface IClientChannelListener
    {
        void onConnected(string clientAddress, string serverAddress);

        void onDisconnected(string clientAddress);

        void onError(string clientAddress, string errorInfo);
    }
}
