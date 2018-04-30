using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class AddGroupUsersRequestBody
    {
        public string GroupGuid { get; set; }
        public List<int> GroupUserIds { get; set; }
    }

    public class AddGroupUsersResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
