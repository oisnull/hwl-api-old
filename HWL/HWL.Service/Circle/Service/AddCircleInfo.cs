using HWL.Entity;
using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class AddCircleInfo : GMSF.ServiceHandler<AddCircleInfoRequestBody, AddCircleInfoResponseBody>
    {
        public AddCircleInfo(AddCircleInfoRequestBody request) : base(request)
        {
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
                throw new Exception("用户参数错误");
            }
        }

        public override AddCircleInfoResponseBody ExecuteCore()
        {
            AddCircleInfoResponseBody res = new AddCircleInfoResponseBody();
            using (HWLEntities db = new HWLEntities())
            {
                t_circle model = new t_circle()
                {
                    user_id = this.request.UserId,
                    circle_content = this.request.Content,
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
                    image_count = 0,
                    like_count = 0,
                    publish_time = DateTime.Now,
                };
                db.t_circle.Add(model);
                db.SaveChanges();

                res.CircleId = model.id;
                res.ContentType = model.content_type;
                res.PublishTime = model.publish_time;

                if (res.CircleId > 0)
                {
                    if (this.request.Images != null && this.request.Images.Count > 0)
                    {
                        //添加图片
                        List<t_circle_image> imgModels = new List<t_circle_image>();
                        this.request.Images.ForEach((i) =>
                        {
                            if (string.IsNullOrEmpty(i.Url)) return;
                            imgModels.Add(new t_circle_image()
                            {
                                circle_id = model.id,
                                user_id = model.user_id,
                                image_url = i.Url,
                                height = i.Height,
                                width = i.Width
                            });
                        });

                        if (imgModels == null || imgModels.Count <= 0) return res;

                        try
                        {
                            db.t_circle_image.AddRange(imgModels);
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            //可以忽略这个错误
                        }
                    }
                }
            }
            return res;
        }
    }
}
