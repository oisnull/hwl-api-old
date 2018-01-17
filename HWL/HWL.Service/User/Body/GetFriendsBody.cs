using HWL.Entity.Extends;
using System.Collections.Generic;

namespace HWL.Service.User.Body
{
    public class GetFriendsRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }
    }

    public class GetFriendsResponseBody
    {
        public List<UserFriendInfo> UserFriendInfos { get; set; }
    }
}
