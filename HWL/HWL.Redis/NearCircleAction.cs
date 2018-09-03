using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Redis
{
    public class NearCircleAction : Client.RedisBase
    {
        public NearCircleAction() : base(0, RedisConfigService.UserDynamicRedisHosts) { }

        /// <summary>
        /// 搜索附近信息的范围初始值
        /// </summary>
        public const int NEAR_CIRCLE_RANGE = 1000;

        /// <summary>
        /// 创建附近圈子信息位置数据,返回创建成功后的组标识
        /// </summary>
        public bool CreateNearCirclePos(int circleId, double lon, double lat)
        {
            bool succ = false;
            base.DbNum = RedisConfigService.NEAR_CIRCLE_GEO_DB;
            base.Exec(db =>
            {
                succ = db.GeoAdd(RedisConfigService.NEAR_CIRCLE_GEO_KEY, lon, lat, circleId);
            });
            return succ;
        }

        /// <summary>
        /// 检测附近1000M内的组数据,并返回对应的组标识
        /// </summary>
        public List<int> GetNearCircleIds(double lon, double lat)
        {
            if (lon < 0 && lat < 0) return null;

            List<int> ids = null;
            base.DbNum = RedisConfigService.NEAR_CIRCLE_GEO_DB;
            base.Exec(db =>
            {
                GeoRadiusResult[] results = db.GeoRadius(RedisConfigService.NEAR_CIRCLE_GEO_KEY, lon, lat, NEAR_CIRCLE_RANGE, GeoUnit.Miles, -1);
                if (results != null && results.Length > 0)
                {
                    ids = results.Select(s => (int)s.Member).OrderByDescending(s => s).ToList();
                }
            });

            return ids;
        }

        public bool DeleteNearCircleId(int nearCircleId)
        {
            if (nearCircleId <= 0) return false;
            bool succ = false;
            base.DbNum = RedisConfigService.NEAR_CIRCLE_GEO_DB;
            base.Exec(db =>
            {
                succ = db.GeoRemove(RedisConfigService.NEAR_CIRCLE_GEO_KEY, nearCircleId);
            });
            return succ;
        }
    }
}
