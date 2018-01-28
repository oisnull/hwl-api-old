using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    public class SearchUserRequestBody
    {

        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string UserKey { get; set; }
    }

    public class SearchUserResponseBody
    {
        public List<UserSearchInfo> UserInfos { get; set; }

    }
}
