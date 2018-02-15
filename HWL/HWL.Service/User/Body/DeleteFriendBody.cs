using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    public class DeleteFriendRequestBody
    {
        public int MyUserId { get; set; }
        public int FriendUserId { get; set; }
    }

    public class DeleteFriendResponseBody
    {
        public ResultStatus Status { get; set; } 
    }
}
