using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class SetUserHeadImage : GMSF.ServiceHandler<SetUserHeadImageRequestBody, SetUserInfoResponseBody>
    {
        public SetUserHeadImage(SetUserHeadImageRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }

            if (string.IsNullOrEmpty(this.request.HeadImageUrl))
            {
                throw new Exception("头像地址不能为空");
            }
        }

        public override SetUserInfoResponseBody ExecuteCore()
        {
            SetUserInfoResponseBody res = new SetUserInfoResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                var user = db.t_user.Where(u => u.id == this.request.UserId).FirstOrDefault();
                if (user == null) throw new Exception("用户不存在");

                user.head_image = this.request.HeadImageUrl;

                db.SaveChanges();
                res.Status = ResultStatus.Success;
                res.Name = user.name;
                res.Symbol = user.symbol;
                res.HeadImage = user.head_image;
            }

            return res;
        }
    }
}
