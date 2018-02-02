using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute
{
    public class MQConfigService
    {
        public static string GroupRabbitMQHosts
        {
            get
            {
                return ConfigurationManager.AppSettings["GroupRabbitMQHosts"].ToString();
            }
        }
    }
}
