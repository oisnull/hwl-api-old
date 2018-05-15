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
    public class SetUserPassword : ServiceHandler<SetUserPasswordRequestBody, SetUserPasswordResponseBody>
    {
        public SetUserPassword(SetUserPasswordRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (string.IsNullOrEmpty(this.request.Email) && string.IsNullOrEmpty(this.request.Mobile))
            {
                throw new Exception("手机或者邮箱不能为空");
            }
            if (string.IsNullOrEmpty(this.request.CheckCode))
            {
                throw new Exception("注册码不能为空");
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

        public override SetUserPasswordResponseBody ExecuteCore()
        {
            SetUserPasswordResponseBody res = new SetUserPasswordResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                IQueryable<t_user> query = db.t_user;
                IQueryable<t_user_code> codeQuery = db.t_user_code.OrderByDescending(c => c.id).Where(c => c.code_type == CodeType.Register);

                if (!string.IsNullOrEmpty(this.request.Mobile))
                {
                    query = query.Where(u => u.mobile == this.request.Mobile);
                    codeQuery = codeQuery.Where(u => u.user_account == this.request.Mobile);
                }
                else
                {
                    query = query.Where(u => u.email == this.request.Email);
                    codeQuery = codeQuery.Where(u => u.user_account == this.request.Email);
                }

                t_user user = query.FirstOrDefault();
                if (user == null) throw new Exception("未注册的帐号不能找回密码!");

                t_user_code userCode = codeQuery.FirstOrDefault();
                if (userCode == null) throw new Exception("注册码不存在");
                if (userCode.expire_time <= DateTime.Now) throw new Exception("注册码已过期");
                if (userCode.code != this.request.CheckCode) throw new Exception("注册码验证错误");

                //添加用户成功后,设置注册码失效
                userCode.expire_time = userCode.expire_time.AddDays(-1);
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
