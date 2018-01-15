using HWL.Entity;
using HWL.Entity.Extends;

namespace HWL.Service.User.Body
{
    public class SetUserPosRequestBody
    {
        public int UserId { get; set; }
        public PosDetails UserPos { get; set; }
    }

    public class SetUserPosResponseBody
    {
        public ResultStatus Status { get; set; }

        public int UserPosId { get; set; }

        public string UserGroupGuid { get; set; }
    }
}
