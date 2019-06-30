using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.Generic;
using HWL.Service.Near.Body;
using HWL.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class GetNearLikes : GMSF.ServiceHandler<GetNearLikesRequestBody, GetNearLikesResponseBody>
    {
        public GetNearLikes(GetNearLikesRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
            if (this.request.NearCircleId <= 0)
            {
                throw new ArgumentNullException("NearCircleId");
            }
        }

        public override GetNearLikesResponseBody ExecuteCore()
        {
            GetNearLikesResponseBody res = new GetNearLikesResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var likes = db.t_near_circle_like.Where(c => c.near_circle_id == this.request.NearCircleId && c.is_delete == false)
                            .Select(c => new
                            {
                                LikeId = c.id,
                                NearCircleId = c.near_circle_id,
                                LikeUserId = c.like_user_id,
                                LikeTime = c.like_time
                            }).OrderBy(c => c.LikeId).ToList();
                if (likes == null || likes.Count <= 0) return res;
                res.NearCircleLikeInfos = new List<NearCircleLikeInfo>();

                var userIds = likes.Select(c => c.LikeUserId).ToList();
                var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.head_image }).ToList();
                var friendList = db.t_user_friend.Where(f => f.user_id == this.request.UserId && userIds.Contains(f.friend_user_id)).Select(f => new { f.friend_user_id, f.friend_user_remark }).ToList();

                likes.ForEach(f =>
                {
                    NearCircleLikeInfo model = new NearCircleLikeInfo()
                    {
                        LikeId = f.LikeId,
                        LikeUserId = f.LikeUserId,
                        NearCircleId = f.NearCircleId,
                        LikeTime = GenericUtility.FormatDate(f.LikeTime),
                    };

                    if (userList != null && userList.Count > 0)
                    {
                        var likeUser = userList.Where(u => u.id == f.LikeUserId).FirstOrDefault();
                        if (likeUser != null)
                        {
                            string friendRemark = friendList != null ? friendList.Where(r => r.friend_user_id == f.LikeUserId).Select(r => r.friend_user_remark).FirstOrDefault() : null;
                            model.LikeUserName = UserUtility.GetShowName(friendRemark, likeUser.name);
                            model.LikeUserImage = likeUser.head_image;
                        }
                    }
                    res.NearCircleLikeInfos.Add(model);
                });
            }

            return res;
        }
    }
}
