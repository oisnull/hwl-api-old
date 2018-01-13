using System;
using System.Configuration;

namespace HWL.Redis
{
    public class RedisConfigService
    {
        /// <summary>
        /// 用户group,token,session,pos地址配置
        /// </summary>
        public static string UserDynamicRedisHosts
        {
            get
            {
                return ConfigurationManager.AppSettings["UserDynamicRedisHosts"].ToString();
            }
        }
        ///// <summary>
        ///// 用户基本信息地址配置
        ///// </summary>
        //public static string UserBaseInfoRedisHosts
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["UserBaseInfoRedisHosts"].ToString();
        //    }
        //}
        /// <summary>
        /// 消息处理地址配置
        /// </summary>
        public static string MsgHandlerRedisHosts
        {
            get
            {
                return ConfigurationManager.AppSettings["MsgHandlerRedisHosts"].ToString();
            }
        }
    }
}
