using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class AddCommentInfoRequestBody
    {
        /// <summary>
        /// 评论文章的用户id
        /// </summary>
        public int CommentUserId { get; set; }
        /// <summary>
        /// 回复评论的用户id
        /// </summary>
        public int ReplyUserId { get; set; }
        public int CircleId { get; set; }
        public string Content { get; set; }
        public string CircleUpdateTime { get; set; }
    }
    public class AddCommentInfoResponseBody
    {
        public CircleCommentInfo CommentInfo { get; set; }
        public string CircleLastUpdateTime { get; set; }
    }
}
