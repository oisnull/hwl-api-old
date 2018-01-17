using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class AddFriend : GMSF.ServiceHandler<AddFriendRequestBody, AddFriendResponseBody>
    {
        public AddFriend(AddFriendRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.MyUserId <= 0)
            {
                throw new Exception("用户参数错误");
            }
            if (string.IsNullOrEmpty(this.request.MyRemark))
            {
                throw new Exception("我的备注不能为空");
            }

            if (this.request.FriendUserId <= 0)
            {
                throw new Exception("用户好友参数错误");
            }
            if (string.IsNullOrEmpty(this.request.FriendUserRemark))
            {
                throw new Exception("好友备注不能为空");
            }
        }

        public override AddFriendResponseBody ExecuteCore()
        {
            AddFriendResponseBody res = new AddFriendResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                int myFriendCount = db.t_user_friend.Where(u => u.user_id == this.request.MyUserId).Count();//获取我的好友数量
                if (myFriendCount >= ConfigManager.UserAddFriendTotalCount)
                {
                    throw new Exception("您添加的好友达到上限,请清理后再添加");
                }

                DateTime start = DateTime.Now.Date;
                DateTime end = DateTime.Now.AddDays(1).Date;
                int myFriendDayCount = db.t_user_friend.Where(u => u.user_id == this.request.MyUserId && u.add_time > start && u.add_time < end).Count();//获取我今天添加的好友数量
                if (myFriendDayCount >= ConfigManager.UserAddFriendDayCount)
                {
                    throw new Exception("您今天添加的好友已经达到上限");
                }

                var userFriendCount = db.t_user_friend.Where(u => u.user_id == this.request.FriendUserId).Count();//获取添加用户的好友数量
                if (userFriendCount >= ConfigManager.UserAddFriendTotalCount)
                {
                    throw new Exception("您添加的用户所拥有的好友数量达到上限");
                }
                int userFriendDayCount = db.t_user_friend.Where(u => u.user_id == this.request.FriendUserId && u.add_time > start && u.add_time < end).Count();//获取用户今天添加的好友数量
                if (userFriendDayCount >= ConfigManager.UserAddFriendDayCount)
                {
                    throw new Exception("今天添加的好友已经达到上限,请您明天再试");
                }

                var myFriendModel = db.t_user_friend.Where(u => u.user_id == this.request.MyUserId && u.friend_user_id == this.request.FriendUserId).FirstOrDefault();
                if (myFriendModel != null)
                {
                    myFriendModel.friend_user_remark = this.request.FriendUserRemark;
                    myFriendModel.friend_first_spell = UserUtility.GetRemarkFirstSpell(this.request.FriendUserRemark.FirstOrDefault().ToString());
                }
                else
                {
                    myFriendModel = new t_user_friend()
                    {
                        add_time = DateTime.Now,
                        user_id = this.request.MyUserId,
                        friend_user_id = this.request.FriendUserId,
                        friend_user_remark = this.request.FriendUserRemark,
                        friend_first_spell = UserUtility.GetRemarkFirstSpell(this.request.FriendUserRemark.FirstOrDefault().ToString())
                    };
                    db.t_user_friend.Add(myFriendModel);
                }

                var toFriendModel = db.t_user_friend.Where(u => u.user_id == this.request.FriendUserId && u.friend_user_id == this.request.MyUserId).FirstOrDefault();
                if (toFriendModel != null)
                {
                    toFriendModel.friend_user_remark = this.request.FriendUserRemark;
                    toFriendModel.friend_first_spell = UserUtility.GetRemarkFirstSpell(this.request.FriendUserRemark.FirstOrDefault().ToString());
                }
                else
                {
                    toFriendModel = new t_user_friend()
                    {
                        add_time = DateTime.Now,
                        user_id = this.request.FriendUserId,
                        friend_user_id = this.request.MyUserId,
                        friend_user_remark = this.request.MyRemark,
                        friend_first_spell = UserUtility.GetRemarkFirstSpell(this.request.MyRemark.FirstOrDefault().ToString())
                    };
                    db.t_user_friend.Add(toFriendModel);
                }

                db.SaveChanges();
                res.Status = ResultStatus.Success;
                res.FirstSpell = toFriendModel.friend_first_spell;
            }

            return res;
        }
    }
}
