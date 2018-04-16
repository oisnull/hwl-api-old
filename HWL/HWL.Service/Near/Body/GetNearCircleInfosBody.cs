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
        //public int PageIndex { get; set; }
        /// <summary>
        /// 如果有值，则获取比这个值小的数据列表
        /// </summary>
        public int MinNearCircleId { get; set; }
        ///// <summary>
        ///// 如果有值，则获取比这个值大的数据列表，如果都有值或者都没有值，则获取最新的数据
        ///// </summary>
        //public int MaxNearCircleId { get; set; }
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
