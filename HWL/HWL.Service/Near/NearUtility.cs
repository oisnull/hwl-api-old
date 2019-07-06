using HWL.Entity.Extends;
using HWL.Service.Near.Body;
using HWL.Service.Near.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near
{
    public class NearUtility
    {
        public const int NEAR_PAGE_COUNT = 15;
        public const int NEAR_COMMENT_PAGE_COUNT = 15;


        public static List<NearCircleCommentInfo> GetNearComments(int userId, int nearCircleId, int count = NEAR_COMMENT_PAGE_COUNT)
        {
            if (nearCircleId <= 0) return null;
            GetNearCommentsResponseBody response = new GetNearComments(new GetNearCommentsRequestBody()
            {
                UserId = userId,
                NearCircleId = nearCircleId,
                LastCommentId = 0,
                Count = count
            }).Execute();
            if (response == null || response.NearCircleCommentInfos == null || response.NearCircleCommentInfos.Count <= 0) return null;
            return response.NearCircleCommentInfos;
        }

        public static List<NearCircleLikeInfo> GetNearLikes(int userId, int nearCircleId)
        {
            if (nearCircleId <= 0) return null;
            GetNearLikesResponseBody response = new GetNearLikes(new GetNearLikesRequestBody()
            {
                UserId = userId,
                NearCircleId = nearCircleId,
            }).Execute();
            if (response == null || response.NearCircleLikeInfos == null || response.NearCircleLikeInfos.Count <= 0) return null;
            return response.NearCircleLikeInfos;
        }
    }
}
