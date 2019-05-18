using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Generic;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class GetGroupAndUsers : GMSF.ServiceHandler<GetGroupAndUsersRequestBody, GetGroupAndUsersResponseBody>
    {
        public GetGroupAndUsers(GetGroupAndUsersRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
            if (string.IsNullOrEmpty(this.request.GroupGuid))
            {
                throw new ArgumentNullException("GroupGuid");
            }
        }

        public override GetGroupAndUsersResponseBody ExecuteCore()
        {
            GetGroupAndUsersResponseBody res = new GetGroupAndUsersResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                t_group group = db.t_group.Where(g => g.group_guid == this.request.GroupGuid).FirstOrDefault();
                if (group == null)
                {
                    //如果组不存在，则删除下面所有用户信息
                    DeleteGroupUsers(db);
                    return res;
                }

                var groupUsers = db.t_group_user.Where(g => g.group_guid == this.request.GroupGuid).ToList();
                if (!groupUsers.Exists(g => g.user_id == this.request.UserId)) return res;

                List<int> userIds = groupUsers.Select(u => u.user_id).ToList();
                var users = db.t_user.Where(u => userIds.Contains(u.id)).Select(u => new { u.id, u.name, u.head_image }).ToList();

                res.GroupInfo = new GroupInfo()
                {
                    GroupGuid = group.group_guid,
                    BuildUserId = group.build_user_id,
                    GroupName = group.group_name,
                    GroupNote = group.group_note,
                    GroupUserCount = group.group_user_count,
                    GroupUsers = groupUsers.Select(u =>
                    {
                        return new GroupUserInfo()
                        {
                            UserId = u.user_id,
                            GroupGuid = u.group_guid,
                            UserHeadImage = users.Where(i => i.id == u.user_id).Select(i => i.head_image).FirstOrDefault(),
                            UserName = users.Where(i => i.id == u.user_id).Select(i => i.name).FirstOrDefault()
                        };
                    }).ToList(),
                    BuildDate = GenericUtility.FormatDate2(group.build_date),
                    UpdateDate = GenericUtility.FormatDate2(group.update_date),
                };
                res.GroupInfo.GroupUserImages = res.GroupInfo.GroupUsers.Select(u => u.UserHeadImage).Take(ConfigManager.GROUP_USER_IMAGE_COUNT).ToList();
            }

            return res;
        }

        private void DeleteGroupUsers(HWLEntities db)
        {
            var groupUsers = db.t_group_user.Where(g => g.group_guid == this.request.GroupGuid).ToList();
            if (groupUsers != null)
            {
                try
                {
                    db.t_group_user.RemoveRange(groupUsers);
                    db.SaveChanges();

                    new Redis.GroupAction().DeleteGroup(this.request.GroupGuid);
                }
                catch (Exception)
                {
                    //可以忽略这个错误
                }
            }
        }
    }
}
