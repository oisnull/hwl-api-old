using HWL.Entity;
using System;

namespace HWL.Service.Near.Body
{
    public class DeleteNearCircleInfoRequestBody
    {
        public int UserId { get; set; }
        public int NearCircleId { get; set; }
    }
    public class DeleteNearCircleInfoResponseBody
    {
        public ResultStatus Status{ get; set; }
    }
}
