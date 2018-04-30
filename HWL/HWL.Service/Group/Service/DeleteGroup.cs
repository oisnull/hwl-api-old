using HWL.Entity;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class DeleteGroup : GMSF.ServiceHandler<DeleteGroupRequestBody, DeleteGroupResponseBody>
    {
        //解散群组

        public DeleteGroup(DeleteGroupRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.BuildUserId <= 0) throw new ArgumentNullException("BuildUserId");
            if (string.IsNullOrEmpty(this.request.GroupGuid)) throw new ArgumentNullException("GroupGuid");
        }

        public override DeleteGroupResponseBody ExecuteCore()
        {
            DeleteGroupResponseBody res = new DeleteGroupResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                var group = db.t_group.Where(g => g.group_guid == this.request.GroupGuid).FirstOrDefault();
                if (group == null)
                {
                    res.Status = ResultStatus.Success;
                    return res;
                }
                else
                {
                    if (group.build_user_id != this.request.BuildUserId)
                    {
                        throw new Exception("你没有权限解散群组,你可以退出");
                    }
                }

                db.t_group.Remove(group);

                var users = db.t_group_user.Where(g => g.group_guid == this.request.GroupGuid).ToList();
                if (users != null && users.Count > 0)
                {
                    db.t_group_user.RemoveRange(users);
                }

                db.SaveChanges();

                new Redis.GroupAction().DeleteGroup(group.group_guid, users.ConvertAll(u => u.user_id));

                res.Status = ResultStatus.Success;
            }

            return res;
        }
    }
}
