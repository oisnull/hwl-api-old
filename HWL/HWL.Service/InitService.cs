using HWL.IMClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service
{
    public class InitService
    {
        public static void Process()
        {
            IMClientV.SetIMAddress(ConfigManager.IMHost, ConfigManager.IMPort);
            IMClientV.SetConnectListener(new IMClientConnectListener());
        }
    }
}
