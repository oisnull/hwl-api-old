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
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string CircleUpdateTime { get; set; }
    }
    public class DeleteCommentInfoResponseBody
    {
        public ResultStatus Status { get; set; }
        public string CircleLastUpdateTime { get; set; }
    }
}
