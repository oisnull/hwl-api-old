using HWL.Entity;
using System;

namespace HWL.Service.Circle.Body
{
    public class SetLikeInfoRequestBody
    {
        /// <summary>
        /// 0表示取消 1表示点赞
        /// </summary>
        public int ActionType { get; set; }
        /// <summary>
        /// 点赞的用户id
        /// </summary>
        public int LikeUserId { get; set; }
        public int CircleId { get; set; }

        //public int LikeInfoId { get; set; }
        public string CircleUpdateTime { get; set; }
    }
    public class SetLikeInfoResponseBody
    {
        public ResultStatus Status { get; set; }
        public string CircleLastUpdateTime { get; set; }
    }
}
