using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class GroupUsersRequestBody
    {
        public GroupType GroupType { get; set; }
        public string GroupGuid { get; set; }
    }

    public class GroupUsersResponseBody
    {
        public List<GroupUserInfo> GroupUserInfos { get; set; }
    }
}
