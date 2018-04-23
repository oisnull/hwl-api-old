using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class GetCircleInfos : GMSF.ServiceHandler<GetCircleInfosRequestBody, GetCircleInfosResponseBody>
    {
        private HWLEntities db = null;

        public GetCircleInfos(GetCircleInfosRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }

            if (this.request.PageIndex <= 0)
                this.request.PageIndex = 1;

            if (this.request.Count <= 0)
                this.request.Count = 15;
        }

        public override GetCircleInfosResponseBody ExecuteCore()
        {
            GetCircleInfosResponseBody res = new GetCircleInfosResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                List<int> userIds = new List<int>() { this.request.UserId };

                //获取好友列表
                var friends = db.t_user_friend.Where(f => f.user_id == this.request.UserId).ToList();
                if (friends != null)
                {
                    userIds.AddRange(friends.Select(f => f.friend_user_id).ToList());
                }

                IQueryable<t_circle> query = db.t_circle.Where(r => userIds.Contains(r.user_id)).OrderByDescending(r => r.id);

                if (this.request.MinCircleId > 0)
                {
                    query = query.Where(q => q.id < this.request.MinCircleId).Take(this.request.Count);
                }
                else
                {
                    query = query.Skip(this.request.Count * (this.request.PageIndex - 1)).Take(this.request.Count);
                }
                var list = query.OrderByDescending(c => c.id).ToList();
                if (list == null || list.Count <= 0) return res;

                var circleList = list.ConvertAll(q => new CircleInfo
                {
                    CircleId = q.id,
                    UserId = q.user_id,
                    ContentType = q.content_type,
                    CircleContent = q.circle_content,
                    CommentCount = q.comment_count,
                    ImageCount = q.image_count,
                    Lat = q.lat,
                    LikeCount = q.like_count,
                    LinkImage = q.link_image,
                    LinkTitle = q.link_title,
                    LinkUrl = q.link_url,
                    Lon = q.lon,
                    PosId = q.pos_id,
                    PublishTime = q.publish_time.ToString("yyyy-MM-dd HH:mm:ss"),

                    //IsLike = false,
                    //CommentInfos = null,
                    //Images = null,
                    //LikeUserInfos = null,
                    //PostUserInfo = null,

                });

                if (circleList == null || circleList.Count <= 0) return res;
                res.CircleInfos = circleList;

                //    List<int> circleIds = circleList.Select(c => c.Id).ToList();

                //    //获取发布的图片
                //    var images = db.t_circle_image.Where(i => circleIds.Contains(i.circle_id)).Select(i => new { CircleId = i.circle_id, ImageUrl = i.image_url }).ToList();

                //    //获取点赞
                //    var likes = db.t_circle_like.Where(l => circleIds.Contains(l.circle_id)).Select(l => new { CircleId = l.circle_id, LikeUserId = l.like_user_id }).ToList();

                //    //获取评论
                //    var comments = db.t_circle_comment.Where(c => circleIds.Contains(c.circle_id)).Select(c => new
                //    {
                //        Id = c.id,
                //        CircleId = c.circle_id,
                //        CommentUserId = c.com_user_id,
                //        ReplyUserId = c.reply_user_id,
                //        ComContent = c.com_content,
                //        CommentTime = c.comment_time
                //    }).ToList();

                //    #region 获取用户id列表
                //    List<int> userIds = circleList.Select(c => c.UserId).Distinct().ToList();
                //    if (userIds == null) userIds = new List<int>();

                //    if (likes != null && likes.Count > 0)
                //    {
                //        List<int> likeUserIds = likes.Select(l => l.LikeUserId).Distinct().ToList();
                //        if (likeUserIds != null && likeUserIds.Count > 0) userIds.AddRange(likeUserIds);
                //    }

                //    if (comments != null && comments.Count > 0)
                //    {
                //        List<int> commentUserIds = comments.Select(c => c.CommentUserId).Distinct().ToList();
                //        if (commentUserIds != null && commentUserIds.Count > 0) userIds.AddRange(commentUserIds);

                //        List<int> replyUserIds = comments.Where(c => c.ReplyUserId > 0).Select(c => c.ReplyUserId).Distinct().ToList();
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
                //    circleList.ForEach(f =>
                //    {
                //        f.PublishTimeDesc = f.PublishTime.ToString("yyyy年MM月dd日 HH:mm");

                //        f.PostUserInfo = users != null && users.Count > 0 ? users.Where(u => u.UserId == f.UserId).FirstOrDefault() : null;

                //        if (f.ImageCount > 0 && images != null && images.Count > 0)
                //        {
                //            f.Images = images.Where(i => i.CircleId == f.Id).Select(i => i.ImageUrl).ToList();
                //        }
                //        if (f.LikeCount > 0 && likes != null && likes.Count > 0 && users != null && users.Count > 0)
                //        {
                //            var likeUserIds = likes.Where(l => l.CircleId == f.Id).Select(l => l.LikeUserId).ToList();
                //            if (likeUserIds != null && likeUserIds.Count > 0)
                //            {
                //                f.LikeUserInfos = users.Where(u => likeUserIds.Contains(u.UserId)).ToList();
                //                f.IsLike = likeUserIds.Where(u => u == this.request.UserId).FirstOrDefault() > 0 ? true : false;
                //            }
                //        }
                //        if (f.CommentCount > 0 && comments != null && comments.Count > 0)
                //        {
                //            f.CommentInfos = comments.Where(c => c.CircleId == f.Id).Select(c =>
                //            {
                //                CommentInfo cinfo = new CommentInfo()
                //                {
                //                    Id = c.Id,
                //                    CircleId = c.CircleId,
                //                    Content = c.ComContent,
                //                    CommentTimeDesc = c.CommentTime.ToString("yyyy年MM月dd日 HH:mm"),
                //                    //CommentUserInfo = null,
                //                    //ReplyUserInfo = null,
                //                };
                //                if (users != null && users.Count > 0)
                //                {
                //                    cinfo.CommentUserInfo = users.Where(u => u.UserId == c.CommentUserId).FirstOrDefault();
                //                    cinfo.ReplyUserInfo = users.Where(u => u.UserId == c.ReplyUserId).FirstOrDefault();
                //                }
                //                return cinfo;
                //            }).ToList();
                //        }
                //    });
            }

            return res;
        }
    }
}
