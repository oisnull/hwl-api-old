using HWL.Entity;
using HWL.Service.Group.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Group.Service
{
    public class SetGroupNote : GMSF.ServiceHandler<SetGroupNoteRequestBody, SetGroupNoteResponseBody>
    {
        public SetGroupNote(SetGroupNoteRequestBody request) : base(request)
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

        public override SetGroupNoteResponseBody ExecuteCore()
        {
            SetGroupNoteResponseBody res = new SetGroupNoteResponseBody() { Status = ResultStatus.Failed };
            using (HWLEntities db = new HWLEntities())
            {
                var group = db.t_group.Where(g => g.group_guid == this.request.GroupGuid).FirstOrDefault();
                if (group == null) throw new Exception("群组不存在");
                //if (group.build_user_id != this.request.UserId) throw new Exception("只有群主才能修改公告");

                group.group_note = this.request.GroupNote;
                group.update_date = DateTime.Now;
                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }
            return res;
        }
    }
}
