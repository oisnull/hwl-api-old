using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Body
{
    public class AddNearCommentRequestBody
    {
        /// <summary>
        /// 附近信息id
        /// </summary>
        public int NearCircleId { get; set; }
        /// <summary>
        /// 发评论的用户id
        /// </summary>
        public int CommentUserId { get; set; }
        /// <summary>
        /// 回复用户id
        /// </summary>
        public int ReplyUserId { get; set; }
        public string Content { get; set; }
        public string NearCircleUpdateTime { get; set; }
    }

    public class AddNearCommentResponseBody
    {
        public int NearCirclePublishUserId { get; set; }
        public string NearCircleLastUpdateTime { get; set; }
        public NearCircleCommentInfo NearCircleCommentInfo { get; set; }
    }
}
