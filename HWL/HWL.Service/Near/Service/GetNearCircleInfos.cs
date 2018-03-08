using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Redis;
using HWL.Service.Near.Body;
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
            if (this.request.Lat < 0 && this.request.Lon < 0)
            {
                throw new Exception("位置参数错误");
            }
            if (this.request.Count <= 0)
                this.request.Count = 10;
        }

        public override GetNearCircleInfosResponseBody ExecuteCore()
        {
            GetNearCircleInfosResponseBody res = new GetNearCircleInfosResponseBody();

            //从redis中获取附近圈子信息id列表
            List<int> geoIdList = new NearCircleAction().GetNearCircleIds(this.request.Lon, this.request.Lat);
            if (geoIdList == null || geoIdList.Count <= 0) return res;

            List<int> ids = geoIdList.Where(g => g > this.request.LastNearCiclreId).Take(this.request.Count).ToList();

            res.NearCircleInfos = db.t_near_circle.Where(c => ids.Contains(c.id)).Select(c => new NearCircleInfo
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
                PublishTime = c.publish_time,
                PublishUserId = c.user_id,
            }).ToList();

            //获取图片列表
            if (res.NearCircleInfos != null && res.NearCircleInfos.Count > 0)
            {
                List<int> circleIds = res.NearCircleInfos.Where(n => n.ContentType == CircleContentType.Image).Select(n => n.NearCircleId).ToList();
                bindImages(res.NearCircleInfos,circleIds);
            }

            return res;
        }

        private void bindImages(List<NearCircleInfo> infos, List<int> circleIds)
        {
            if (circleIds == null || circleIds.Count <= 0) return;
            var imageList = db.t_near_circle_image.Where(i => circleIds.Contains(i.near_circle_id)).Select(i => new { i.near_circle_id, i.image_url }).ToList();
            if (imageList == null || imageList.Count <= 0) return;
            foreach (var item in infos)
            {
                item.Images = imageList.Where(i => i.near_circle_id == item.NearCircleId).Select(i => i.image_url).ToList();
            }
        }
    }
}
