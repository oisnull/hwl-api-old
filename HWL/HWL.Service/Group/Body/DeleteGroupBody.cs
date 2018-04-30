using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class DeleteGroupRequestBody
    {
        public string GroupGuid { get; set; }
        public int BuildUserId { get; set; }
    }

    public class DeleteGroupResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
