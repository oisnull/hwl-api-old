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
        public static List<NearCircleCommentInfo> GetNearComments(int nearCircleId, int count = 30)
        {
            if (nearCircleId <= 0) return null;
            GetNearCommentsResponseBody response = new GetNearComments(new GetNearCommentsRequestBody()
            {
                NearCircleId = nearCircleId,
                LastCommentId = 0,
                Count = count
            }).Execute();
            if (response == null || response.NearCircleCommentInfos == null || response.NearCircleCommentInfos.Count <= 0) return null;
            return response.NearCircleCommentInfos;
        }

        public static List<NearCircleLikeInfo> GetNearLikes(int nearCircleId)
        {
            if (nearCircleId <= 0) return null;
            GetNearLikesResponseBody response = new GetNearLikes(new GetNearLikesRequestBody()
            {
                NearCircleId = nearCircleId,
            }).Execute();
            if (response == null || response.NearCircleLikeInfos == null || response.NearCircleLikeInfos.Count <= 0) return null;
            return response.NearCircleLikeInfos;
        }
    }
}
