using HWL.Entity;
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
        public static List<CircleCommentInfo> GetComments(int userId, int circleId, int count = 30)
        {
            if (circleId <= 0) return null;
            GetCircleCommentsResponseBody response = new GetCircleComments(new GetCircleCommentsRequestBody()
            {
                UserId = userId,
                CircleId = circleId,
                LastCommentId = 0,
                Count = count
            }).Execute();
            if (response == null || response.CircleCommentInfos == null || response.CircleCommentInfos.Count <= 0) return null;
            return response.CircleCommentInfos;
        }

        public static List<CircleLikeInfo> GetLikes(int userId, int CircleId)
        {
            if (CircleId <= 0) return null;
            GetCircleLikesResponseBody response = new GetCircleLikes(new GetCircleLikesRequestBody()
            {
                UserId = userId,
                CircleId = CircleId,
            }).Execute();
            if (response == null || response.CircleLikeInfos == null || response.CircleLikeInfos.Count <= 0) return null;
            return response.CircleLikeInfos;
        }

        /// <summary>
        /// bool 是否为图片
        /// 如果是图片，则返回图片的地址
        /// 如果不是图片，返回内容信息
        /// </summary>
        public static Tuple<bool, List<string>> GetCircleNewInfo(HWLEntities db, int userId, int count = 3)
        {
            if (userId <= 0) return null;
            if (count <= 0) count = 3;

            var infos = db.t_circle.Where(c => c.user_id == userId)
                .Select(c => new { c.id, c.circle_content, c.image_count })
                .OrderByDescending(c => c.id)
                .Take(count)
                .ToList();
            if (infos == null) return null;

            if (infos.Sum(i => i.image_count) > 0)
            {
                List<int> cids = infos.Select(i => i.id).ToList();
                List<string> images = db.t_circle_image.Where(i => cids.Contains(i.circle_id))
                    .OrderByDescending(i => i.circle_id)
                    .Select(i => i.image_url)
                    .Take(count)
                    .ToList();
                if (images != null && images.Count > 0)
                {
                    return new Tuple<bool, List<string>>(true, images);
                }
            }

            return new Tuple<bool, List<string>>(false, infos.Select(i => i.circle_content).ToList());
        }
    }
}
