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
    }
}
