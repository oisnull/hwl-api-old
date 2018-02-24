using HWL.Entity;
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

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户id不能为空");
            }
            if (this.request.Lat < 0 && this.request.Lon < 0)
            {
                throw new Exception("位置参数错误");
            }
        }

        public override GetNearCircleInfosResponseBody ExecuteCore()
        {
            GetNearCircleInfosResponseBody res = new GetNearCircleInfosResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                
            }

            return res;
        }
    }
}
