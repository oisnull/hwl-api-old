using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class GetGroupAndUsersRequestBody
    {
        public int UserId { get; set; }
        public string GroupGuid { get; set; }
    }

    public class GetGroupAndUsersResponseBody
    {
        public GroupInfo GroupInfo { get; set; }
    }
}
