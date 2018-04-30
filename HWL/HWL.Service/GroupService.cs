using GMSF;
using GMSF.Model;
using HWL.Service.Group.Body;
using HWL.Service.Group.Service;
using System;

namespace HWL.Service
{
    public class GroupService
    {
        public static Response<GroupUsersResponseBody> GroupUsers(Request<GroupUsersRequestBody> request)
        {
            var context = new ServiceContext<GroupUsersRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GroupUsers(r.Body).Execute();
            });
        }

        public static Response<AddGroupResponseBody> AddGroup(Request<AddGroupRequestBody> request)
        {
            var context = new ServiceContext<AddGroupRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddGroup(r.Body).Execute();
            });
        }

        public static Response<AddGroupUsersResponseBody> AddGroupUsers(Request<AddGroupUsersRequestBody> request)
        {
            var context = new ServiceContext<AddGroupUsersRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddGroupUsers(r.Body).Execute();
            });
        }

        public static Response<DeleteGroupResponseBody> DeleteGroup(Request<DeleteGroupRequestBody> request)
        {
            var context = new ServiceContext<DeleteGroupRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteGroup(r.Body).Execute();
            });
        }

        public static Response<DeleteGroupUserResponseBody> DeleteGroupUser(Request<DeleteGroupUserRequestBody> request)
        {
            var context = new ServiceContext<DeleteGroupUserRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteGroupUser(r.Body).Execute();
            });
        }
    }
}
