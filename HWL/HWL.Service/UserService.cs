
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

        public static Response<SetUserPosResponseBody> SetUserPos(Request<SetUserPosRequestBody> request)
        {
            var context = new ServiceContext<SetUserPosRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserPos(r.Body).Execute();
            });
        }

        public static Response<GetFriendsResponseBody> GetFriends(Request<GetFriendsRequestBody> request)
        {
            var context = new ServiceContext<GetFriendsRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetFriends(r.Body).Execute();
            });
        }

        public static Response<AddFriendResponseBody> AddFriend(Request<AddFriendRequestBody> request)
        {
            var context = new ServiceContext<AddFriendRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddFriend(r.Body).Execute();
            });
        }

        public static Response<SetFriendRemarkResponseBody> SetFriendRemark(Request<SetFriendRemarkRequestBody> request)
        {
            var context = new ServiceContext<SetFriendRemarkRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetFriendRemark(r.Body).Execute();
            });
        }

        public static Response<SetUserInfoResponseBody> SetUserInfo(Request<SetUserInfoRequestBody> request)
        {
            var context = new ServiceContext<SetUserInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserInfo(r.Body).Execute();
            });
        }

        public static Response<GetUserDetailsResponseBody> GetUserDetails(Request<GetUserDetailsRequestBody> request)
        {
            var context = new ServiceContext<GetUserDetailsRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetUserDetails(r.Body).Execute();
            });
        }
    }
}
