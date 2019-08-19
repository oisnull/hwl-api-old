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


        /*
         * 功能描述：
         * 1,存储用户的token,格式：db=0 set userid token
         * 1.1,存储token对应的用户,格式：db=1 set token userid
         * 
         * 2,存储用户的sessionid,格式：db=2 set userid sessionid
         * 3,存储组所属用户的标识,格式：db=3 set userid groupGuid
         * 
         * 4,存储用户的位置pos,格式：db=4 geo user:pos lat lon userid
         * 
         * 5,存储用户的基本信息,格式:db=7 set userid [name,headimage] 过期时间为 5分钟
         * 6,存储用户好友信息,格式：db=8 set userid:fuserid remark 过期时间为 5分钟
         */

        public const string USER_GEO_KEY = "user:pos";
        public const int USER_TOKEN_DB = 00;
        public const int TOKEN_USER_DB = 01;
        public const int USER_SESSION_DB = 02;
        //const int USER_CREAT_GROUP_DB = 03;
        public const int USER_GEO_DB = 04;
        public const int USER_BASEINFO_DB = 07;
        public const int USER_FRIEND_DB = 08;
        public const int USER_OFFLINE_MESSAGE_DB = 09;

        public const int USER_BASERINFO_ERPIRE_TIME = 30;//用户基本信息过期时间配置，单位：分钟
        public const int USER_FRIEND_ERPIRE_TIME = 30;//用户对应的好友信息过期时间配置，单位：分钟



        /// <summary>
        /// 附近圈子信息geo的key
        /// </summary>
        public const string NEAR_CIRCLE_GEO_KEY = "near:circle:pos";

        /// <summary>
        /// 附近圈子信息所在的数据库
        /// </summary>
        public const int NEAR_CIRCLE_GEO_DB = 20;

        /*
         * 功能描述：
         * 1,存储组位置,格式：db=5 geo group:pos lat lon guid
         * 2,存储组用户集合,格式：db=6 set groupGuid userids
         * 
         */

        /// <summary>
        /// 组geo的key
        /// </summary>
        public const string GROUP_GEO_KEY = "group:pos";
        /// <summary>
        /// 组所在的数据库
        /// </summary>
        public const int GROUP_GEO_DB = 10;
        /// <summary>
        /// 组的用户set集合所在的数据库
        /// </summary>
        public const int GROUP_USER_SET_DB = 11;
        /// <summary>
        /// 个人组所在的数据库
        /// </summary>
        //public const int GROUP_DB = 12;

        /// <summary>
        /// 用户离线消息数据库
        /// </summary>
        public const int OFFLINE_MESSAGE_DB = 9;

    }
}
