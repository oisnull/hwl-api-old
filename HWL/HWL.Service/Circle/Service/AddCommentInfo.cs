using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class AddCommentInfo : GMSF.ServiceHandler<AddCommentInfoRequestBody, AddCommentInfoResponseBody>
    {
        public AddCommentInfo(AddCommentInfoRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.CircleId <= 0)
            {
                throw new Exception("圈子信息参数错误");
            }
            if (this.request.CircleUserId <= 0)
            {
                throw new Exception("圈子信息的用户参数错误");
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

        public override AddCommentInfoResponseBody ExecuteCore()
        {
            AddCommentInfoResponseBody res = new AddCommentInfoResponseBody();

            //using (HWLEntities db = new HWLEntities())
            //{
            //    var circleModel = db.t_circle.Where(c => c.id == this.request.CircleId).FirstOrDefault();
            //    if (circleModel == null)
            //    {
            //        throw new Exception("信息不存在");
            //    }

            //    t_circle_comment model = new t_circle_comment()
            //    {
            //        circle_id = this.request.CircleId,
            //        com_content = this.request.Content,
            //        reply_user_id = this.request.ReplyUserId,
            //        com_user_id = this.request.CommentUserId,
            //        circle_user_id = this.request.CircleUserId,
            //        id = 0,
            //        comment_time = DateTime.Now,
            //    };
            //    db.t_circle_comment.Add(model);
            //    db.SaveChanges();

            //    List<UserShowInfo> users = db.t_user.Where(u => u.id == model.com_user_id || u.id == model.reply_user_id).Select(u => new UserShowInfo
            //    {
            //        UserId = u.id,
            //        HeadImage = u.head_image,
            //        ShowName = u.nick_name,
            //    }).ToList();

            //    UserShowInfo comUser = null;
            //    UserShowInfo replyUser = null;
            //    if (users != null && users.Count > 0)
            //    {
            //        comUser = users.Where(u => u.UserId == model.com_user_id).FirstOrDefault();
            //        replyUser = users.Where(u => u.UserId == model.reply_user_id).FirstOrDefault();
            //    }

            //    res.CommentInfo = new CommentInfo
            //    {
            //        Id = model.id,
            //        CircleId = model.circle_id,
            //        CommentTimeDesc = model.comment_time.ToString("yyyy年MM月dd日 HH:mm"),
            //        CommentUserInfo = comUser,
            //        ReplyUserInfo = replyUser,
            //        Content = model.com_content,
            //    };

            //    circleModel.comment_count = circleModel.comment_count + 1;
            //    db.SaveChanges();
            //}

            return res;
        }
    }
}
