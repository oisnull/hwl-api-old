using System;
using System.Collections.Generic;

namespace HWL.Entity.Extends
{
    public class NearExt
    {
    }

    public class NearCircleInfo
    {
        public int NearCircleId { get; set; }
        public int PublishUserId { get; set; }
        public string PublishUserName { get; set; }
        public string PublishUserImage { get; set; }
        public CircleContentType ContentType { get; set; }
        public string Content { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
        public string LinkImage { get; set; }
        public string PublishTime { get; set; }
        public string FromPosDesc { get; set; }
        public List<ImageInfo> Images { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }

        public List<NearCircleLikeInfo> LikeInfos { get; set; }
        public List<NearCircleCommentInfo> CommentInfos { get; set; }
    }

    public class NearCircleCommentInfo
    {
        public int CommentId { get; set; }
        public int NearCircleId { get; set; }

        public int CommentUserId { get; set; }
        public string CommentUserName { get; set; }
        public string CommentUserImage { get; set; }

        public int ReplyUserId { get; set; }
        public string ReplyUserName { get; set; }
        public string ReplyUserImage { get; set; }

        public string Content { get; set; }
        public string CommentTime { get; set; }
    }

    public class NearCircleLikeInfo
    {
        public int LikeId { get; set; }
        public int NearCircleId { get; set; }
        public int LikeUserId { get; set; }
        public string LikeUserName { get; set; }
        public string LikeUserImage { get; set; }
        public string LikeTime { get; set; }
    }
}
