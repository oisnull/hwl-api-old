using HWL.Service.Circle.Body;
using System;
using System.Collections.Generic;
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

            //using (HWLEntities db = new HWLEntities())
            //{
            //    var postUser = db.t_user.Where(u => u.id == this.request.ViewUserId).Select(u => new UserShowInfo()
            //    {
            //        UserId = u.id,
            //        CircleBackImage = u.circle_back_image,
            //        HeadImage = u.head_image,
            //        ShowName = u.nick_name,
            //        LifeNotes = u.life_notes,
            //    }).FirstOrDefault();
            //    if (postUser == null) throw new Exception("用户不存在");
            //    res.UserShowInfo = postUser;

            //    IQueryable<t_circle> query = db.t_circle.OrderByDescending(r => r.id);

            //    if (this.request.ViewUserId > 0)
            //    {
            //        query = query.Where(q => q.user_id == this.request.ViewUserId);
            //    }

            //    var circleList = query.Skip(this.request.PageSize * (this.request.PageIndex - 1)).Take(this.request.PageSize).Select(q => new CircleInfo
            //    {
            //        Id = q.id,
            //        UserId = q.user_id,
            //        ContentType = q.content_type,
            //        CircleContent = q.circle_content,
            //        CommentCount = q.comment_count,
            //        ImageCount = q.image_count,
            //        Lat = q.lat,
            //        LikeCount = q.like_count,
            //        LinkImage = q.link_image,
            //        LinkTitle = q.link_title,
            //        LinkUrl = q.link_url,
            //        Lng = q.lng,
            //        PosId = q.pos_id,
            //        PublishTime = q.publish_time,

            //        //IsLike = false,
            //        //CommentInfos = null,
            //        //Images = null,
            //        //LikeUserInfos = null,
            //        //PostUserInfo = null,

            //    }).ToList();

            //    if (circleList == null || circleList.Count <= 0) return res;
            //    res.CircleInfos = circleList;

            //    List<int> circleIds = circleList.Select(c => c.Id).ToList();

            //    //获取发布的图片
            //    var images = db.t_circle_image.Where(i => circleIds.Contains(i.circle_id)).Select(i => new { CircleId = i.circle_id, ImageUrl = i.image_url }).ToList();

            //    //绑定到列表
            //    circleList.ForEach(f =>
            //    {
            //        f.PublishTimeDesc = f.PublishTime.ToString("MM-dd");

            //        //f.PostUserInfo = postUser;

            //        if (f.ImageCount > 0 && images != null && images.Count > 0)
            //        {
            //            f.Images = images.Where(i => i.CircleId == f.Id).Select(i => i.ImageUrl).ToList();
            //        }
            //    });
            //}

            return res;
        }
    }
}
