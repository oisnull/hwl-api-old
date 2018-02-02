using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HWL.Redis
{
    /// <summary>
    /// 群组操作
    /// </summary>
    public class GroupAction : Client.RedisBase
    {

        public GroupAction()
            : base(0, RedisConfigService.UserDynamicRedisHosts)
        {
        }

        /*
         * 功能描述：
         * 1,存储组位置,格式：db=5 geo group:pos lat lon guid
         * 2,存储组用户集合,格式：db=6 set groupGuid userids
         * 
         */

        /// <summary>
        /// 组geo的key
        /// </summary>
        const string GROUP_GEO_KEY = "group:pos";
        /// <summary>
        /// 组所在的数据库
        /// </summary>
        const int GROUP_GEO_DB = 5;
        /// <summary>
        /// 组的用户set集合所在的数据库
        /// </summary>
        const int GROUP_USER_SET_DB = 6;
        ///// <summary>
        ///// 组的创建人的数据库
        ///// </summary>
        //const int GROUP_CREATE_USER_DB = 7;
        /// <summary>
        /// 搜索附近的范围初始值
        /// </summary>
        const int NEAR_RANGE = 100;

        /// <summary>
        /// 检测周围100M内的组数据,并返回对应的组标识列表
        /// </summary>
        public List<string> GetGroupGuids(double lon, double lat)
        {
            if (lon <= 0 || lat <= 0) return null;

            List<string> keys = null;
            base.DbNum = GROUP_GEO_DB;
            base.Exec(db =>
            {
                GeoRadiusResult[] results = db.GeoRadius(GROUP_GEO_KEY, lon, lat, NEAR_RANGE, GeoUnit.Miles, 1);
                if (results != null && results.Length > 0)
                {
                    keys = results.Select(s => s.Member.ToString()).ToList();
                }
            });

            return keys;
        }

        /// <summary>
        /// 检测周围100M内的组数据,并返回对应的组标识
        /// </summary>
        public string GetNearGroupGuid(double lon, double lat)
        {
            List<string> groupGuids = this.GetGroupGuids(lon, lat);
            if (groupGuids == null || groupGuids.Count <= 0) return null;

            foreach (var guid in groupGuids)
            {
                if (this.GetGroupUserCount(guid) < RedisConfigService.GroupUserTotalCount)
                {
                    return guid;
                }
            }

            return null;
        }

        public void SaveGroupUser(string groupGuid, params int[] userIds)
        {
            if (string.IsNullOrEmpty(groupGuid)) return;
            if (userIds == null || userIds.Length <= 0) return;

            base.DbNum = GROUP_USER_SET_DB;
            base.Exec(db =>
            {
                RedisValue[] array = new RedisValue[userIds.Length];
                for (int i = 0; i < userIds.Length; i++)
                {
                    array[i] = userIds[i];
                }
                db.SetAdd(groupGuid, array);
            });
        }

        public int GetGroupUserCount(string groupGuid)
        {
            if (string.IsNullOrEmpty(groupGuid)) return 0;
            base.DbNum = GROUP_USER_SET_DB;
            int count = 0;
            base.Exec(db =>
            {
                count = Convert.ToInt32(db.SetLength(groupGuid));
            });
            return count;
        }

        public List<int> GetGroupUserIds(string groupGuid)
        {
            if (string.IsNullOrEmpty(groupGuid)) return null;

            List<int> userIds = new List<int>();
            base.DbNum = GROUP_USER_SET_DB;
            base.Exec(db =>
            {
                RedisValue[] users = db.SetMembers(groupGuid);
                if (users != null && users.Length > 0)
                {
                    userIds.AddRange(users.Select(u =>
                    {
                        int uid;
                        u.TryParse(out uid);
                        return uid;
                    }).ToArray());
                }
            });

            return userIds;
        }

        ///// <summary>
        ///// 保存组用户
        ///// </summary>
        //public void SaveGroupUser(string groupGuid,params int[] userIds)
        //{
        //    if (string.IsNullOrEmpty(groupGuid)) return;
        //    if (userIds == null || userIds.Length <= 0) return;

        //    base.DbNum = GROUP_USER_SET_DB;
        //    base.Exec(db =>
        //    {
        //        RedisValue[] array = new RedisValue[userIds.Length];
        //        for (int i = 0; i < userIds.Length; i++)
        //        {
        //            array[i] = userIds[i];
        //        }
        //        db.SetAdd(groupGuid, array);
        //    });
        //}

        //public int GetGroupUserCount(string groupGuid)
        //{
        //    if (string.IsNullOrEmpty(groupGuid)) return 0;
        //    base.DbNum = GROUP_USER_SET_DB;
        //    int count = 0;
        //    base.Exec(db =>
        //    {
        //        count = Convert.ToInt32(db.SetLength(groupGuid));
        //    });
        //    return count;
        //}

        //public List<string> GetGroupUserIds(string groupGuid)
        //{
        //    if (string.IsNullOrEmpty(groupGuid)) return null;

        //    List<string> userIds = new List<string>();
        //    base.DbNum = GROUP_USER_SET_DB;
        //    base.Exec(db =>
        //    {
        //        RedisValue[] users = db.SetMembers(groupGuid);
        //        if (users != null && users.Length > 0)
        //        {
        //            userIds.AddRange(users.ToStringArray());
        //        }
        //    });

        //    return userIds;
        //}

        /// <summary>
        /// 创建组位置数据,返回创建成功后的组标识
        /// </summary>
        public string CreateGroupPos(int userId, double lon, double lat)
        {
            bool succ = false;
            base.DbNum = GROUP_GEO_DB;
            string guid = Guid.NewGuid().ToString();
            base.Exec(db =>
            {
                succ = db.GeoAdd(GROUP_GEO_KEY, lon, lat, guid);
            });
            if (succ)
            {
                return guid;
            }
            return null;
        }

        //#region 组的创建人操作

        //public bool SaveGroupCreateUser(string groupGuid,int userId)
        //{
        //    if (userId <= 0) return false;
        //    if (string.IsNullOrEmpty(groupGuid)) return false;

        //    bool succ = false;
        //    base.DbNum = GROUP_CREATE_USER_DB;
        //    base.Exec(db =>
        //    {
        //        succ = db.StringSet(groupGuid, userId.ToString());
        //    });
        //    return succ;
        //}

        //#endregion
    }
}
