using GMSF;
using GMSF.Model;
using HWL.Service.Near.Body;
using HWL.Service.Near.Service;
using System;

namespace HWL.Service
{
    public class NearService
    {
        public static Response<AddNearCircleInfoResponseBody> AddNearCircleInfo(Request<AddNearCircleInfoRequestBody> request)
        {
            var context = new ServiceContext<AddNearCircleInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddNearCircleInfo(r.Body).Execute();
            });
        }
    }
}
