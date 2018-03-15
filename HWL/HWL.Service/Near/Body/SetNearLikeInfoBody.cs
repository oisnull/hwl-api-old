using HWL.Entity;
using System;

namespace HWL.Service.Near.Body
{
    public class SetNearLikeInfoRequestBody
    {
        /// <summary>
        /// 0表示取消 1表示点赞
        /// </summary>
        public int ActionType { get; set; }
        /// <summary>
        /// 点赞的用户id
        /// </summary>
        public int LikeUserId { get; set; }
        public int NearCircleId { get; set; }

        //public int LikeInfoId { get; set; }
    }
    public class SetNearLikeInfoResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
