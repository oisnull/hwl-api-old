using HWL.Entity;
using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class DeleteCircleInfo : GMSF.ServiceHandler<DeleteCircleInfoRequestBody, DeleteCircleInfoResponseBody>
    {
        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.CircleId <= 0)
            {
                throw new Exception("圈子信息参数错误");
            }
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
        }

        public DeleteCircleInfo(DeleteCircleInfoRequestBody request) : base(request)
        {
        }

        public override DeleteCircleInfoResponseBody ExecuteCore()
        {
            DeleteCircleInfoResponseBody res = new DeleteCircleInfoResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                //t_circle model = db.t_circle.Where(l => l.id == this.request.CircleId).FirstOrDefault();
                //if (model != null)
                //{
                //    db.t_circle.Remove(model);
                //    db.SaveChanges();
                //    res.Status = ResultStatus.Success;
                //}
            }

            return res;
        }
    }
}
