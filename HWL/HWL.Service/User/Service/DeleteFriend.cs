using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class DeleteFriend : GMSF.ServiceHandler<DeleteFriendRequestBody, DeleteFriendResponseBody>
    {
        public DeleteFriend(DeleteFriendRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.MyUserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
            if (this.request.FriendUserId <= 0)
            {
                throw new Exception("用户好友参数错误");
            }
            if (this.request.MyUserId == this.request.FriendUserId)
            {
                throw new Exception("不能删除自己");
            }
        }

        public override DeleteFriendResponseBody ExecuteCore()
        {

            DeleteFriendResponseBody res = new DeleteFriendResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                var friendInfo = db.t_user_friend.Where(u => u.user_id == this.request.MyUserId && u.friend_user_id == this.request.FriendUserId).FirstOrDefault();
                if (friendInfo == null)
                {
                    res.Status = ResultStatus.None;
                    return res;
                }

                var myInf = db.t_user_friend.Where(u => u.user_id == this.request.FriendUserId && u.friend_user_id == this.request.MyUserId).FirstOrDefault();
                if (myInf != null)
                {
                    db.t_user_friend.Remove(myInf);
                }

                db.t_user_friend.Remove(friendInfo);
                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }

            return res;
        }
    }
}
