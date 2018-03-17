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
    public class GetNearCircleDetail : GMSF.ServiceHandler<GetNearCircleDetailRequestBody, GetNearCircleDetailResponseBody>
    {
        private HWLEntities db = null;
        public GetNearCircleDetail(GetNearCircleDetailRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
            if (this.request.NearCircleId <= 0)
            {
                throw new ArgumentNullException("NearCircleId");
            }
        }

        public override GetNearCircleDetailResponseBody ExecuteCore()
        {
            GetNearCircleDetailResponseBody res = new GetNearCircleDetailResponseBody();
            db = new HWLEntities();
            var model = db.t_near_circle.Where(c => c.id == this.request.NearCircleId).FirstOrDefault();
            if (model == null)
            {
                throw new Exception("信息不存在");
            }

            res.NearCircleInfo = new NearCircleInfo()
            {
                NearCircleId = model.id,
                CommentCount = model.comment_count,
                Content = model.content_info,
                ContentType = model.content_type,
                FromPosDesc = model.pos_id + "",
                //Images = null,
                LikeCount = model.like_count,
                LinkImage = model.link_image,
                LinkTitle = model.link_title,
                LinkUrl = model.link_url,
                PublishTime = model.publish_time.ToString("yyyy-MM-dd HH:mm:ss"),
                PublishUserId = model.user_id,
                CommentInfos = NearUtility.GetNearComments(model.id),
                LikeInfos = NearUtility.GetNearLikes(model.id)
            };

            BindInfo(res.NearCircleInfo);

            return res;
        }

        private void BindInfo(NearCircleInfo info)
        {
            var imageList = db.t_near_circle_image.Where(i => i.near_circle_id == info.NearCircleId).Select(i => new { i.near_circle_id, i.image_url }).ToList();
            var likeList = db.t_near_circle_like.Where(l => l.like_user_id == this.request.UserId && l.near_circle_id == info.NearCircleId && l.is_delete == false).ToList();
            var userList = db.t_user.Where(i => i.id == info.PublishUserId).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();

            if (imageList != null && imageList.Count > 0)
            {
                info.Images = imageList.Where(i => i.near_circle_id == info.NearCircleId).Select(i => i.image_url).ToList();
            }
            if (userList != null && userList.Count > 0)
            {
                var user = userList.Where(u => u.id == info.PublishUserId).FirstOrDefault();
                info.PublishUserName = UserUtility.GetShowName(user.name, user.symbol);
                info.PublishUserImage = user.head_image;
            }

            if (likeList != null && likeList.Count > 0)
            {
                info.IsLiked = likeList.Where(l => l.near_circle_id == info.NearCircleId && l.like_user_id == this.request.UserId).Select(l => l.id).FirstOrDefault() > 0 ? true : false;
            }
        }
    }
}
