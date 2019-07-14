using HWL.Entity;
using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.User.Body
{
    public class SetUserPosRequestBody
    {
        public int UserId { get; set; }
        public string LastGroupGuid { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Details { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class SetUserPosResponseBody
    {
        public ResultStatus Status { get; set; }

        public int UserPosId { get; set; }

        public string UserGroupGuid { get; set; }

        public List<UserSecretInfo> GroupUserInfos { get; set; }
    }
}
