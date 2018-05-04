using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class SetGroupNameRequestBody
    {
        public int UserId { get; set; }
        public string GroupGuid { get; set; }
        public string GroupName { get; set; }
    }

    public class SetGroupNameResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
