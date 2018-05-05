using HWL.Entity;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class DeleteGroupUser : GMSF.ServiceHandler<DeleteGroupUserRequestBody, DeleteGroupUserResponseBody>
    {
        //退出群组

        public DeleteGroupUser(DeleteGroupUserRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0) throw new ArgumentNullException("UserId");
            if (string.IsNullOrEmpty(this.request.GroupGuid)) throw new ArgumentNullException("GroupGuid");
        }

        public override DeleteGroupUserResponseBody ExecuteCore()
        {
            DeleteGroupUserResponseBody res = new DeleteGroupUserResponseBody() { Status = ResultStatus.Failed };
            using (HWLEntities db = new HWLEntities())
            {
                var group = db.t_group.Where(g => g.group_guid == this.request.GroupGuid).FirstOrDefault();
                if (group != null)
                {
                    var grouUser = db.t_group_user.Where(u => u.group_guid == this.request.GroupGuid && u.user_id == this.request.UserId).FirstOrDefault();
                    if (grouUser != null)
                    {
                        db.t_group_user.Remove(grouUser);
                        group.group_user_count = group.group_user_count - 1;
                        if (group.group_user_count < 0) group.group_user_count = 0;
                    }
                    if (group.build_user_id == this.request.UserId)
                    {
                        group.build_user_id = 0;
                    }
                    db.SaveChanges();
                }

                new Redis.GroupAction().DeleteGroupUser(this.request.GroupGuid, this.request.UserId);
                res.Status = ResultStatus.Success;

                try
                {
                    if (new Redis.GroupAction().GetGroupUserCount(this.request.GroupGuid) > 0) return res;

                    //如果redis的群组里没有人了，就直接删除组
                    var users = db.t_group_user.Where(g => g.group_guid == this.request.GroupGuid).ToList();
                    if (users != null && users.Count > 0)
                    {
                        if (group != null) db.t_group.Remove(group);
                        db.t_group_user.RemoveRange(users);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                }
            }
            return res;
        }
    }
}
