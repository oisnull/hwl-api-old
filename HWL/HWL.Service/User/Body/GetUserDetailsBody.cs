using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    public class GetUserDetailsRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取详情的用户id
        /// </summary>
        public int GetUserId { get; set; }
    }
    public class GetUserDetailsResponseBody
    {
        public UserDetailsInfo UserDetailsInfo { get; set; }
    }
}
