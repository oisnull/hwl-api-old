using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class GetNearCircleDetail : GMSF.ServiceHandler<GetNearCircleDetailRequestBody, GetNearCircleDetailResponseBody>
    {
        public GetNearCircleDetail(GetNearCircleDetailRequestBody request) : base(request)
        {
        }

        public override GetNearCircleDetailResponseBody ExecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}
