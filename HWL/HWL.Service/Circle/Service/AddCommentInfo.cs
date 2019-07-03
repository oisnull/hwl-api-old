using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using HWL.Service.Generic;
using HWL.Service.User;
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

            using (HWLEntities db = new HWLEntities())
            {
                var circleModel = db.t_circle.Where(c => c.id == this.request.CircleId).FirstOrDefault();
                if (circleModel == null)
                {
                    throw new Exception("你评论的信息已经被用户删除");
                }

                bool isChanged = string.IsNullOrEmpty(this.request.CircleUpdateTime) || this.request.CircleUpdateTime != GenericUtility.FormatDate2(circleModel.update_time);

                t_circle_comment model = new t_circle_comment()
                {
                    com_user_id = this.request.CommentUserId,
                    com_content = this.request.Content,
                    circle_id = this.request.CircleId,
                    reply_user_id = this.request.ReplyUserId,
                    id = 0,
                    comment_time = DateTime.Now,
                };
                db.t_circle_comment.Add(model);
                circleModel.comment_count = circleModel.comment_count + 1;
                circleModel.update_time = DateTime.Now;
                db.SaveChanges();

                var userList = db.t_user.Where(i => i.id == model.com_user_id || i.id == model.reply_user_id).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();
                CircleCommentInfo info = new CircleCommentInfo()
                {
                    CommentId = model.id,
                    Content = model.com_content,
                    CommentTime = model.comment_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    CommentUserId = model.com_user_id,
                    //CommentUserImage = model.com,
                    //CommentUserName = model.content_info,
                    CircleId = model.circle_id,
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

                res.CommentInfo = info;
                if (!isChanged)
                    res.CircleLastUpdateTime = GenericUtility.FormatDate2(circleModel.update_time);
            }


            return res;
        }
    }
}
