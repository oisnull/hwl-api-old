using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Entity.Extends
{
    public class CircleExt
    {

    }

    public class CircleInfo
    {
        public int CircleId { get; set; }
        public CircleContentType ContentType { get; set; }
        public string CircleContent { get; set; }
        public string PublishTime { get; set; }
        public string UpdateTime { get; set; }
        public int PosId { get; set; }
        public string PosDesc { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string LinkUrl { get; set; }
        public string LinkTitle { get; set; }
        public string LinkImage { get; set; }
        public int ImageCount { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        /// <summary>
        /// 是否点赞过
        /// </summary>
        public bool IsLiked { get; set; }

        public List<ImageInfo> Images { get; set; }

        public int PublishUserId { get; set; }
        public string PublishUserName { get; set; }
        public string PublishUserImage { get; set; }

        public List<CircleLikeInfo> LikeInfos { get; set; }

        public List<CircleCommentInfo> CommentInfos { get; set; }
    }

    public class CircleCommentInfo
    {
        public int CommentId { get; set; }
        public int CircleId { get; set; }


        public int CommentUserId { get; set; }
        public string CommentUserName { get; set; }
        public string CommentUserImage { get; set; }

        public int ReplyUserId { get; set; }
        public string ReplyUserName { get; set; }
        public string ReplyUserImage { get; set; }

        public string Content { get; set; }
        public string CommentTime { get; set; }
    }

    public class CircleLikeInfo
    {
        public int LikeId { get; set; }
        public int CircleId { get; set; }
        public int LikeUserId { get; set; }
        public string LikeUserName { get; set; }
        public string LikeUserImage { get; set; }
        public string LikeTime { get; set; }
    }

    public class CircleMatchInfo
    {
        public CircleMatchInfo() { }

        public CircleMatchInfo(int circleId, string updateTime)
        {
            this.CircleId = circleId;
            this.UpdateTime = updateTime;
        }

        public int CircleId { get; set; }
        public string UpdateTime { get; set; }
    }
}
