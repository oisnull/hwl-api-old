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

            //从redis中获取附近圈子信息id列表
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

            res.NearCircleInfos = list.Select(c => new NearCircleInfo
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
                PublishTime = GenericUtility.formatDate(c.publish_time),
                UpdateTime = GenericUtility.formatDate2(c.update_time),
                PublishUserId = c.user_id,
            }).ToList();

            BindInfo(res.NearCircleInfos);

            //BindLike(res.NearCircleInfos);
            //BindImages(res.NearCircleInfos, res.NearCircleInfos.Where(n => n.ContentType == CircleContentType.Image).Select(n => n.NearCircleId).ToList());
            //BindUser(res.NearCircleInfos, res.NearCircleInfos.Select(u => u.PublishUserId).Distinct().ToList());

            return res;
        }


        private void BindInfo(List<NearCircleInfo> infos)
        {
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
            var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();

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
                        item.PublishUserName = UserUtility.GetShowName(user.name, user.symbol);
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
                                    model.LikeUserName = UserUtility.GetShowName(likeUser.name, likeUser.symbol);
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

                //item.LikeInfos = NearUtility.GetNearLikes(item.NearCircleId);
                //item.CommentInfos = NearUtility.GetNearComments(item.NearCircleId, 20);
            }
        }

        //private void BindInfo(List<NearCircleInfo> infos)
        //{
        //    List<int> circleIds = infos.Where(n => CustomerEnumDesc.ImageContentTypes().Contains(n.ContentType)).Select(n => n.NearCircleId).ToList();
        //    var imageList = db.t_near_circle_image.Where(i => circleIds.Contains(i.near_circle_id)).Select(i => new { i.near_circle_id, i.image_url, i.width, i.height }).ToList();
        //    var likeList = db.t_near_circle_like.Where(l => l.like_user_id == this.request.UserId && circleIds.Contains(l.near_circle_id) && l.is_delete == false).ToList();

        //    var userIds = infos.Select(u => u.PublishUserId).Distinct().ToList();
        //    var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();

        //    foreach (var item in infos)
        //    {
        //        if (imageList != null && imageList.Count > 0)
        //        {
        //            item.Images = imageList.Where(i => i.near_circle_id == item.NearCircleId).Select(i => new ImageInfo()
        //            {
        //                Url = i.image_url,
        //                Height = i.height,
        //                Width = i.width
        //            }).ToList();
        //        }
        //        if (userList != null && userList.Count > 0)
        //        {
        //            var user = userList.Where(u => u.id == item.PublishUserId).FirstOrDefault();
        //            item.PublishUserName = UserUtility.GetShowName(user.name, user.symbol);
        //            item.PublishUserImage = user.head_image;
        //        }

        //        if (likeList != null && likeList.Count > 0)
        //        {
        //            item.IsLiked = likeList.Where(l => l.near_circle_id == item.NearCircleId && l.like_user_id == this.request.UserId).Select(l => l.id).FirstOrDefault() > 0 ? true : false;
        //        }

        //        //item.LikeInfos = NearUtility.GetNearLikes(item.NearCircleId);
        //        //item.CommentInfos = NearUtility.GetNearComments(item.NearCircleId, 20);
        //    }
        //}

        //private void BindImages(List<NearCircleInfo> infos, List<int> circleIds)
        //{
        //    if (circleIds == null || circleIds.Count <= 0) return;
        //    var imageList = db.t_near_circle_image.Where(i => circleIds.Contains(i.near_circle_id)).Select(i => new { i.near_circle_id, i.image_url }).ToList();
        //    if (imageList == null || imageList.Count <= 0) return;
        //    foreach (var item in infos)
        //    {
        //        item.Images = imageList.Where(i => i.near_circle_id == item.NearCircleId).Select(i => i.image_url).ToList();
        //    }
        //}

        //private void BindUser(List<NearCircleInfo> infos, List<int> userIds)
        //{
        //    if (userIds == null || userIds.Count <= 0) return;
        //    var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();
        //    if (userList == null || userList.Count <= 0) return;
        //    foreach (var item in infos)
        //    {
        //        var user = userList.Where(u => u.id == item.PublishUserId).FirstOrDefault();
        //        item.PublishUserName = UserUtility.GetShowName(user.name, user.symbol);
        //        item.PublishUserImage = user.head_image;
        //    }
        //}
    }
}
