using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class AddGroupRequestBody
    {
        public string GroupName { get; set; }
        public int BuildUserId { get; set; }

        public List<int> GroupUserIds { get; set; }
    }

    public class AddGroupResponseBody
    {
        public ResultStatus Status { get; set; }
        public string GroupGuid { get; set; }
        public string BuildTime { get; set; }
    }
}
