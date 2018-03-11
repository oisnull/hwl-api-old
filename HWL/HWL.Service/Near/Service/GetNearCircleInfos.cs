using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Redis;
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
            db = new HWLEntities();
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

            List<int> ids = geoIdList.Where(g => g > this.request.LastNearCiclreId).Take(this.request.Count).ToList();

            var list = db.t_near_circle.Where(c => ids.Contains(c.id)).OrderByDescending(c => c.id).ToList();

            if (list == null || list.Count <= 0) return res;

            res.NearCircleInfos = list.Select(c => new NearCircleInfo
            {
                NearCircleId = c.id,
                CommentCount = c.comment_count,
                Content = c.content_info,
                ContentType = c.content_type,
                FromPosDesc = c.pos_id + "",
                //Images = null,
                LikeCount = c.like_count,
                LinkImage = c.link_image,
                LinkTitle = c.link_title,
                LinkUrl = c.link_url,
                PublishTime = c.publish_time.ToString("yyyy-MM-dd HH:mm:ss"),
                PublishUserId = c.user_id,
            }).ToList();

            BindImages(res.NearCircleInfos, res.NearCircleInfos.Where(n => n.ContentType == CircleContentType.Image).Select(n => n.NearCircleId).ToList());
            BindUser(res.NearCircleInfos, res.NearCircleInfos.Select(u => u.PublishUserId).Distinct().ToList());

            return res;
        }

        private void BindImages(List<NearCircleInfo> infos, List<int> circleIds)
        {
            if (circleIds == null || circleIds.Count <= 0) return;
            var imageList = db.t_near_circle_image.Where(i => circleIds.Contains(i.near_circle_id)).Select(i => new { i.near_circle_id, i.image_url }).ToList();
            if (imageList == null || imageList.Count <= 0) return;
            foreach (var item in infos)
            {
                item.Images = imageList.Where(i => i.near_circle_id == item.NearCircleId).Select(i => i.image_url).ToList();
            }
        }

        private void BindUser(List<NearCircleInfo> infos, List<int> userIds)
        {
            if (userIds == null || userIds.Count <= 0) return;
            var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();
            if (userList == null || userList.Count <= 0) return;
            foreach (var item in infos)
            {
                var user = userList.Where(u => u.id == item.PublishUserId).FirstOrDefault();
                item.PublishUserName = UserUtility.GetShowName(user.name, user.symbol);
                item.PublishUserImage = user.head_image;
            }
        }
    }
}
