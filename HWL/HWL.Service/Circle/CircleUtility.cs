using HWL.Entity.Extends;
using HWL.Service.Circle.Body;
using HWL.Service.Circle.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Circle
{
    public class CircleUtility
    {
        public static List<CircleCommentInfo> GetComments(int CircleId, int count = 30)
        {
            if (CircleId <= 0) return null;
            GetCircleCommentsResponseBody response = new GetCircleComments(new GetCircleCommentsRequestBody()
            {
                CircleId = CircleId,
                LastCommentId = 0,
                Count = count
            }).Execute();
            if (response == null || response.CircleCommentInfos == null || response.CircleCommentInfos.Count <= 0) return null;
            return response.CircleCommentInfos;
        }

        public static List<CircleLikeInfo> GetLikes(int CircleId)
        {
            if (CircleId <= 0) return null;
            GetCircleLikesResponseBody response = new GetCircleLikes(new GetCircleLikesRequestBody()
            {
                CircleId = CircleId,
            }).Execute();
            if (response == null || response.CircleLikeInfos == null || response.CircleLikeInfos.Count <= 0) return null;
            return response.CircleLikeInfos;
        }
    }
}
