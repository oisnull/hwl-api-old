using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using HWL.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle.Service
{
    public class GetCircleLikes : GMSF.ServiceHandler<GetCircleLikesRequestBody, GetCircleLikesResponseBody>
    {
        public GetCircleLikes(GetCircleLikesRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.CircleId <= 0)
            {
                throw new ArgumentNullException("CircleId");
            }
        }

        public override GetCircleLikesResponseBody ExecuteCore()
        {
            GetCircleLikesResponseBody res = new GetCircleLikesResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var likes = db.t_circle_like.Where(c => c.circle_id == this.request.CircleId && c.is_delete == false)
                            .Select(c => new
                            {
                                LikeId = c.id,
                                CircleId = c.circle_id,
                                LikeUserId = c.like_user_id,
                                LikeTime = c.like_time
                            }).OrderBy(c=>c.LikeId).ToList();
                if (likes == null || likes.Count <= 0) return res;
                res.CircleLikeInfos = new List<CircleLikeInfo>();

                var userIds = likes.Select(c => c.LikeUserId).ToList();
                var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();

                likes.ForEach(f =>
                {
                    CircleLikeInfo model = new CircleLikeInfo()
                    {
                        LikeId = f.LikeId,
                        LikeUserId = f.LikeUserId,
                        CircleId = f.CircleId,
                        LikeTime = f.LikeTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    };

                    if (userList != null && userList.Count > 0)
                    {
                        var likeUser = userList.Where(u => u.id == f.LikeUserId).FirstOrDefault();
                        if (likeUser != null)
                        {
                            model.LikeUserName = UserUtility.GetShowName(likeUser.name, likeUser.symbol);
                            model.LikeUserImage = likeUser.head_image;
                        }
                    }
                    res.CircleLikeInfos.Add(model);
                });
            }

            return res;
        }
    }
}
