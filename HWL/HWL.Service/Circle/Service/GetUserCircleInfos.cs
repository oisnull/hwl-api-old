using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class GetUserCircleInfos : GMSF.ServiceHandler<GetUserCircleInfosRequestBody, GetUserCircleInfosResponseBody>
    {
        public GetUserCircleInfos(GetUserCircleInfosRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
            if (this.request.ViewUserId <= 0)
            {
                throw new Exception("查看用户的参数错误");
            }

            if (this.request.PageIndex <= 0)
                this.request.PageIndex = 1;

            if (this.request.PageSize <= 0)
                this.request.PageSize = 20;
        }

        public override GetUserCircleInfosResponseBody ExecuteCore()
        {
            GetUserCircleInfosResponseBody res = new GetUserCircleInfosResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                //var postUser = db.t_user.Where(u => u.id == this.request.ViewUserId).FirstOrDefault();
                //if (postUser == null) throw new Exception("用户不存在");
                //res.ViewUserId = postUser.id;
                //res.ViewUserImage = postUser.head_image;
                //res.ViewUserName = postUser.name;

                IQueryable<t_circle> query = db.t_circle.OrderByDescending(r => r.id);
                if (this.request.ViewUserId > 0)
                {
                    query = query.Where(q => q.user_id == this.request.ViewUserId);
                }
                var list = query.Skip(this.request.PageSize * (this.request.PageIndex - 1)).Take(this.request.PageSize).ToList();
                if (list == null || list.Count <= 0) return res;

                var circleList = list.ConvertAll(q => new CircleInfo
                {
                    CircleId = q.id,
                    UserId = q.user_id,
                    ContentType = q.content_type,
                    CircleContent = q.circle_content,
                    CommentCount = q.comment_count,
                    ImageCount = q.image_count,
                    Lat = q.lat,
                    LikeCount = q.like_count,
                    LinkImage = q.link_image,
                    LinkTitle = q.link_title,
                    LinkUrl = q.link_url,
                    Lon = q.lon,
                    PosId = q.pos_id,
                    PublishTime = q.publish_time.ToString("yyyy-MM-dd HH:mm:ss"),

                    //IsLike = false,
                    //CommentInfos = null,
                    //Images = null,
                    //LikeUserInfos = null,
                    //PostUserInfo = null,

                });

                if (circleList == null || circleList.Count <= 0) return res;
                res.CircleInfos = circleList;

                List<int> circleIds = circleList.Select(c => c.CircleId).ToList();

                //获取发布的图片
                var images = db.t_circle_image.Where(i => circleIds.Contains(i.circle_id)).ToList();

                //绑定到列表
                circleList.ForEach(f =>
                {
                    if (f.ImageCount > 0 && images != null && images.Count > 0)
                    {
                        f.Images = images.Where(i => i.circle_id == f.CircleId).Select(i => new ImageInfo()
                        {
                            Url = i.image_url,
                            Height = i.height,
                            Width = i.width
                        }).ToList();
                    }
                });
            }

            return res;
        }
    }
}
