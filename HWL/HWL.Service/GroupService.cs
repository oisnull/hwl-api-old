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

        public static Response<SetGroupNameResponseBody> SetGroupName(Request<SetGroupNameRequestBody> request)
        {
            var context = new ServiceContext<SetGroupNameRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetGroupName(r.Body).Execute();
            });
        }

        public static Response<SetGroupNoteResponseBody> SetGroupNote(Request<SetGroupNoteRequestBody> request)
        {
            var context = new ServiceContext<SetGroupNoteRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetGroupNote(r.Body).Execute();
            });
        }

        public static Response<GetGroupsResponseBody> GetGroups(Request<GetGroupsRequestBody> request)
        {
            var context = new ServiceContext<GetGroupsRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetGroups(r.Body).Execute();
            });
        }
    }
}
