﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Entity.Extends
{
    public class NearExt
    {
    }

    public class NearCircleInfo
    {
        public int NearCircleId { get; set; }
        public int UserId { get; set; }
        public int ContentType { get; set; }
        public string Content { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
        public string LinkImage { get; set; }
        public System.DateTime PublishTime { get; set; }
        public string FromPosDesc { get; set; }
        public List<string> Images { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
    }
}