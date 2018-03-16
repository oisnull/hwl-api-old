using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Body
{
    public class DeleteNearCommentRequestBody
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
    public class DeleteNearCommentResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
