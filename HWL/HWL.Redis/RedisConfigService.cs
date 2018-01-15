using System;
using System.Configuration;

namespace HWL.Redis
{
    public class RedisConfigService
    {
        /// <summary>
        /// 用户动态配置
        /// </summary>
        public static string UserDynamicRedisHosts
        {
            get
            {
                return ConfigurationManager.AppSettings["UserDynamicRedisHosts"].ToString();
            }
        }

        /// <summary>
        /// 群组好友总数量
        /// </summary>
        public static int GroupUserTotalCount
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["GroupUserTotalCount"]);
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
