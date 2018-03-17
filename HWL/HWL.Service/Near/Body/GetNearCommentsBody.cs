using HWL.Entity.Extends;
using System;
using System.Collections.Generic;

namespace HWL.Service.Near.Body
{
    public class GetNearCommentsRequestBody
    {
        public int NearCircleId { get; set; }
        public int Count { get; set; }
        public int LastCommentId { get; set; }
    }

    public class GetNearCommentsResponseBody
    {
        public List<NearCircleCommentInfo> NearCircleCommentInfos { get; set; }
    }
}
