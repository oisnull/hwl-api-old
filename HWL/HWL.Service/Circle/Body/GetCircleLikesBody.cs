using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Body
{
    public class GetCircleLikesRequestBody
    {
        public int UserId { get; set; }
        public int CircleId { get; set; }
    }
    public class GetCircleLikesResponseBody
    {
        public List<CircleLikeInfo> CircleLikeInfos { get; set; }
    }
}
