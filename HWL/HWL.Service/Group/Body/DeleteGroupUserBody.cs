using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class DeleteGroupUserRequestBody
    {
        public string GroupGuid { get; set; }
        public int UserId { get; set; }
    }

    public class DeleteGroupUserResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
