using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class SetUserInfo : GMSF.ServiceHandler<SetUserInfoRequestBody, SetUserInfoResponseBody>
    {
        public SetUserInfo(SetUserInfoRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }

            if (string.IsNullOrEmpty(this.request.UserName))
            {
                throw new Exception("用户名称不能为空");
            }
        }

        public override SetUserInfoResponseBody ExecuteCore()
        {
            SetUserInfoResponseBody res = new SetUserInfoResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                var user = db.t_user.Where(u => u.id == this.request.UserId).FirstOrDefault();
                if (user == null) throw new Exception("用户不存在");

                user.name = this.request.UserName;
                user.head_image = this.request.HeadImage;
                user.sex = this.request.UserSex;
                user.life_notes = this.request.LifeNotes;
                if (this.request.PosIdList != null && this.request.PosIdList.Count > 0)
                {
                    user.register_country = 1;//this.request.PosIdList[0];
                    user.register_province = this.request.PosIdList[1];
                    user.register_city = this.request.PosIdList[2];
                    user.register_district = this.request.PosIdList[3];
                }

                db.SaveChanges();
                res.Status = ResultStatus.Success;

                ////重置redis缓存中的的用户基本信息
                //var t = new Task(() =>
                //{
                //    Redis.UserAction userAction = new Redis.UserAction();
                //    userAction.SetUserInfoExpire(this.request.UserId);
                //});
                //t.Start();
            }

            return res;
        }
    }
}
