using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class SetUserCircleBackImage : GMSF.ServiceHandler<SetUserCircleBackImageRequestBody, SetUserCircleBackImageResponseBody>
    {
        public SetUserCircleBackImage(SetUserCircleBackImageRequestBody request) : base(request)
        {
        }


        public override SetUserCircleBackImageResponseBody ExecuteCore()
        {
            SetUserCircleBackImageResponseBody res = new SetUserCircleBackImageResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                var user = db.t_user.Where(u => u.id == this.request.UserId).FirstOrDefault();
                if (user == null) throw new Exception("用户不存在");

                user.circle_back_image = this.request.CircleBackImageUrl;
                user.update_date = DateTime.Now;

                db.SaveChanges();
                res.Status = ResultStatus.Success;
                res.CircleBackImageUrl = user.circle_back_image;
                //res.Name = user.name;
                //res.Symbol = user.symbol;
                //res.HeadImage = user.head_image;
            }

            return res;
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }

            if (string.IsNullOrEmpty(this.request.CircleBackImageUrl))
            {
                throw new Exception("背景地址不能为空");
            }
        }
    }
}
