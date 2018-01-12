
using GMSF;
using GMSF.Model;
using HWL.Service.User.Body;
using HWL.Service.User.Service;

namespace HWL.Service
{
    public class UserService
    {
        public static Response<UserLoginResponseBody> UserLogin(Request<UserLoginRequestBody> request)
        {
            var context = new ServiceContext<UserLoginRequestBody>(request, new RequestValidate(false, false));
            return ContextProcessor.Execute(context, r =>
            {
                return new UserLogin(r.Body).Execute();
            });
        }

        public static Response<UserRegisterResponseBody> UserRegister(Request<UserRegisterRequestBody> request)
        {
            var context = new ServiceContext<UserRegisterRequestBody>(request, new RequestValidate(false, false));
            return ContextProcessor.Execute(context, r =>
            {
                return new UserRegister(r.Body).Execute();
            });
        }

        //public static Response<SearchUserResponseBody> SearchUser(Request<SearchUserRequestBody> request)
        //{
        //    var context = new ServiceContext<SearchUserRequestBody>(request, new RequestValidate());
        //    return ContextProcessor.Execute(context, r =>
        //    {
        //        return new SearchUser(r.Body).Execute();
        //    });
        //}
    }
}
