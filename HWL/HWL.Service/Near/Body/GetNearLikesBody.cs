using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Body
{
    public class GetNearLikesRequestBody
    {
        public int UserId { get; set; }
        public int NearCircleId { get; set; }
    }

    public class GetNearLikesResponseBody
    {
        public List<NearCircleLikeInfo> NearCircleLikeInfos { get; set; }
    }
}
