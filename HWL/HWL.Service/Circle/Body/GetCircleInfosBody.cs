using HWL.Entity;
using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.Circle.Body
{
    public class GetCircleInfosRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }

        public CircleType CircleType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public class GetCircleInfosResponseBody
    {
        public List<CircleInfo> CircleInfos { get; set; }
    }
}
