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

        [HttpPost]
        [Description("发送短信")]
        public Response<SendSMSResponseBody> SendSMS(Request<SendSMSRequestBody> request)
        {
            return GenericService.SendSMS(request);
        }

        [HttpPost]
        [Description("设置用户位置信息")]
        public Response<SetUserPosResponseBody> SetUserPos(Request<SetUserPosRequestBody> request)
        {
            return UserService.SetUserPos(request);
        }

        [HttpPost]
        [Description("获取好友列表")]
        public Response<GetFriendsResponseBody> GetFriends(Request<GetFriendsRequestBody> request)
        {
            return UserService.GetFriends(request);
        }

        [HttpPost]
        [Description("添加好友")]
        public Response<AddFriendResponseBody> AddFriend(Request<AddFriendRequestBody> request)
        {
            return UserService.AddFriend(request);
        }

        [HttpPost]
        [Description("设置好友备注")]
        public Response<SetFriendRemarkResponseBody> SetFriendRemark(Request<SetFriendRemarkRequestBody> request)
        {
            return UserService.SetFriendRemark(request);
        }

        [HttpPost]
        [Description("设置用户基本信息")]
        public Response<SetUserInfoResponseBody> SetUserInfo(Request<SetUserInfoRequestBody> request)
        {
            return UserService.SetUserInfo(request);
        }

        [HttpPost]
        [Description("获取用户详细信息")]
        public Response<GetUserDetailsResponseBody> GetUserDetails(Request<GetUserDetailsRequestBody> request)
        {
            return UserService.GetUserDetails(request);
        }
    }
}
