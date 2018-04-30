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
                var user = db.t_group_user.Where(u => u.group_guid == this.request.GroupGuid && u.user_id == this.request.UserId).FirstOrDefault();
                if (user == null)
                {
                    new Redis.GroupAction().DeleteGroupUser(this.request.GroupGuid, this.request.UserId);
                    res.Status = ResultStatus.Success;
                    return res;
                }

                db.t_group_user.Remove(user);
                db.SaveChanges();
                new Redis.GroupAction().DeleteGroupUser(this.request.GroupGuid, this.request.UserId);
                res.Status = ResultStatus.Success;
            }
            return res;
        }
    }
}
