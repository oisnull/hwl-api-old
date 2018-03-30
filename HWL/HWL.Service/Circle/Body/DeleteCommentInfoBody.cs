using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class DeleteCommentInfoRequestBody
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
    public class DeleteCommentInfoResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
