using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    public class GetUserRelationInfoRequestBody
    {
        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int UserId { get; set; }

        public int RelationUserId { get; set; }
    }

    public class GetUserRelationInfoResponseBody
    {
        public bool isFriend { get; set; }
        public bool isInBlackList { get; set; }
    }
}
