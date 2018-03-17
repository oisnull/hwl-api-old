using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Near.Body;
using HWL.Service.User;
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

                var userList = db.t_user.Where(i => i.id == model.comment_user_id || i.id == model.reply_user_id).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();
                NearCircleCommentInfo info = new NearCircleCommentInfo()
                {
                    CommentId = model.id,
                    Content = model.content_info,
                    CommentTime = model.comment_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    CommentUserId = model.comment_user_id,
                    //CommentUserImage = model.com,
                    //CommentUserName = model.content_info,
                    NearCircleId = model.near_circle_id,
                    ReplyUserId = model.reply_user_id,
                    //ReplyUserImage = model.content_info,
                    //ReplyUserName = model.content_info,
                };
                if (userList != null && userList.Count > 0)
                {
                    if (info.CommentUserId > 0)
                    {
                        var comUser = userList.Where(u => u.id == info.CommentUserId).FirstOrDefault();
                        info.CommentUserName = UserUtility.GetShowName(comUser.name, comUser.symbol);
                        info.CommentUserImage = comUser.head_image;
                    }

                    if (info.ReplyUserId > 0)
                    {
                        var repUser = userList.Where(u => u.id == info.ReplyUserId).FirstOrDefault();
                        info.ReplyUserName = UserUtility.GetShowName(repUser.name, repUser.symbol);
                        info.ReplyUserImage = repUser.head_image;
                    }
                }

                res.NearCircleCommentInfo = info;
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
