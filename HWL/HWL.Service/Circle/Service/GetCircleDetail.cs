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

        HWLEntities db = null;

        public override GetCircleDetailResponseBody ExecuteCore()
        {
            GetCircleDetailResponseBody res = new GetCircleDetailResponseBody();

            db = new HWLEntities();
            var model = db.t_circle.Where(c => c.id == this.request.CircleId).FirstOrDefault();
            if (model == null)
            {
                return res;
            }
            if (this.request.UpdateTime != null && this.request.UpdateTime == GenericUtility.FormatDate2(model.update_time))
            {
                return res;
            }

            res.CircleInfo = new CircleInfo()
            {
                CircleId = model.id,
                CommentCount = model.comment_count,
                CircleContent = model.circle_content,
                ContentType = model.content_type,
                PosDesc = model.pos_desc,
                //Images = null,
                ImageCount = model.image_count,
                LikeCount = model.like_count,
                LinkImage = model.link_image,
                LinkTitle = model.link_title,
                LinkUrl = model.link_url,
                PublishTime = GenericUtility.FormatDate(model.publish_time),
                UpdateTime = GenericUtility.FormatDate2(model.update_time),
                PublishUserId = model.user_id,
                CommentInfos = CircleUtility.GetComments(this.request.UserId, model.id),
                LikeInfos = CircleUtility.GetLikes(this.request.UserId, model.id)
            };

            BindInfo(res.CircleInfo);

            return res;
        }

        private void BindInfo(CircleInfo info)
        {
            info.Images = db.t_circle_image.Where(i => i.circle_id == info.CircleId).Select(i => new ImageInfo()
            {
                Url = i.image_url,
                Height = i.height,
                Width = i.width
            }).ToList();

            var user = db.t_user.Where(u => u.id == info.PublishUserId).FirstOrDefault();
            string friendRemark = db.t_user_friend.Where(f => f.user_id == this.request.UserId && f.friend_user_id == info.PublishUserId).Select(f => f.friend_user_remark).FirstOrDefault();
            info.PublishUserName = UserUtility.GetShowName(friendRemark, user.name);
            info.PublishUserImage = user.head_image;
            info.IsLiked = db.t_circle_like.Where(l => l.circle_id == info.CircleId && l.like_user_id == this.request.UserId && l.is_delete == false).Select(l => l.id).FirstOrDefault() > 0 ? true : false;
        }
    }
}
