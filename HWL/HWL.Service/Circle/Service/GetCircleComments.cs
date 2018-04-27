﻿using HWL.Entity;
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
    public class GetCircleComments : GMSF.ServiceHandler<GetCircleCommentsRequestBody, GetCircleCommentsResponseBody>
    {
        public GetCircleComments(GetCircleCommentsRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            //if (this.request.UserId <= 0)
            //{
            //    throw new ArgumentNullException("UserId");
            //}
            if (this.request.CircleId <= 0)
            {
                throw new ArgumentNullException("CircleId");
            }
            if (this.request.Count <= 0)
            {
                this.request.Count = 15;
            }
        }

        public override GetCircleCommentsResponseBody ExecuteCore()
        {
            GetCircleCommentsResponseBody res = new GetCircleCommentsResponseBody();
            System.Threading.Thread.Sleep(1000);
            using (HWLEntities db = new HWLEntities())
            {
                var comments = db.t_circle_comment.Where(c => c.circle_id == this.request.CircleId && c.id > this.request.LastCommentId)
                    .Take(this.request.Count)
                    .Select(c => new
                    {
                        Id = c.id,
                        CircleId = c.circle_id,
                        CommentUserId = c.com_user_id,
                        ReplyUserId = c.reply_user_id,
                        Content = c.com_content,
                        CommentTime = c.comment_time
                    }).OrderBy(c => c.Id).ToList();
                if (comments == null || comments.Count <= 0) return res;
                res.CircleCommentInfos = new List<CircleCommentInfo>();

                var userIds = comments.Select(c => c.CommentUserId).Union(comments.Select(c => c.ReplyUserId)).ToList();
                var userList = db.t_user.Where(i => userIds.Contains(i.id)).Select(i => new { i.id, i.name, i.symbol, i.head_image }).ToList();

                comments.ForEach(f =>
                {
                    CircleCommentInfo model = new CircleCommentInfo()
                    {
                        CommentId = f.Id,
                        Content = f.Content,
                        CircleId = f.CircleId,
                        CommentTime = f.CommentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        CommentUserId = f.CommentUserId,
                        CommentUserImage = null,
                        CommentUserName = null,
                        ReplyUserId = f.ReplyUserId,
                        ReplyUserName = null,
                        ReplyUserImage = null,
                    };

                    if (userList != null && userList.Count > 0)
                    {
                        if (f.CommentUserId > 0)
                        {
                            var comUser = userList.Where(u => u.id == f.CommentUserId).FirstOrDefault();
                            model.CommentUserName = UserUtility.GetShowName(comUser.name, comUser.symbol);
                            model.CommentUserImage = comUser.head_image;
                        }

                        if (f.ReplyUserId > 0)
                        {
                            var repUser = userList.Where(u => u.id == f.ReplyUserId).FirstOrDefault();
                            model.ReplyUserName = UserUtility.GetShowName(repUser.name, repUser.symbol);
                            model.ReplyUserImage = repUser.head_image;
                        }
                    }
                    res.CircleCommentInfos.Add(model);
                });
            }

            return res;
        }
    }
}