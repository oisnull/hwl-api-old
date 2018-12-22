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
    public class GetGroups : GMSF.ServiceHandler<GetGroupsRequestBody, GetGroupsResponseBody>
    {
        public GetGroups(GetGroupsRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
        }

        public override GetGroupsResponseBody ExecuteCore()
        {
            GetGroupsResponseBody res = new GetGroupsResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                //获取用户所在组的guid
                List<string> groupGuids = db.t_group_user.Where(g => g.user_id == this.request.UserId).Select(g => g.group_guid).ToList();
                if (groupGuids == null || groupGuids.Count <= 0) return res;

                var groups = db.t_group.Where(g => groupGuids.Contains(g.group_guid)).ToList();
                if (groups == null || groups.Count <= 0) return res;

                var groupUsers = db.t_group_user.Where(u => groupGuids.Contains(u.group_guid)).ToList();

                List<int> userIds = groupUsers.Select(u => u.user_id).ToList();
                var users = db.t_user.Where(u => userIds.Contains(u.id)).Select(u => new { u.id, u.name, u.head_image }).ToList();

                List<t_group> emptyGroup = new List<t_group>();
                res.GroupInfos = new List<GroupInfo>();
                groups.ForEach(f =>
                {
                    GroupInfo info = new GroupInfo()
                    {
                        GroupGuid = f.group_guid,
                        BuildUserId = f.build_user_id,
                        GroupName = f.group_name,
                        GroupNote = f.group_note,
                        GroupUserCount = f.group_user_count,
                        GroupUsers = groupUsers.Where(u => u.group_guid == f.group_guid).Select(u =>
                        {
                            GroupUserInfo user = new GroupUserInfo()
                            {
                                UserId = u.user_id,
                                GroupGuid = u.group_guid,
                                UserHeadImage = users.Where(i => i.id == u.user_id).Select(i => i.head_image).FirstOrDefault(),
                                UserName = users.Where(i => i.id == u.user_id).Select(i => i.name).FirstOrDefault()
                            };

                            return user;
                        }).ToList(),
                        BuildDate = GenericUtility.formatDate2(f.build_date),
                        UpdateDate = GenericUtility.formatDate2(f.update_date),
                    };
                    if (info.GroupUsers == null || info.GroupUsers.Count <= 0)
                    {
                        emptyGroup.Add(f);
                    }
                    else
                    {
                        res.GroupInfos.Add(info);
                    }
                });

                if (emptyGroup.Count > 0)
                {
                    db.t_group.RemoveRange(emptyGroup);
                    //try
                    //{
                    db.SaveChanges();
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
            }

            return res;
        }
    }
}
