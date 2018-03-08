using HWL.Entity;
using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class GetNearCircleDetail : GMSF.ServiceHandler<GetNearCircleDetailRequestBody, GetNearCircleDetailResponseBody>
    {
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
            using (HWLEntities db = new HWLEntities())
            {
                var model = db.t_near_circle.Where(c => c.id == this.request.NearCircleId).FirstOrDefault();
                if (model == null)
                {
                    throw new Exception("信息不存在");
                }

                res.NearCircleInfo = new Entity.Extends.NearCircleInfo()
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
                    PublishTime = model.publish_time,
                    PublishUserId = model.user_id,
                };
            }
            return res;
        }
    }
}
