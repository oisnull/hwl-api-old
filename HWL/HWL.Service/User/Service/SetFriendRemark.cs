using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class SetFriendRemark : GMSF.ServiceHandler<SetFriendRemarkRequestBody, SetFriendRemarkResponseBody>
    {
        public SetFriendRemark(SetFriendRemarkRequestBody request) : base(request)
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
            //if (string.IsNullOrEmpty(this.request.FriendUserRemark))
            //{
            //    throw new Exception("好友备注不能为空");
            //}
        }

        public override SetFriendRemarkResponseBody ExecuteCore()
        {
            SetFriendRemarkResponseBody res = new SetFriendRemarkResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var friendModel = db.t_user_friend.Where(u => u.user_id == this.request.MyUserId && u.friend_user_id == this.request.FriendUserId).FirstOrDefault();
                if (friendModel == null) throw new Exception("您没有权限备注当前用户");

                friendModel.friend_user_remark = this.request.FriendUserRemark;
                //friendModel.friend_first_spell = UserUtility.GetRemarkFirstSpell(this.request.FriendUserRemark.FirstOrDefault().ToString());

                db.SaveChanges();
                res.Status = ResultStatus.Success;
                //res.FirstSpell = friendModel.friend_first_spell;

                ////重置redis缓存中的remark
                //var t = new Task(() =>
                //{
                //    Redis.UserAction userAction = new Redis.UserAction();
                //    userAction.SetFriendRemarkExpire(this.request.MyUserId, this.request.FriendUserId);
                //});
                //t.Start();
            }
            return res;
        }
    }
}
