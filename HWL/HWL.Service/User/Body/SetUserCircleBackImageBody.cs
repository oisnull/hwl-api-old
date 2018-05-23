using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    public class SetUserCircleBackImageRequestBody
    {
        public int UserId { get; set; }
        public string CircleBackImageUrl { get; set; }
    }

    public class SetUserCircleBackImageResponseBody
    {
        public ResultStatus Status { get; set; }
        public String CircleBackImageUrl { get; set; }
    }
}
