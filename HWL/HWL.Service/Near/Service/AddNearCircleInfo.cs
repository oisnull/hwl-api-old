using HWL.Service.Near.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Near.Service
{
    public class AddNearCircleInfo : GMSF.ServiceHandler<AddNearCircleInfoRequestBody, AddNearCircleInfoResponseBody>
    {
        public AddNearCircleInfo(AddNearCircleInfoRequestBody request) : base(request)
        {
        }

        public override AddNearCircleInfoResponseBody ExecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}
