using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class GetGroupsRequestBody
    {
        public int UserId { get; set; }
    }

    public class GetGroupsResponseBody
    {
        public List<GroupInfo> GroupInfos { get; set; }
    }
}
