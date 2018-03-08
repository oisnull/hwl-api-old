using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.Near.Body
{
    public class GetNearCircleInfosRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }
        public int LastNearCiclreId { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        /// <summary>
        /// 获取数据的条数
        /// </summary>
        public int Count { get; set; }
    }

    public class GetNearCircleInfosResponseBody
    {
        public List<NearCircleInfo> NearCircleInfos { get; set; }
    }
}
