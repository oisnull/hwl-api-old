using GMSF.Model;
using HWL.Service;
using HWL.Service.Generic.Body;
using HWL.Service.User.Body;
using HWL.Tools;
using System.ComponentModel;
using System.Web.Http;

namespace HWL.API.Controllers
{
    [Route("api/{action}")]
    public class DefaultController : ApiController
    {
        LogAction log = new LogAction("api-" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt");

        [HttpPost]
        [Description("用户登陆")]
        public Response<UserLoginResponseBody> UserLogin(Request<UserLoginRequestBody> request)
        {
            log.WriterLog(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            return UserService.UserLogin(request);
        }

        [HttpPost]
        [Description("用户注册")]
        public Response<UserRegisterResponseBody> UserRegister(Request<UserRegisterRequestBody> request)
        {
            return UserService.UserRegister(request);
        }

        [HttpPost]
        [Description("发送邮件")]
        public Response<SendEmailResponseBody> SendEmail(Request<SendEmailRequestBody> request)
        {
            return GenericService.SendEmail(request);
        }

        //[HttpPost]
        //[Description("search")]
        //public Response<SearchUserResponseBody> SearchUser(Request<SearchUserRequestBody> request)
        //{
        //    return UserService.SearchUser(request);
        //}
    }
}
