using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.Near.Body
{
    public class AddNearCircleInfoRequestBody
    {
        public int UserId { get; set; }
        public string Content { get; set; }
        public string FromPosDesc { get; set; }
        public List<string> Images { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
        public string LinkImage { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class AddNearCircleInfoResponseBody
    {
        public int NearCircleId { get; set; }
    }
}
