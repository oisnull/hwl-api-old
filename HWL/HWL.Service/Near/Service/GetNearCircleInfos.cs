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
            if (this.request.Lat < 0 && this.request.Lon < 0)
            {
                throw new Exception("位置参数错误");
            }
        }

        public override GetNearCircleInfosResponseBody ExecuteCore()
        {
            GetNearCircleInfosResponseBody res = new GetNearCircleInfosResponseBody();

            //从redis中获取附近圈子信息id列表
            List<int> ids = new NearCircleAction().GetNearCircleIds(this.request.Lon, this.request.Lat);
            if (ids == null || ids.Count <= 0) return res;

            using (HWLEntities db = new HWLEntities())
            {
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
            }

            return res;
        }
    }
}
