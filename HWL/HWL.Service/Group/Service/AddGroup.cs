using HWL.Entity;
using HWL.Service.Generic;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class AddGroup : GMSF.ServiceHandler<AddGroupRequestBody, AddGroupResponseBody>
    {
        public AddGroup(AddGroupRequestBody request) : base(request)
        {
        }

        public override AddGroupResponseBody ExecuteCore()
        {
            AddGroupResponseBody res = new AddGroupResponseBody() { Status = ResultStatus.Failed };

            string groupGuid = Guid.NewGuid().ToString();
            using (HWLEntities db = new HWLEntities())
            {
                t_group group = new t_group()
                {
                    build_user_id = this.request.BuildUserId,
                    group_guid = groupGuid,
                    group_name = this.request.GroupName,
                    group_user_count = this.request.GroupUserIds.Count,
                    build_date = DateTime.Now,
                    update_date = DateTime.Now,
                    group_note = ""
                };
                db.t_group.Add(group);
                int groupId = db.SaveChanges();
                if (groupId <= 0) throw new Exception("组创建失败");

                List<t_group_user> users = this.request.GroupUserIds.ConvertAll(f => new t_group_user
                {
                    group_guid = group.group_guid,
                    user_id = f,
                    add_date = DateTime.Now
                });
                db.t_group_user.AddRange(users);
                db.SaveChanges();

                res.Status = ResultStatus.Success;
                res.GroupGuid = groupGuid;
                res.BuildTime = GenericUtility.formatDate2(group.build_date);

                //不能在这里发送创建成功的消息，因为创建群组是客户端的行为，应由客户端来决定MQ消息的发送规则
                //往redis中添加数据
                new Redis.GroupAction().SaveGroupUser(groupGuid, this.request.GroupUserIds.ToArray());
            }

            return res;
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.BuildUserId <= 0)
            {
                throw new ArgumentNullException("BuildUserId");
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

            if (!this.request.GroupUserIds.Contains(this.request.BuildUserId))
            {
                this.request.GroupUserIds.Add(this.request.BuildUserId);
            }

            if (string.IsNullOrEmpty(this.request.GroupName))
            {
                throw new ArgumentNullException("GroupName");
            }
        }

    }
}
