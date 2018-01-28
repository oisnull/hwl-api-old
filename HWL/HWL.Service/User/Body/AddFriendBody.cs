using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    public class AddFriendRequestBody
    {
        public int MyUserId { get; set; }
        /// <summary>
        /// 我的备注
        /// </summary>
        public string MyRemark { get; set; }
        public int FriendUserId { get; set; }
    }

    public class AddFriendResponseBody
    {
        public ResultStatus Status { get; set; }
        public UserFriendInfo FriendInfo { get; set; }
    }
}
