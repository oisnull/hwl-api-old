using HWL.Entity;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class SetUserSymbol : GMSF.ServiceHandler<SetUserSymbolRequestBody, SetUserInfoResponseBody>
    {
        public SetUserSymbol(SetUserSymbolRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户参数错误");
            }

            if (string.IsNullOrEmpty(this.request.Symbol))
            {
                throw new Exception("用户标识不能为空");
            }
        }

        public override SetUserInfoResponseBody ExecuteCore()
        {
            SetUserInfoResponseBody res = new SetUserInfoResponseBody() { Status = ResultStatus.Failed };

            using (HWLEntities db = new HWLEntities())
            {
                var user = db.t_user.Where(u => u.id == this.request.UserId).FirstOrDefault();
                if (user == null) throw new Exception("用户不存在");
                if (!string.IsNullOrEmpty(user.symbol)) throw new Exception("添加后不能修改标识");//已经添加过的就不能修改了
                else
                {
                    var uid = db.t_user.Where(u => u.symbol == this.request.Symbol).Select(u => u.id).FirstOrDefault();
                    if (uid > 0) throw new Exception("此标识已经存在,请修改");
                }

                user.symbol = this.request.Symbol;

                db.SaveChanges();
                res.Status = ResultStatus.Success;
            }

            return res;
        }
    }
}
