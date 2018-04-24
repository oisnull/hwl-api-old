using HWL.Entity;
using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class DeleteCommentInfo : GMSF.ServiceHandler<DeleteCommentInfoRequestBody, DeleteCommentInfoResponseBody>
    {
        public DeleteCommentInfo(DeleteCommentInfoRequestBody request) : base(request)
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

        public override DeleteCommentInfoResponseBody ExecuteCore()
        {
            DeleteCommentInfoResponseBody res = new DeleteCommentInfoResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                t_circle_comment model = db.t_circle_comment.Where(l => l.id == this.request.CommentId).FirstOrDefault();
                if (model != null)
                {
                    db.t_circle_comment.Remove(model);
                    db.SaveChanges();
                    res.Status = ResultStatus.Success;
                }
            }

            return res;
        }
    }
}
