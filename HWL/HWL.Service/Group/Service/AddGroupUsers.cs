using HWL.Entity;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class AddGroupUsers : GMSF.ServiceHandler<AddGroupUsersRequestBody, AddGroupUsersResponseBody>
    {
        public AddGroupUsers(AddGroupUsersRequestBody request) : base(request)
        {
        }

        public override AddGroupUsersResponseBody ExecuteCore()
        {
            AddGroupUsersResponseBody res = new AddGroupUsersResponseBody() { Status = ResultStatus.Failed };

            //往redis中添加数据
            new Redis.GroupAction().SaveGroupUser(this.request.GroupGuid, this.request.GroupUserIds.ToArray());

            using (HWLEntities db = new HWLEntities())
            {
                List<t_group_user> users = this.request.GroupUserIds.ConvertAll(f => new t_group_user
                {
                    group_guid = this.request.GroupGuid,
                    user_id = f,
                    add_date = DateTime.Now
                });
                db.t_group_user.AddRange(users);
                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }
            return res;
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (string.IsNullOrEmpty(this.request.GroupGuid))
            {
                throw new ArgumentNullException("GroupGuid");
            }

            if (this.request.GroupUserIds == null)
            {
                throw new ArgumentNullException("GroupUserIds");
            }

            this.request.GroupUserIds.RemoveAll(u => u <= 0);
            if (this.request.GroupUserIds.Count <= 0)
            {
                throw new Exception("组成员不能为空");
            }
        }
    }
}
