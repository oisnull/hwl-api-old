using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class GetCircleCommentsRequestBody
    {
        public int UserId { get; set; }
        public int CircleId { get; set; }
        public int Count { get; set; }
        public int LastCommentId { get; set; }
    }

    public class GetCircleCommentsResponseBody
    {
        public List<CircleCommentInfo> CircleCommentInfos { get; set; }
    }
}
