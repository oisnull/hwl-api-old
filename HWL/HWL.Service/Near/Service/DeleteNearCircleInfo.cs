using HWL.Entity;
using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class DeleteNearCircleInfo : GMSF.ServiceHandler<DeleteNearCircleInfoRequestBody, DeleteNearCircleInfoResponseBody>
    {
        public DeleteNearCircleInfo(DeleteNearCircleInfoRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.NearCircleId <= 0)
            {
                throw new Exception("信息参数错误");
            }
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
        }

        public override DeleteNearCircleInfoResponseBody ExecuteCore()
        {
            DeleteNearCircleInfoResponseBody res = new DeleteNearCircleInfoResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                t_near_circle model = db.t_near_circle.Where(l => l.id == this.request.NearCircleId && l.user_id == this.request.UserId).FirstOrDefault();
                if (model == null) throw new Exception("要删除的数据不存在");

                bool succ = new Redis.NearCircleAction().DeleteNearCircleId(this.request.NearCircleId);
                if (!succ) throw new Exception("删除失败");

                db.t_near_circle.Remove(model);
                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }

            return res;
        }
    }
}
