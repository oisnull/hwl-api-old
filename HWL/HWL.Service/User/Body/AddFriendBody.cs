using HWL.Entity;
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
        /// 别人给我的备注
        /// </summary>
        public string MyRemark { get; set; }
        public int FriendUserId { get; set; }
        /// <summary>
        /// 我给别人的备注
        /// </summary>
        public string FriendUserRemark { get; set; }
    }

    public class AddFriendResponseBody
    {
        public ResultStatus Status { get; set; }
        /// <summary>
        /// show name的首字母
        /// </summary>
        public string FirstSpell { get; set; }
    }
}
