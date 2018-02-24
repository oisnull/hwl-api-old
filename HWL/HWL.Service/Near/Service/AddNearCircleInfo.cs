using HWL.Entity;
using HWL.Redis;
using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class AddNearCircleInfo : GMSF.ServiceHandler<AddNearCircleInfoRequestBody, AddNearCircleInfoResponseBody>
    {
        public AddNearCircleInfo(AddNearCircleInfoRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (
                string.IsNullOrEmpty(this.request.Content)
                && (this.request.Images == null || this.request.Images.Count <= 0)
                && string.IsNullOrEmpty(this.request.LinkTitle)
                )
            {
                throw new Exception("不能发布空信息");
            }

            if (this.request.UserId <= 0)
            {
                throw new Exception("发布的用户id不能为空");
            }
        }

        public override AddNearCircleInfoResponseBody ExecuteCore()
        {
            AddNearCircleInfoResponseBody res = new AddNearCircleInfoResponseBody();
            using (HWLEntities db = new HWLEntities())
            {
                t_near_circle model = new t_near_circle()
                {
                    user_id = this.request.UserId,
                    content_info = this.request.Content,
                    content_type = 0,
                    link_image = this.request.LinkImage,
                    link_title = this.request.LinkTitle,
                    link_url = this.request.LinkUrl,
                    lat = this.request.Lat,
                    lon = this.request.Lon,
                    id = 0,
                    pos_id = 0,
                    comment_count = 0,
                    image_count = 0,
                    like_count = 0,
                    publish_time = DateTime.Now,
                };
                db.t_near_circle.Add(model);
                db.SaveChanges();

                res.NearCircleId = model.id;

                //向redis中添加信息的位置数据
                if (res.NearCircleId > 0)
                {
                    bool succ = new NearCircleAction().CreateNearCirclePos(res.NearCircleId, this.request.Lon, this.request.Lat);
                    if (!succ)//如果添加失败,则将数据库中已经添加的数据删除
                    {
                        db.t_near_circle.Remove(model);
                        db.SaveChanges();
                        throw new Exception("发布附近信息失败");
                    }
                }
            }
            return res;
        }
    }
}
