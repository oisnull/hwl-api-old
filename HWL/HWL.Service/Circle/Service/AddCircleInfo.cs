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

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
        }

        public override AddCircleInfoResponseBody ExecuteCore()
        {
            AddCircleInfoResponseBody res = new AddCircleInfoResponseBody();

            //using (HWLEntities db = new HWLEntities())
            //{
            //    CircleContentType circleContentType = 0;
            //    if (!string.IsNullOrEmpty(this.request.LinkUrl))
            //    {
            //        circleContentType = CircleContentType.Link;
            //    }
            //    if (this.request.Images != null && this.request.Images.Count > 0)
            //    {
            //        if (!string.IsNullOrEmpty(this.request.Content))
            //        {
            //            circleContentType = CircleContentType.TextImage;
            //        }
            //        else
            //        {
            //            circleContentType = CircleContentType.Image;
            //        }
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(this.request.Content))
            //        {
            //            circleContentType = CircleContentType.Text;
            //        }
            //    }
            //    if (circleContentType == 0)
            //    {
            //        throw new Exception("发布内容错误");
            //    }

            //    t_circle model = new t_circle()
            //    {
            //        user_id = this.request.UserId,
            //        content_type = circleContentType,
            //        circle_content = this.request.Content,
            //        pos_id = this.request.PosId,
            //        lng = this.request.Longitude,
            //        lat = this.request.Latitude,
            //        link_url = this.request.LinkUrl,
            //        link_title = this.request.LinkTitle,
            //        link_image = this.request.LinkImage,

            //        id = 0,
            //        publish_time = DateTime.Now,
            //        like_count = 0,
            //        comment_count = 0,
            //        image_count = this.request.Images != null && this.request.Images.Count > 0 ? this.request.Images.Count : 0,
            //    };

            //    db.t_circle.Add(model);
            //    db.SaveChanges();

            //    if (model.id > 0 && (model.content_type == CircleContentType.Image || model.content_type == CircleContentType.TextImage))
            //    {
            //        List<t_circle_image> circleImgs = new List<t_circle_image>();

            //        //保存图片
            //        //将base64字符流转换为图片,并保存
            //        string directory = string.Format("cirlce/{0}/", DateTime.Now.ToString("yyyyMMdd"));
            //        foreach (var item in this.request.Images)
            //        {
            //            Base64ImageAction imageAction = new Base64ImageAction(item);
            //            imageAction.SaveDirectory = string.Format("{0}{1}", ConfigService.UploadDirectory, directory);
            //            imageAction.SaveFileName = string.Format("{0}", Guid.NewGuid().ToString());
            //            imageAction.Save();

            //            circleImgs.Add(new t_circle_image()
            //            {
            //                circle_id = model.id,
            //                image_url = ConfigService.FileAccessUrl + (ConfigService.FileAccessUrl.EndsWith("/") ? "" : "/") + directory + imageAction.SaveFileName,
            //                user_id = model.user_id,
            //            });
            //        }

            //        if (circleImgs.Count > 0)
            //        {
            //            db.t_circle_image.AddRange(circleImgs);
            //            db.SaveChanges();
            //        }
            //    }

            //    res.Id = model.id;
            //}

            return res;
        }
    }
}
