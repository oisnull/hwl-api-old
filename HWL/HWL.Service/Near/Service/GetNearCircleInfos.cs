using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Redis;
using HWL.Service.Generic;
using HWL.Service.Near.Body;
using HWL.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class GetNearCircleInfos : GMSF.ServiceHandler<GetNearCircleInfosRequestBody, GetNearCircleInfosResponseBody>
    {
        private HWLEntities db = null;

        public GetNearCircleInfos(GetNearCircleInfosRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户id不能为空");
            }

            if (this.request.Count <= 0)
                this.request.Count = 10;

            //if (this.request.PageIndex <= 0)
            //    this.request.PageIndex = 1;
        }

        public override GetNearCircleInfosResponseBody ExecuteCore()
        {
            GetNearCircleInfosResponseBody res = new GetNearCircleInfosResponseBody();
            if (this.request.Lat < 0 && this.request.Lon < 0)
            {
                return res;
            }

            List<int> geoIdList = new NearCircleAction().GetNearCircleIds(this.request.Lon, this.request.Lat);
            if (geoIdList == null || geoIdList.Count <= 0) return res;

            List<int> ids = null;
            if (this.request.MinNearCircleId > 0)
            {
                ids = geoIdList.Where(g => g < this.request.MinNearCircleId).Take(this.request.Count).ToList();
            }
            else
            {
                ids = geoIdList.Take(this.request.Count).ToList();
                //ids = geoIdList.Skip((this.request.PageIndex - 1) * this.request.Count).Take(this.request.Count).ToList();
            }
            //if (this.request.MaxNearCircleId > 0)
            //{
            //    ids = geoIdList.Where(g => g > this.request.MaxNearCircleId).Take(this.request.Count).ToList();
            //}
            //else if (this.request.MinNearCircleId > 0)
            //{
            //    ids = geoIdList.Where(g => g < this.request.MinNearCircleId).Take(this.request.Count).ToList();
            //}
            //else
            //{
            //    ids = geoIdList.Take(this.request.Count).ToList();
            //}
            if (ids == null || ids.Count <= 0) return res;

            db = new HWLEntities();

            var list = db.t_near_circle.Where(c => ids.Contains(c.id)).OrderByDescending(c => c.id).ToList();
            if (list == null || list.Count <= 0) return res;

            if (this.request.NearCircleMatchInfos != null && this.request.NearCircleMatchInfos.Count > 0)
            {
                list.RemoveAll(r => this.request.NearCircleMatchInfos.Exists(c => c.NearCircleId == r.id && c.UpdateTime == GenericUtility.FormatDate2(r.update_time)));
            }

            res.NearCircleInfos = list.ConvertAll(c => new NearCircleInfo
            {
                NearCircleId = c.id,
                CommentCount = c.comment_count,
                Content = c.content_info,
                ContentType = c.content_type,
                PosDesc = c.pos_desc,
                //Images = null,
                LikeCount = c.like_count,
                LinkImage = c.link_image,
                LinkTitle = c.link_title,
                LinkUrl = c.link_url,
                PublishTime = GenericUtility.FormatDate(c.publish_time),
                UpdateTime = GenericUtility.FormatDate2(c.update_time),
                PublishUserId = c.user_id,
            });

            //if (this.request.NearCircleMatchInfos != null && this.request.NearCircleMatchInfos.Count > 0)
            //{
            //    int removeCount = res.NearCircleInfos.RemoveAll(r => this.request.NearCircleMatchInfos.Exists(c => c.NearCircleId == r.NearCircleId && c.UpdateTime == r.UpdateTime));
            //}
            //if (res.NearCircleInfos == null || res.NearCircleInfos.Count <= 0) return res;

            BindInfo(res.NearCircleInfos);

            return res;
        }

        private void BindInfo(List<NearCircleInfo> infos)
        {
            if (infos == null || infos.Count <= 0) return;

            List<int> imageCircleIds = infos.Where(n => CustomerEnumDesc.ImageContentTypes().Contains(n.ContentType)).Select(n => n.NearCircleId).ToList();
            List<t_near_circle_image> imageList = null;
            if (imageCircleIds != null && imageCircleIds.Count > 0)
            {
                imageList = db.t_near_circle_image.Where(i => imageCircleIds.Contains(i.near_circle_id)).ToList();
            }
            List<int> circleIds = infos.Select(n => n.NearCircleId).ToList();
            var likeList = db.t_near_circle_like.Where(l => circleIds.Contains(l.near_circle_id) && l.is_delete == false).ToList();
            var commentList = db.t_near_circle_comment.Where(c => circleIds.Contains(c.near_circle_id)).ToList();

            List<int> userIds = infos.Select(u => u.PublishUserId).ToList();
            if (likeList != null && likeList.Count > 0)
            {
                userIds.AddRange(likeList.Select(u => u.like_user_id).ToList());
            }
            if (commentList != null && commentList.Count > 0)
            {
                userIds.AddRange(commentList.Select(u => u.comment_user_id).ToList());
                userIds.AddRange(commentList.Select(c => c.reply_user_id).ToList());
            }
            var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.head_image }).ToList();
            var friendList = db.t_user_friend.Where(f => f.user_id == this.request.UserId && userIds.Contains(f.friend_user_id)).Select(f => new { f.friend_user_id, f.friend_user_remark }).ToList();

            foreach (var item in infos)
            {
                if (imageList != null && imageList.Count > 0)
                {
                    item.Images = imageList.Where(i => i.near_circle_id == item.NearCircleId).Select(i => new ImageInfo()
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
                    item.IsLiked = likeList.Where(l => l.near_circle_id == item.NearCircleId && l.like_user_id == this.request.UserId).Select(l => l.id).FirstOrDefault() > 0 ? true : false;
                    item.LikeInfos = likeList.Where(l => l.near_circle_id == item.NearCircleId)
                        .Select(l =>
                        {
                            NearCircleLikeInfo model = new NearCircleLikeInfo()
                            {
                                LikeId = l.id,
                                LikeUserId = l.like_user_id,
                                NearCircleId = l.near_circle_id,
                                LikeTime = l.like_time.ToString("yyyy-MM-dd HH:mm:ss"),
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
                    item.CommentInfos = commentList.Where(c => c.near_circle_id == item.NearCircleId)
                        .Select(c =>
                        {
                            NearCircleCommentInfo model = new NearCircleCommentInfo()
                            {
                                CommentId = c.id,
                                Content = c.content_info,
                                NearCircleId = c.near_circle_id,
                                CommentTime = c.comment_time.ToString("yyyy-MM-dd HH:mm:ss"),
                                CommentUserId = c.comment_user_id,
                                CommentUserImage = null,
                                CommentUserName = null,
                                ReplyUserId = c.reply_user_id,
                                ReplyUserName = null,
                                ReplyUserImage = null,
                            };
                            if (userList != null && userList.Count > 0)
                            {
                                if (c.comment_user_id > 0)
                                {
                                    var comUser = userList.Where(u => u.id == c.comment_user_id).FirstOrDefault();
                                    if (comUser != null)
                                    {
                                        string friendRemark = friendList != null ? friendList.Where(f => f.friend_user_id == c.comment_user_id).Select(f => f.friend_user_remark).FirstOrDefault() : null;
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

                //item.LikeInfos = NearUtility.GetNearLikes(item.NearCircleId);
                //item.CommentInfos = NearUtility.GetNearComments(item.NearCircleId, 20);
            }
        }
    }
}
