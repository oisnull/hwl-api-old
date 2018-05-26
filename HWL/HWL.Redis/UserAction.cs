using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace HWL.Redis
{
    public class UserAction : Client.RedisBase
    {
        public UserAction()
            : base(0, RedisConfigService.UserDynamicRedisHosts)
        {
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

        const string USER_GEO_KEY = "user:pos";
        const int USER_TOKEN_DB = 00;
        const int TOKEN_USER_DB = 01;
        const int USER_SESSION_DB = 02;
        //const int USER_CREAT_GROUP_DB = 03;
        const int USER_GEO_DB = 04;
        const int USER_BASEINFO_DB = 07;
        const int USER_FRIEND_DB = 08;

        const int USER_BASERINFO_ERPIRE_TIME = 30;//用户基本信息过期时间配置，单位：分钟
        const int USER_FRIEND_ERPIRE_TIME = 30;//用户对应的好友信息过期时间配置，单位：分钟

        /// <summary>
        /// 搜索附近用户的范围
        /// </summary>
        const int USER_SEARCH_RANGE = 1000;

        #region 用户sessioin操作
        //存储用户会话状态(key为用户id)
        public bool SaveSession(int userId, string sessionId)
        {
            if (userId <= 0) return false;
            if (string.IsNullOrEmpty(sessionId)) return false;
            base.DbNum = USER_SESSION_DB;
            bool succ = false;
            Exec(db =>
            {
                succ = db.StringSet(userId.ToString(), sessionId);
            });
            return succ;
        }

        public bool DeleteSession(int userId)
        {
            if (userId <= 0) return false;

            base.DbNum = USER_SESSION_DB;
            bool succ = false;
            Exec(db =>
            {
                succ = db.KeyDelete(userId.ToString());
            });
            return succ;
        }

        /// <summary>
        /// 获取用户会话id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserSessionId(int userId)
        {
            if (userId <= 0) return null;

            base.DbNum = USER_SESSION_DB;
            string sessionId = null;
            Exec(db =>
            {
                sessionId = db.StringGet(userId.ToString());
            });
            return sessionId;
        }

        /// <summary>
        /// 获取用户会话列表（如果用户的会话不存在,则列表里面会存在null数据）
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<string> GetUserSessionIds(List<string> userIds)
        {
            if (userIds == null || userIds.Count <= 0) return null;

            List<string> sessions = new List<string>();
            base.DbNum = USER_SESSION_DB;
            base.Exec(db =>
            {
                RedisValue[] values = db.StringGet(userIds.ConvertAll(u => (RedisKey)u).ToArray());
                if (values != null && values.Length > 0)
                {
                    sessions.AddRange(values.ToStringArray());
                }
            });
            return sessions;
        }

        #endregion

        #region 用户位置操作 (redis地理位置的坐标是以WGS84为标准，WGS84，全称World Geodetic System 1984，是为GPS全球定位系统使用而建立的坐标系统)

        //存储用户当前在线的位置信息
        public bool SavePos(int userId, double lon, double lat)
        {
            if (userId <= 0) return false;
            if (lon <= 0) return false;
            if (lat <= 0) return false;

            base.DbNum = USER_GEO_DB;
            bool succ = false;
            base.Exec(db =>
            {
                succ = db.GeoAdd(USER_GEO_KEY, lon, lat, userId.ToString());
            });
            return succ;
        }

        //获取附近的用户列表
        public int[] GetNearUserList(double lon, double lat)
        {
            int[] userIdArray = null;
            base.DbNum = USER_GEO_DB;
            base.Exec(db =>
            {
                GeoRadiusResult[] results = db.GeoRadius(USER_GEO_KEY, lon, lat, USER_SEARCH_RANGE, GeoUnit.Miles);
                if (results != null && results.Length > 0)
                {
                    userIdArray = new int[results.Length];
                    for (int i = 0; i < results.Length; i++)
                    {
                        userIdArray[i] = Convert.ToInt32(results[i].Member);
                    }
                }
            });
            return userIdArray;
        }

        #endregion

        #region 用户token操作

        //public string createUserToken(int userId)
        //{
        //    if (userId <= 0) return null;

        //    string token = Guid.NewGuid().ToString();
        //    bool succ = false;
        //    base.DbNum = USER_TOKEN_DB;
        //    base.Exec(db =>
        //    {
        //        //直接存token,不管里面有没有
        //        if (db.StringSet(userId.ToString(), token))
        //        {
        //            succ = this.SaveTokenUser(userId, token);
        //        }
        //    });

        //    if (succ)
        //    {
        //        return token;
        //    }
        //    return null;
        //}

        /// <summary>
        /// 保存用户登录票据,保存成功后返回token(key为用户id)
        /// userid:token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SaveUserToken(int userId, string token)
        {
            if (userId <= 0) return false;
            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token)) return false;
            bool succ = false;
            base.DbNum = USER_TOKEN_DB;
            base.Exec(db =>
            {
                //直接存token,不管里面有没有
                if (db.StringSet(userId.ToString(), token))
                {
                    succ = this.SaveTokenUser(userId, token);
                }
            });
            return succ;
        }

        //token:userid
        private bool SaveTokenUser(int userId, string token, string oldToken = null)
        {
            base.DbNum = TOKEN_USER_DB;
            bool succ = false;
            base.Exec(db =>
            {
                if (!string.IsNullOrEmpty(oldToken))
                {
                    db.KeyDelete(oldToken);
                }
                succ = db.StringSet(token, userId.ToString());
            });
            return succ;
        }

        public int GetTokenUser(string token)
        {
            if (string.IsNullOrEmpty(token)) return 0;

            base.DbNum = TOKEN_USER_DB;
            int userId = 0;
            base.Exec(db =>
            {
                string id = db.StringGet(token);
                if (!string.IsNullOrEmpty(id))
                {
                    userId = Convert.ToInt32(id);
                }
            });
            return userId;
        }

        public string GetUserToken(int userId)
        {
            if (userId <= 0) return null;

            base.DbNum = USER_TOKEN_DB;
            string token = null;
            base.Exec(db =>
            {
                token = db.StringGet(userId.ToString());
            });
            return token;
        }

        //后期需要应用事务
        public bool RemoveUserToken(int userId)
        {
            if (userId <= 0) return false;
            string token = GetUserToken(userId);
            if (string.IsNullOrEmpty(token)) return true;

            base.DbNum = USER_TOKEN_DB;
            bool succ = false;
            base.Exec(db =>
            {
                succ = db.KeyDelete(userId.ToString());
            });

            base.DbNum = TOKEN_USER_DB;
            base.Exec(db =>
            {
                succ = db.KeyDelete(token);
            });
            return succ;
        }

        #endregion

        #region 用户group操作
        ///// <summary>
        ///// 根据用户id获取组标识
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public string GetGroupByUserId(int userId)
        //{
        //    if (userId <= 0) return null;
        //    base.DbNum = USER_CREAT_GROUP_DB;
        //    string groupGuid = null;
        //    Exec(db =>
        //    {
        //        groupGuid = db.StringGet(userId.ToString());
        //    });
        //    return groupGuid;
        //}

        //public Dictionary<int, string> GetGroupGuids(List<int> userIds)
        //{
        //    if (userIds == null || userIds.Count <= 0) return null;

        //    Dictionary<int, string> groupGuids = new Dictionary<int, string>();
        //    base.DbNum = USER_CREAT_GROUP_DB;
        //    base.Exec(db =>
        //    {
        //        RedisValue[] values = db.StringGet(userIds.ConvertAll(u => (RedisKey)u.ToString()).ToArray());
        //        if (values != null && values.Length > 0)
        //        {
        //            //groupGuids.AddRange(values.ToStringArray());
        //            for (int i = 0; i < userIds.Count; i++)
        //            {
        //                groupGuids.Add(userIds[i], values[i]);
        //            }
        //        }
        //    });
        //    return groupGuids;
        //}

        ///// <summary>
        ///// 存储这个组是哪个用户创建的
        ///// </summary>
        ///// <returns></returns>
        //public bool CreateUserGroup(int userId, string groupGuid)
        //{
        //    if (userId <= 0) return false;
        //    if (string.IsNullOrEmpty(groupGuid)) return false;
        //    base.DbNum = USER_CREAT_GROUP_DB;
        //    bool succ = false;
        //    Exec(db =>
        //    {
        //        succ = db.StringSet(userId.ToString(), groupGuid);
        //    });

        //    return succ;
        //}

        #endregion

        #region 用户基本信息操作

        ///// <summary>
        ///// 获取用户基本信息(infos:[name,headimage])
        ///// </summary>
        //public List<string> GetUserInfo(int userId)
        //{
        //    if (userId <= 0) return null;

        //    List<string> infos = null;

        //    base.DbNum = USER_BASEINFO_DB;
        //    base.Exec(db =>
        //    {
        //        string infoStr = db.StringGet(userId.ToString());
        //        if (!string.IsNullOrEmpty(infoStr))
        //        {
        //            infos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(infoStr);
        //        }
        //    });

        //    return infos;
        //}

        ///// <summary>
        ///// 添加用户基本信息(infos:[name,headimage])
        ///// </summary>
        //public bool SaveUserInfo(int userId, List<string> infos)
        //{
        //    if (userId <= 0) return false;
        //    if (infos == null || infos.Count <= 0) return false;

        //    base.DbNum = USER_BASEINFO_DB;
        //    bool succ = false;
        //    base.Exec(db =>
        //    {
        //        succ = db.StringSet(userId.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(infos), new TimeSpan(0, USER_BASERINFO_ERPIRE_TIME, 0));
        //    });
        //    return succ;
        //}

        /// <summary>
        /// 设置备注过期
        /// </summary>
        public bool SetUserInfoExpire(int userId)
        {
            if (userId <= 0) return false;
            base.DbNum = USER_BASEINFO_DB;
            bool succ = false;
            base.Exec(db =>
            {
                succ = db.StringSet(userId.ToString(), "", new TimeSpan(0, 0, 0, 0, 1));
            });
            return succ;
        }

        #endregion

        #region 用户好友信息操作

        /// <summary>
        /// 获取好友的备注信息,(我给用户的备注,用户给我的备注)
        /// </summary>
        public string[] GetFriendRemark(int userId, int fuserId)
        {
            if (userId <= 0 || fuserId <= 0) return null;

            string[] remarks = null;
            base.DbNum = USER_FRIEND_DB;
            base.Exec(db =>
            {
                RedisKey[] keys = new RedisKey[2];
                keys[0] = string.Format("{0}:{1}", userId, fuserId);
                keys[1] = string.Format("{0}:{1}", fuserId, userId);
                RedisValue[] values = db.StringGet(keys);
                if (values != null && values.Length > 0)
                {
                    remarks = values.ToStringArray();
                }
            });
            return remarks;
        }

        /// <summary>
        /// 保存好友备注信息
        /// </summary>
        public bool SaveFriendRemark(int userId, int fuserId, string fremark)
        {
            if (userId <= 0 || fuserId <= 0 || string.IsNullOrEmpty(fremark)) return false;

            base.DbNum = USER_FRIEND_DB;
            bool succ = false;
            base.Exec(db =>
            {
                string key = string.Format("{0}:{1}", userId, fuserId);
                succ = db.StringSet(key, fremark, new TimeSpan(0, USER_FRIEND_ERPIRE_TIME, 0));
            });
            return succ;
        }

        /// <summary>
        /// 设置备注过期
        /// </summary>
        public bool SetFriendRemarkExpire(int userId, int fuserId)
        {
            if (userId <= 0 || fuserId <= 0) return false;
            base.DbNum = USER_FRIEND_DB;
            bool succ = false;
            base.Exec(db =>
            {
                string key = string.Format("{0}:{1}", userId, fuserId);
                succ = db.StringSet(key, "", new TimeSpan(0, 0, 0, 0, 1));
            });
            return succ;
        }

        #endregion
    }
}
