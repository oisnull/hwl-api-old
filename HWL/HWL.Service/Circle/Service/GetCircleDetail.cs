using HWL.Entity;
using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class GetCircleDetail : GMSF.ServiceHandler<GetCircleDetailRequestBody, GetCircleDetailResponseBody>
    {
        public GetCircleDetail(GetCircleDetailRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
            if (this.request.CircleId <= 0)
            {
                throw new Exception("圈子信息参数错误");
            }
        }

        public override GetCircleDetailResponseBody ExecuteCore()
        {
            GetCircleDetailResponseBody res = new GetCircleDetailResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                //    CircleInfo model = db.t_circle.Where(c => c.id == this.request.CircleId).Select(q => new CircleInfo
                //    {
                //        Id = q.id,
                //        UserId = q.user_id,
                //        ContentType = q.content_type,
                //        CircleContent = q.circle_content,
                //        CommentCount = q.comment_count,
                //        ImageCount = q.image_count,
                //        Lat = q.lat,
                //        LikeCount = q.like_count,
                //        LinkImage = q.link_image,
                //        LinkTitle = q.link_title,
                //        LinkUrl = q.link_url,
                //        Lng = q.lng,
                //        PosId = q.pos_id,
                //        PublishTime = q.publish_time,

                //        //CommentInfos = null,
                //        //Images = null,
                //        //LikeUserInfos = null,
                //        //PostUserInfo = null,

                //    }).FirstOrDefault();

                //    if (model == null) return res;

                //    res.CircleInfo = model;

                //    //获取发布的图片
                //    var images = db.t_circle_image.Where(i => i.circle_id == model.Id).Select(i => new { CircleId = i.circle_id, ImageUrl = i.image_url }).ToList();

                //    //获取点赞
                //    var likes = db.t_circle_like.Where(l => l.circle_id == model.Id).Select(l => new { CircleId = l.circle_id, LikeUserId = l.like_user_id }).ToList();

                //    //获取评论
                //    var comments = db.t_circle_comment.Where(c => c.circle_id == model.Id).Select(c => new
                //    {
                //        CircleId = c.circle_id,
                //        CommentUserId = c.com_user_id,
                //        ReplyUserId = c.reply_user_id,
                //        ComContent = c.com_content,
                //        CommentTime = c.comment_time
                //    }).ToList();

                //    #region 获取用户id列表
                //    List<int> userIds = new List<int>();
                //    userIds.Add(model.UserId);

                //    if (likes != null && likes.Count > 0)
                //    {
                //        List<int> likeUserIds = likes.Select(l => l.LikeUserId).Distinct().ToList();
                //        if (likeUserIds != null && likeUserIds.Count > 0) userIds.AddRange(likeUserIds);
                //    }

                //    if (comments != null && comments.Count > 0)
                //    {
                //        List<int> commentUserIds = comments.Select(c => c.CommentUserId).Distinct().ToList();
                //        if (commentUserIds != null && commentUserIds.Count > 0) userIds.AddRange(commentUserIds);

                //        List<int> replyUserIds = comments.Select(c => c.ReplyUserId).Distinct().ToList();
                //        if (replyUserIds != null && replyUserIds.Count > 0) userIds.AddRange(replyUserIds);
                //    }

                //    userIds = userIds.Distinct().ToList();//去重复

                //    #endregion

                //    //获取用户信息
                //    List<UserShowInfo> users = null;
                //    if (userIds != null && userIds.Count > 0)
                //    {
                //        users = db.t_user.Where(u => userIds.Contains(u.id)).Select(u => new UserShowInfo
                //        {
                //            UserId = u.id,
                //            HeadImage = u.head_image,
                //            ShowName = u.nick_name,
                //        }).ToList();
                //    }

                //    //绑定到列表
                //    model.PublishTimeDesc = model.PublishTime.ToString("yyyy年MM月dd日 HH:mm");

                //    model.PostUserInfo = users != null && users.Count > 0 ? users.Where(u => u.UserId == model.UserId).FirstOrDefault() : null;

                //    if (model.ImageCount > 0 && images != null && images.Count > 0)
                //    {
                //        model.Images = images.Where(i => i.CircleId == model.Id).Select(i => i.ImageUrl).ToList();
                //    }
                //    if (model.LikeCount > 0 && likes != null && likes.Count > 0 && users != null && users.Count > 0)
                //    {
                //        var likeUserIds = likes.Where(l => l.CircleId == model.Id).Select(l => l.LikeUserId).ToList();
                //        if (likeUserIds != null && likeUserIds.Count > 0)
                //        {
                //            model.LikeUserInfos = users.Where(u => likeUserIds.Contains(u.UserId)).ToList();
                //            model.IsLike = likeUserIds.Where(u => u == this.request.UserId).FirstOrDefault() > 0 ? true : false;
                //        }
                //    }
                //    if (model.CommentCount > 0 && comments != null && comments.Count > 0)
                //    {
                //        model.CommentInfos = comments.Where(c => c.CircleId == model.Id).Select(c =>
                //        {
                //            CommentInfo cinfo = new CommentInfo()
                //            {
                //                CircleId = c.CircleId,
                //                Content = c.ComContent,
                //                CommentTimeDesc = c.CommentTime.ToString("yyyy年MM月dd日 HH:mm"),
                //                //CommentUserInfo = null,
                //                //ReplyUserInfo = null,
                //            };
                //            if (users != null && users.Count > 0)
                //            {
                //                cinfo.CommentUserInfo = users.Where(u => u.UserId == c.CommentUserId).FirstOrDefault();
                //                cinfo.ReplyUserInfo = users.Where(u => u.UserId == c.ReplyUserId).FirstOrDefault();
                //            }
                //            return cinfo;
                //        }).ToList();
                //    }
            }

            return res;
        }
    }
}
