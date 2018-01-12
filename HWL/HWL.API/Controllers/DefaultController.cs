using System.ComponentModel;
using System.Web.Http;

namespace HWL.API.Controllers
{
    [Route("api/{action}")]
    public class DefaultController : ApiController
    {
        //[HttpPost]
        //[Description("login")]
        //public Response<UserLoginResponseBody> UserLogin(Request<UserLoginRequestBody> request)
        //{
        //    return UserService.UserLogin(request);
        //}

        //[HttpPost]
        //[Description("search")]
        //public Response<SearchUserResponseBody> SearchUser(Request<SearchUserRequestBody> request)
        //{
        //    return UserService.SearchUser(request);
        //}
    }
}
