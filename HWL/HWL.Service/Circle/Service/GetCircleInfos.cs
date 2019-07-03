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
    public class GetCircleInfos : GMSF.ServiceHandler<GetCircleInfosRequestBody, GetCircleInfosResponseBody>
    {
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
                List<int> userIds = null;
                IQueryable<t_circle> query = null;
                if (this.request.ViewUserId > 0 && this.request.ViewUserId != this.request.UserId)
                {
                    //获取查看用户的信息
                    var postUser = db.t_user.Where(u => u.id == this.request.ViewUserId).FirstOrDefault();
                    if (postUser == null) throw new Exception("用户不存在");
                    res.ViewUserId = postUser.id;
                    res.ViewUserImage = postUser.head_image;
                    res.ViewUserName = postUser.name;
                    userIds = new List<int>() { this.request.ViewUserId };
                }
                else
                {
                    //获取好友id列表
                    userIds = new List<int>() { this.request.UserId };
                    var friendIds = db.t_user_friend.Where(f => f.user_id == this.request.UserId).Select(f => f.friend_user_id).ToList();
                    if (friendIds != null && friendIds.Count > 0)
                    {
                        userIds.AddRange(friendIds);
                    }
                }

                query = db.t_circle.Where(r => userIds.Contains(r.user_id)).OrderByDescending(r => r.id);
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

                if (this.request.CircleMatchInfos != null && this.request.CircleMatchInfos.Count > 0)
                {
                    list.RemoveAll(r => this.request.CircleMatchInfos.Exists(c => c.CircleId == r.id && c.UpdateTime == GenericUtility.FormatDate2(r.update_time)));
                }

                res.CircleInfos = list.ConvertAll(q => new CircleInfo
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
                    PublishTime = GenericUtility.FormatDate(q.publish_time),
                    UpdateTime = GenericUtility.FormatDate2(q.update_time),

                    //IsLike = false,
                    //CommentInfos = null,
                    //Images = null,
                    //LikeUserInfos = null,
                    //PostUserInfo = null,
                });

                BindCircleInfos(db, this.request.UserId, res.CircleInfos);
            }

            return res;
        }

        public static void BindCircleInfos(HWLEntities db, int userId, List<CircleInfo> infos)
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
            var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.head_image }).ToList();
            var friendList = db.t_user_friend.Where(f => f.user_id == userId && userIds.Contains(f.friend_user_id)).Select(f => new { f.friend_user_id, f.friend_user_remark }).ToList();

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
                        string friendRemark = friendList != null ? friendList.Where(f => f.friend_user_id == item.PublishUserId).Select(f => f.friend_user_remark).FirstOrDefault() : null;
                        item.PublishUserName = UserUtility.GetShowName(friendRemark, user.name);
                        item.PublishUserImage = user.head_image;
                    }
                }

                if (likeList != null && likeList.Count > 0)
                {
                    item.IsLiked = likeList.Where(l => l.circle_id == item.CircleId && l.like_user_id == userId).Select(l => l.id).FirstOrDefault() > 0 ? true : false;
                    item.LikeInfos = likeList.Where(l => l.circle_id == item.CircleId)
                        .Select(l =>
                        {
                            CircleLikeInfo model = new CircleLikeInfo()
                            {
                                LikeId = l.id,
                                LikeUserId = l.like_user_id,
                                CircleId = l.circle_id,
                                LikeTime = GenericUtility.FormatDate(l.like_time),
                            };
                            if (userList != null && userList.Count > 0)
                            {
                                var likeUser = userList.Where(u => u.id == l.like_user_id).FirstOrDefault();
                                if (likeUser != null)
                                {
                                    string friendRemark = friendList != null ? friendList.Where(f => f.friend_user_id == l.like_user_id).Select(f => f.friend_user_remark).FirstOrDefault() : null;
                                    model.LikeUserName = UserUtility.GetShowName(friendRemark, likeUser.name);
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
                                CommentTime = GenericUtility.FormatDate(c.comment_time),
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
                                        string friendRemark = friendList != null ? friendList.Where(f => f.friend_user_id == c.com_user_id).Select(f => f.friend_user_remark).FirstOrDefault() : null;
                                        model.CommentUserName = UserUtility.GetShowName(friendRemark, comUser.name);
                                        model.CommentUserImage = comUser.head_image;
                                    }
                                }

                                if (c.reply_user_id > 0)
                                {
                                    var repUser = userList.Where(u => u.id == c.reply_user_id).FirstOrDefault();
                                    if (repUser != null)
                                    {
                                        string friendRemark = friendList != null ? friendList.Where(f => f.friend_user_id == c.reply_user_id).Select(f => f.friend_user_remark).FirstOrDefault() : null;
                                        model.ReplyUserName = UserUtility.GetShowName(friendRemark, repUser.name);
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
