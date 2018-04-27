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
                && string.IsNullOrEmpty(this.request.LinkUrl)
                && string.IsNullOrEmpty(this.request.LinkTitle)
                )
            {
                throw new Exception("不能发布空信息");
            }

            if (this.request.UserId <= 0)
            {
                throw new Exception("发布的用户id不能为空");
            }
            if (this.request.Lat <= 0 && this.request.Lon <= 0)
            {
                throw new Exception("位置参数错误");
            }
        }

        private CircleContentType GetContentType()
        {
            if (!string.IsNullOrEmpty(this.request.LinkUrl) && !string.IsNullOrEmpty(this.request.LinkTitle))
            {
                return CircleContentType.Link;
            }
            if (!string.IsNullOrEmpty(this.request.Content) && !(this.request.Images == null || this.request.Images.Count <= 0))
            {
                return CircleContentType.TextImage;
            }
            if (!string.IsNullOrEmpty(this.request.Content))
            {
                return CircleContentType.Text;
            }
            if (!(this.request.Images == null || this.request.Images.Count <= 0))
            {
                return CircleContentType.Image;
            }
            return CircleContentType.Other;
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
                    content_type = GetContentType(),
                    link_image = this.request.LinkImage,
                    link_title = this.request.LinkTitle,
                    link_url = this.request.LinkUrl,
                    lat = this.request.Lat,
                    lon = this.request.Lon,
                    id = 0,
                    pos_id = this.request.PosId,
                    pos_desc = this.request.PosDesc,
                    comment_count = 0,
                    image_count = this.request.Images != null ? this.request.Images.Count : 0,
                    like_count = 0,
                    publish_time = DateTime.Now,
                };
                db.t_near_circle.Add(model);
                db.SaveChanges();

                res.NearCircleId = model.id;
                res.ContentType = model.content_type;
                res.PublishTime = model.publish_time;

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
                    else
                    {
                        if (this.request.Images != null && this.request.Images.Count > 0)
                        {
                            //添加图片
                            List<t_near_circle_image> imgModels = new List<t_near_circle_image>();
                            this.request.Images.ForEach((i) =>
                            {
                                if (string.IsNullOrEmpty(i.Url)) return;
                                imgModels.Add(new t_near_circle_image()
                                {
                                    near_circle_id = model.id,
                                    near_circle_user_id = model.user_id,
                                    image_url = i.Url,
                                    height = i.Height,
                                    width = i.Width
                                });
                            });

                            if (imgModels == null || imgModels.Count <= 0) return res;

                            try
                            {
                                db.t_near_circle_image.AddRange(imgModels);
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                //可以忽略这个错误
                            }
                        }
                    }
                }
            }
            return res;
        }
    }
}
