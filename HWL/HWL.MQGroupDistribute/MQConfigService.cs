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
        public static int GroupRabbitMQHostsPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["GroupRabbitMQHostsPort"]);
            }
        }
        public static string GroupRabbitMQHostsUser
        {
            get
            {
                return ConfigurationManager.AppSettings["GroupRabbitMQHostsUser"].ToString();
            }
        }
        public static string GroupRabbitMQHostsPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["GroupRabbitMQHostsPassword"].ToString();
            }
        }
    }
}
