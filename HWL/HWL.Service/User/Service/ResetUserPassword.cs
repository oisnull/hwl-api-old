using GMSF;
using HWL.Entity;
using HWL.Service.User.Body;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class ResetUserPassword : ServiceHandler<ResetUserPasswordRequestBody, ResetUserPasswordResponseBody>
    {
        public ResetUserPassword(ResetUserPasswordRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new Exception("帐号错误");
            }
            if (string.IsNullOrEmpty(this.request.OldPassword))
            {
                throw new Exception("旧密码不能为空");
            }
            if (string.IsNullOrEmpty(this.request.Password))
            {
                throw new Exception("密码不能为空");
            }
            if (string.IsNullOrEmpty(this.request.PasswordOK))
            {
                throw new Exception("密码确认不能为空");
            }
            if (this.request.Password.Contains(" "))
            {
                throw new Exception("密码中不能包含空格");
            }
            if (this.request.PasswordOK.Trim() != this.request.Password.Trim())
            {
                throw new Exception("两次密码输入不一致");
            }
        }

        public override ResetUserPasswordResponseBody ExecuteCore()
        {
            ResetUserPasswordResponseBody res = new ResetUserPasswordResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var user = db.t_user.Where(u => u.id == this.request.UserId).FirstOrDefault();
                if (user == null) throw new Exception("帐号错误,密码重置失败!");
                if (user.password != this.request.OldPassword) throw new Exception("旧密码错误");

                //更新用户密码信息
                user.password = this.request.PasswordOK;
                db.SaveChanges();
                try
                {
                    //清除用户之前登录用过的TOKEN
                    new Redis.UserAction().RemoveUserToken(user.id);
                }
                catch (Exception)
                {
                }
                res.Status = ResultStatus.Success;
            }

            return res;
        }
    }
}
