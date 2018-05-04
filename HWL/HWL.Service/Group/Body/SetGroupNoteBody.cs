using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Body
{
    public class SetGroupNoteRequestBody
    {
        public int UserId { get; set; }
        public string GroupGuid { get; set; }
        public string GroupNote { get; set; }
    }

    public class SetGroupNoteResponseBody
    {
        public ResultStatus Status { get; set; }
    }
}
