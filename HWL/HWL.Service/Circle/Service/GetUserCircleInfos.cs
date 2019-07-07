using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using HWL.Service.Generic;
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

            //if (this.request.PageIndex <= 0)
            //    this.request.PageIndex = 1;

            if (this.request.Count <= 0)
                this.request.Count = 15;
        }

        public override GetUserCircleInfosResponseBody ExecuteCore()
        {
            GetUserCircleInfosResponseBody res = new GetUserCircleInfosResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var postUser = db.t_user.Where(u => u.id == this.request.ViewUserId).FirstOrDefault();
                if (postUser == null) throw new Exception("用户不存在");
                res.ViewUserId = postUser.id;
                res.ViewUserImage = postUser.head_image;
                res.ViewUserName = postUser.name;
                //res.CircleBackImage = postUser.circle_back_image;
                //res.LifeNotes = postUser.life_notes;

                IQueryable<t_circle> query = db.t_circle.OrderByDescending(r => r.id);
                if (this.request.ViewUserId > 0)
                {
                    query = query.Where(q => q.user_id == this.request.ViewUserId);
                }
                if (this.request.MinCircleId > 0)
                {
                    query = query.Where(q => q.id < this.request.MinCircleId).Take(this.request.Count);
                }

                var list = query.ToList();
                if (list == null || list.Count <= 0) return res;

                if (this.request.CircleMatchInfos != null && this.request.CircleMatchInfos.Count > 0)
                {
                    list.RemoveAll(r => this.request.CircleMatchInfos.Exists(c => c.CircleId == r.id && c.UpdateTime == GenericUtility.FormatDate2(r.update_time)));
                }

                res.CircleInfos = list.ConvertAll(q => new CircleInfo
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
                    PublishTime = GenericUtility.FormatDate(q.publish_time),
                    UpdateTime = GenericUtility.FormatDate2(q.update_time),

                    //IsLike = false,
                    //CommentInfos = null,
                    //Images = null,
                    //LikeUserInfos = null,
                    //PostUserInfo = null,
                });

                GetCircleInfos.BindCircleInfos(db, this.request.ViewUserId, res.CircleInfos);
            }

            return res;
        }
    }
}
