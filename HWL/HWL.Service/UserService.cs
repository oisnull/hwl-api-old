
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

        public static Response<DeleteFriendResponseBody> DeleteFriend(Request<DeleteFriendRequestBody> request)
        {
            var context = new ServiceContext<DeleteFriendRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteFriend(r.Body).Execute();
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

        public static Response<SetUserInfoResponseBody> SetUserSymbol(Request<SetUserSymbolRequestBody> request)
        {
            var context = new ServiceContext<SetUserSymbolRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserSymbol(r.Body).Execute();
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
        public static Response<SetUserInfoResponseBody> SetUserLifeNotes(Request<SetUserLifeNotesRequestBody> request)
        {
            var context = new ServiceContext<SetUserLifeNotesRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserLifeNotes(r.Body).Execute();
            });
        }
        public static Response<SetUserInfoResponseBody> SetUserName(Request<SetUserNameRequestBody> request)
        {
            var context = new ServiceContext<SetUserNameRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserName(r.Body).Execute();
            });
        }
        public static Response<SetUserInfoResponseBody> SetUserHeadImage(Request<SetUserHeadImageRequestBody> request)
        {
            var context = new ServiceContext<SetUserHeadImageRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserHeadImage(r.Body).Execute();
            });
        }
        
        public static Response<SetUserInfoResponseBody> SetUserSex(Request<SetUserSexRequestBody> request)
        {
            var context = new ServiceContext<SetUserSexRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetUserSex(r.Body).Execute();
            });
        }
        public static Response<SearchUserResponseBody> SearchUser(Request<SearchUserRequestBody> request)
        {
            var context = new ServiceContext<SearchUserRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SearchUser(r.Body).Execute();
            });
        }
    }
}
