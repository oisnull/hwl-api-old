using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class GetNearCircleInfos : GMSF.ServiceHandler<GetNearCircleInfosRequestBody, GetNearCircleInfosResponseBody>
    {
        public GetNearCircleInfos(GetNearCircleInfosRequestBody request) : base(request)
        {
        }

        public override GetNearCircleInfosResponseBody ExecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}
