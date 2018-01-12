using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class UserLogin : GMSF.ServiceHandler<UserLoginRequestBody, UserLoginResponseBody>
    {
        public UserLogin(UserLoginRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
        }

        public override UserLoginResponseBody ExecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}
