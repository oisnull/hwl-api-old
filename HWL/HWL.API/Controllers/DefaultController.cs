using GMSF.Model;
using HWL.Service;
using HWL.Service.Generic.Body;
using HWL.Service.Group.Body;
using HWL.Service.Near.Body;
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
            //log.WriterLog(Newtonsoft.Json.JsonConvert.SerializeObject(request));
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
        [Description("删除好友")]
        public Response<DeleteFriendResponseBody> DeleteFriend(Request<DeleteFriendRequestBody> request)
        {
            return UserService.DeleteFriend(request);
        }

        [HttpPost]
        [Description("设置好友备注")]
        public Response<SetFriendRemarkResponseBody> SetFriendRemark(Request<SetFriendRemarkRequestBody> request)
        {
            return UserService.SetFriendRemark(request);
        }

        [HttpPost]
        [Description("获取用户详细信息")]
        public Response<GetUserDetailsResponseBody> GetUserDetails(Request<GetUserDetailsRequestBody> request)
        {
            return UserService.GetUserDetails(request);
        }

        [HttpPost]
        [Description("设置用户头像")]
        public Response<SetUserInfoResponseBody> SetUserHeadImage(Request<SetUserHeadImageRequestBody> request)
        {
            return UserService.SetUserHeadImage(request);
        }

        [HttpPost]
        [Description("设置用户名称")]
        public Response<SetUserInfoResponseBody> SetUserName(Request<SetUserNameRequestBody> request)
        {
            return UserService.SetUserName(request);
        }

        [HttpPost]
        [Description("设置用户描述")]
        public Response<SetUserInfoResponseBody> SetUserLifeNotes(Request<SetUserLifeNotesRequestBody> request)
        {
            return UserService.SetUserLifeNotes(request);
        }

        [HttpPost]
        [Description("设置用户性别")]
        public Response<SetUserInfoResponseBody> SetUserSex(Request<SetUserSexRequestBody> request)
        {
            return UserService.SetUserSex(request);
        }


        [HttpPost]
        [Description("设置用户标识")]
        public Response<SetUserInfoResponseBody> SetUserSymbol(Request<SetUserSymbolRequestBody> request)
        {
            return UserService.SetUserSymbol(request);
        }

        [HttpPost]
        [Description("查找用户信息")]
        public Response<SearchUserResponseBody> SearchUser(Request<SearchUserRequestBody> request)
        {
            return UserService.SearchUser(request);
        }

        [HttpPost]
        [Description("获取组用户列表")]
        public Response<GroupUsersResponseBody> GroupUsers(Request<GroupUsersRequestBody> request)
        {
            return GroupService.GroupUsers(request);
        }

        [HttpPost]
        [Description("发布附近圈子信息")]
        public Response<AddNearCircleInfoResponseBody> AddNearCircleInfo(Request<AddNearCircleInfoRequestBody> request)
        {
            return NearService.AddNearCircleInfo(request);
        }

        [HttpPost]
        [Description("获取附近圈子信息列表")]
        public Response<GetNearCircleInfosResponseBody> GetNearCircleInfos(Request<GetNearCircleInfosRequestBody> request)
        {
            return NearService.GetNearCircleInfos(request);
        }

        [HttpPost]
        [Description("获取附近圈子详细信息")]
        public Response<GetNearCircleDetailResponseBody> GetNearCircleDetail(Request<GetNearCircleDetailRequestBody> request)
        {
            return NearService.GetNearCircleDetail(request);
        }

        [HttpPost]
        [Description("附近圈子信息点赞设置")]
        public Response<SetNearLikeInfoResponseBody> SetNearLikeInfo(Request<SetNearLikeInfoRequestBody> request)
        {
            return NearService.SetNearLikeInfo(request);
        }

        [HttpPost]
        [Description("添加附近圈子信息评论,回复评论")]
        public Response<AddNearCommentResponseBody> AddNearComment(Request<AddNearCommentRequestBody> request)
        {
            return NearService.AddNearComment(request);
        }

        [HttpPost]
        [Description("获取附近圈子信息评论,回复评论列表")]
        public Response<GetNearCommentsResponseBody> GetNearComments(Request<GetNearCommentsRequestBody> request)
        {
            return NearService.GetNearComments(request);
        }

        [HttpPost]
        [Description("删除附近圈子信息评论,回复评论")]
        public Response<DeleteNearCommentResponseBody> DeleteNearComment(Request<DeleteNearCommentRequestBody> request)
        {
            return NearService.DeleteNearComment(request);
        }
    }
}
