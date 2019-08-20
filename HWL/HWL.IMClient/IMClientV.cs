using HWL.IMClient.Core;
using HWL.IMClient.Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.IMClient
{
    public class IMClientV
    {
        private static IMClientV instance = new IMClientV();
        private static string imHost;
        private static int imPort;
        private static IClientConnectListener clientConnectListener;

        public static void SetIMAddress(string host, int port)
        {
            imHost = host;
            imPort = port;
        }
        public static void SetConnectListener(IClientConnectListener connectListener)
        {
            clientConnectListener = connectListener;
        }

        public static IMClientV INSTANCE
        {
            get
            {
                return instance;
            }
        }

        private IMClientEngine im;

        private void checkConnect()
        {
            if (im == null || !im.isConnected())
            {
                im = new IMClientEngine(imHost, imPort);
                im.setConnectListener(clientConnectListener);
                im.connect();
            }
        }

        public void SendSystemMessage(ulong toUserId, string toUserName, string toGroupGuid)
        {
            checkConnect();

            im.send(new SystemMessageSend(toUserId, toUserName, toGroupGuid));
        }
    }
}
