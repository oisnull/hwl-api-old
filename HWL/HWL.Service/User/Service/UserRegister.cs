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
    public class UserRegister : ServiceHandler<UserRegisterRequestBody, UserRegisterResponseBody>
    {
        public UserRegister(UserRegisterRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (string.IsNullOrEmpty(this.request.Email) && string.IsNullOrEmpty(this.request.Mobile))
            {
                throw new Exception("手机或者邮箱不能为空");
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
            if (string.IsNullOrEmpty(this.request.CheckCode))
            {
                throw new Exception("注册码不能为空");
            }
        }

        public override UserRegisterResponseBody ExecuteCore()
        {
            UserRegisterResponseBody res = new UserRegisterResponseBody();

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
                if (user != null) throw new Exception("该帐号已经被注册");

                if (this.request.CheckCode != "888888")
                {
                    t_user_code userCode = codeQuery.FirstOrDefault();
                    if (userCode == null) throw new Exception("注册码不存在");
                    if (userCode.expire_time <= DateTime.Now) throw new Exception("注册码已过期");
                    if (userCode.code != this.request.CheckCode) throw new Exception("注册码验证错误");

                    //添加用户成功后,设置注册码失效
                    userCode.expire_time = userCode.expire_time.AddDays(-1);
                }

                //添加用户到数据库
                t_user model = new t_user()
                {
                    id = 0,
                    email = this.request.Email ?? " ",
                    mobile = this.request.Mobile ?? " ",
                    password = this.request.PasswordOK,

                    status = UserStatus.Normal,
                    sex = UserSex.Unknow,
                    register_date = DateTime.Now,
                    update_date = DateTime.Now,
                    name = RandomText.GetNum(),
                    head_image = ConfigManager.UserDefaultHeadImage,
                    circle_back_image = ConfigManager.UserCircleBackImage,
                };
                db.t_user.Add(model);
                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }

            return res;
        }


        ////添加注册提示消息记录
        //private void AddRegisterMsgRecord(HWLEntities db, int userId)
        //{
        //    if (userId <= 0) return;

        //    var registerNoticeModel = db.t_notice.Where(n => n.notice_type == NoticeType.Register).OrderByDescending(n => n.id).FirstOrDefault();
        //    if (registerNoticeModel != null)
        //    {
        //        t_msg_record record = new t_msg_record()
        //        {
        //            id = 0,

        //            title = "HWL官方",
        //            first_msg_content = registerNoticeModel.title,
        //            my_user_id = userId,
        //            from_user_id = 0,
        //            msg_record_type = MsgRecordType.ServiceNO,
        //            msg_count = 1,
        //            first_msg_id = 0,
        //            record_time = DateTime.Now,
        //            create_time = DateTime.Now,
        //        };
        //        db.t_msg_record.Add(record);
        //        db.SaveChanges();
        //    }
        //}
    }
}
