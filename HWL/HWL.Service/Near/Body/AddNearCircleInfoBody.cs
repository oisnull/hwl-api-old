using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.Near.Body
{
    public class AddNearCircleInfoRequestBody
    {
        public NearCircleInfo NearCircleInfo { get; set; }
    }

    public class AddNearCircleInfoResponseBody
    {
        public int NearCircleId { get; set; }
    }
}
