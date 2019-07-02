using HWL.Entity;
using HWL.Service.Generic;
using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class DeleteNearComment : GMSF.ServiceHandler<DeleteNearCommentRequestBody, DeleteNearCommentResponseBody>
    {
        public DeleteNearComment(DeleteNearCommentRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.CommentId <= 0)
            {
                throw new Exception("评论信息参数错误");
            }
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
        }

        public override DeleteNearCommentResponseBody ExecuteCore()
        {
            DeleteNearCommentResponseBody res = new DeleteNearCommentResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                t_near_circle_comment model = db.t_near_circle_comment.Where(l => l.id == this.request.CommentId).FirstOrDefault();
                if (model != null)
                {
                    db.t_near_circle_comment.Remove(model);
                    var circleModel = db.t_near_circle.Where(c => c.id == model.near_circle_id).FirstOrDefault();
                    if (circleModel != null)
                    {
                        bool isChanged = string.IsNullOrEmpty(this.request.NearCircleUpdateTime) || this.request.NearCircleUpdateTime != GenericUtility.FormatDate2(circleModel.update_time);
                        circleModel.update_time = DateTime.Now;

                        res.NearCircleLastUpdateTime = GenericUtility.FormatDate2(circleModel.update_time);
                    }
                    else
                    {
                        db.SaveChanges();
                    }
                }
            }

            res.Status = ResultStatus.Success;
            return res;
        }
    }
}
