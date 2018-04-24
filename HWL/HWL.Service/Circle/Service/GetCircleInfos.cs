using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using HWL.Service.User;
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

            using (db = new HWLEntities())
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
                    PublishUserId = q.user_id,
                    PublishTime = q.publish_time.ToString("yyyy-MM-dd HH:mm:ss"),

                    //IsLike = false,
                    //CommentInfos = null,
                    //Images = null,
                    //LikeUserInfos = null,
                    //PostUserInfo = null,

                });

                if (circleList == null || circleList.Count <= 0) return res;
                res.CircleInfos = circleList;

                BindInfo(res.CircleInfos);
            }

            return res;
        }

        private void BindInfo(List<CircleInfo> infos)
        {
            List<int> imageCircleIds = infos.Where(n => CustomerEnumDesc.ImageContentTypes().Contains(n.ContentType)).Select(n => n.CircleId).ToList();
            List<t_circle_image> imageList = null;
            if (imageCircleIds != null && imageCircleIds.Count > 0)
            {
                imageList = db.t_circle_image.Where(i => imageCircleIds.Contains(i.circle_id)).ToList();
            }
            List<int> circleIds = infos.Select(n => n.CircleId).ToList();
            var likeList = db.t_circle_like.Where(l => circleIds.Contains(l.circle_id) && l.is_delete == false).ToList();
            var commentList = db.t_circle_comment.Where(c => circleIds.Contains(c.circle_id)).ToList();

            List<int> userIds = infos.Select(u => u.PublishUserId).ToList();
            if (likeList != null && likeList.Count > 0)
            {
                userIds.AddRange(likeList.Select(u => u.like_user_id).ToList());
            }
            if (commentList != null && commentList.Count > 0)
            {
                userIds.AddRange(commentList.Select(u => u.com_user_id).ToList());
                userIds.AddRange(commentList.Select(c => c.reply_user_id).ToList());
            }
            var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();

            foreach (var item in infos)
            {
                if (imageList != null && imageList.Count > 0)
                {
                    item.Images = imageList.Where(i => i.circle_id == item.CircleId).Select(i => new ImageInfo()
                    {
                        Url = i.image_url,
                        Height = i.height,
                        Width = i.width
                    }).ToList();
                }

                if (userList != null && userList.Count > 0)
                {
                    var user = userList.Where(u => u.id == item.PublishUserId).FirstOrDefault();
                    if (user != null)
                    {
                        item.PublishUserName = UserUtility.GetShowName(user.name, user.symbol);
                        item.PublishUserImage = user.head_image;
                    }
                }

                if (likeList != null && likeList.Count > 0)
                {
                    item.IsLiked = likeList.Where(l => l.circle_id == item.CircleId && l.like_user_id == this.request.UserId).Select(l => l.id).FirstOrDefault() > 0 ? true : false;
                    item.LikeInfos = likeList.Where(l => l.circle_id == item.CircleId)
                        .Select(l =>
                        {
                            CircleLikeInfo model = new CircleLikeInfo()
                            {
                                LikeId = l.id,
                                LikeUserId = l.like_user_id,
                                CircleId = l.circle_id,
                                LikeTime = l.like_time.ToString("yyyy-MM-dd HH:mm:ss"),
                            };
                            if (userList != null && userList.Count > 0)
                            {
                                var likeUser = userList.Where(u => u.id == l.like_user_id).FirstOrDefault();
                                if (likeUser != null)
                                {
                                    model.LikeUserName = UserUtility.GetShowName(likeUser.name, likeUser.symbol);
                                    model.LikeUserImage = likeUser.head_image;
                                }
                            }
                            return model;
                        }).ToList();
                }

                if (commentList != null && commentList.Count > 0)
                {
                    item.CommentInfos = commentList.Where(c => c.circle_id == item.CircleId)
                        .Select(c =>
                        {
                            CircleCommentInfo model = new CircleCommentInfo()
                            {
                                CommentId = c.id,
                                Content = c.com_content,
                                CircleId = c.circle_id,
                                CommentTime = c.comment_time.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommentUserId = c.com_user_id,
                                CommentUserImage = null,
                                CommentUserName = null,
                                ReplyUserId = c.reply_user_id,
                                ReplyUserName = null,
                                ReplyUserImage = null,
                            };
                            if (userList != null && userList.Count > 0)
                            {
                                if (c.com_user_id > 0)
                                {
                                    var comUser = userList.Where(u => u.id == c.com_user_id).FirstOrDefault();
                                    if (comUser != null)
                                    {
                                        model.CommentUserName = UserUtility.GetShowName(comUser.name, comUser.symbol);
                                        model.CommentUserImage = comUser.head_image;
                                    }
                                }

                                if (c.reply_user_id > 0)
                                {
                                    var repUser = userList.Where(u => u.id == c.reply_user_id).FirstOrDefault();
                                    if (repUser != null)
                                    {
                                        model.ReplyUserName = UserUtility.GetShowName(repUser.name, repUser.symbol);
                                        model.ReplyUserImage = repUser.head_image;
                                    }
                                }
                            }
                            return model;
                        }).ToList();
                }

            }
        }
    }
}
