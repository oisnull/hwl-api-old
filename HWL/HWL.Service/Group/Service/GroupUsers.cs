using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class GroupUsers : GMSF.ServiceHandler<GroupUsersRequestBody, GroupUsersResponseBody>
    {
        public GroupUsers(GroupUsersRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (string.IsNullOrEmpty(this.request.GroupGuid))
            {
                throw new ArgumentNullException("GroupGuid");
            }
            //if (this.request.GroupType != GroupType.Near && this.request.GroupType != GroupType.User)
            //    throw new Exception("组类型参数错误");
        }

        public override GroupUsersResponseBody ExecuteCore()
        {
            GroupUsersResponseBody res = new GroupUsersResponseBody();

            Redis.GroupAction groupAction = new Redis.GroupAction();
            List<int> userIds = groupAction.GetGroupUserIds(this.request.GroupGuid);
            //switch (this.request.GroupType)
            //{
            //    case GroupType.Near:
            //        userIds = groupAction.GetNearGroupUserIds(this.request.GroupGuid);
            //        break;
            //    case GroupType.User:
            //        userIds = groupAction.GetGroupUserIds(this.request.GroupGuid);
            //        break;
            //}
            if (userIds == null || userIds.Count <= 0) return res;

            using (HWLEntities db = new HWLEntities())
            {
                res.GroupUserInfos = db.t_user.Where(u => userIds.Contains(u.id))
                .Select(u => new GroupUserInfo()
                {
                    GroupGuid = this.request.GroupGuid,
                    UserId = u.id,
                    UserName = u.name,
                    UserHeadImage = u.head_image,
                }).ToList();
            }

            return res;
        }
    }
}
