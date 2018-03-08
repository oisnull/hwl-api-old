using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.Near.Body
{
    public class GetNearCircleDetailRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }
        public int NearCircleId { get; set; }
    }

    public class GetNearCircleDetailResponseBody
    {
        public NearCircleInfo NearCircleInfo { get; set; }
    }
}
