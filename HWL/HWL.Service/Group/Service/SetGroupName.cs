using HWL.Entity;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class SetGroupName : GMSF.ServiceHandler<SetGroupNameRequestBody, SetGroupNameResponseBody>
    {
        public SetGroupName(SetGroupNameRequestBody request) : base(request)
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

            if (string.IsNullOrEmpty(this.request.GroupName))
            {
                throw new Exception("群组名称不能为空");
            }
        }

        public override SetGroupNameResponseBody ExecuteCore()
        {
            SetGroupNameResponseBody res = new SetGroupNameResponseBody() { Status = ResultStatus.Failed };
            using (HWLEntities db = new HWLEntities())
            {
                var group = db.t_group.Where(g => g.group_guid == this.request.GroupGuid).FirstOrDefault();
                if (group == null) throw new Exception("群组不存在");

                group.group_name = this.request.GroupName;
                group.update_date = DateTime.Now;
                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }
            return res;
        }
    }
}
