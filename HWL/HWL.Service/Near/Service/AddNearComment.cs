using HWL.Entity;
using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class AddNearComment : GMSF.ServiceHandler<AddNearCommentRequestBody, AddNearCommentResponseBody>
    {
        public AddNearComment(AddNearCommentRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.NearCircleId <= 0)
            {
                throw new Exception("信息参数错误");
            }
            if (this.request.CommentUserId <= 0 && this.request.ReplyUserId <= 0)
            {
                throw new Exception("评论或者回复用户参数错误");
            }
            if (string.IsNullOrEmpty(this.request.Content))
            {
                throw new Exception("评论内容不能为空");
            }
        }

        public override AddNearCommentResponseBody ExecuteCore()
        {
            AddNearCommentResponseBody res = new AddNearCommentResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var circleModel = db.t_near_circle.Where(c => c.id == this.request.NearCircleId).FirstOrDefault();
                if (circleModel == null)
                {
                    throw new Exception("信息不存在");
                }

                t_near_circle_comment model = new t_near_circle_comment()
                {
                    comment_user_id = this.request.CommentUserId,
                    content_info = this.request.Content,
                    near_circle_id = this.request.NearCircleId,
                    reply_user_id = this.request.ReplyUserId,
                    id = 0,
                    comment_time = DateTime.Now,
                };
                db.t_near_circle_comment.Add(model);
                circleModel.comment_count = circleModel.comment_count + 1;
                db.SaveChanges();

                //List<UserShowInfo> users = db.t_user.Where(u => u.id == model.com_user_id || u.id == model.reply_user_id).Select(u => new UserShowInfo
                //{
                //    UserId = u.id,
                //    HeadImage = u.head_image,
                //    ShowName = u.nick_name,
                //}).ToList();

                //UserShowInfo comUser = null;
                //UserShowInfo replyUser = null;
                //if (users != null && users.Count > 0)
                //{
                //    comUser = users.Where(u => u.UserId == model.com_user_id).FirstOrDefault();
                //    replyUser = users.Where(u => u.UserId == model.reply_user_id).FirstOrDefault();
                //}

                //res.CommentInfo = new CommentInfo
                //{
                //    Id = model.id,
                //    CircleId = model.circle_id,
                //    CommentTimeDesc = model.comment_time.ToString("yyyy年MM月dd日 HH:mm"),
                //    CommentUserInfo = comUser,
                //    ReplyUserInfo = replyUser,
                //    Content = model.com_content,
                //};
            }

            return res;
        }
    }
}
