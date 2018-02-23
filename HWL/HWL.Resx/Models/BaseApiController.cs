using GMSF.Model;
using HWL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HWL.Resx.Models
{
    public class BaseApiController : ApiController
    {
        //protected bool CheckToken(string token)
        //{
        //    RequestValidate requestValidate = new RequestValidate();
        //    return requestValidate.CheckToken(token);
        //}
        //protected Response<UpImageResponseBody> GetResult(string resultCode, string message, UpImageResponseBody body = null)
        //{
        //    Response<UpImageResponseBody> response = new Response<UpImageResponseBody>();
        //    response.Head = new GMSF.HeadDefine.ResponseHead()
        //    {
        //        ResultCode = resultCode,
        //        ResultMessage = message
        //    };
        //    response.Body = body;

        //    return response;
        //}

        /// <summary>
        /// 返回元祖《表示验证是否成功,验证成功后返回的userid》
        /// </summary>
        protected Tuple<bool, int> CheckToken(string token)
        {
            RequestValidate requestValidate = new RequestValidate();
            bool succ = requestValidate.CheckToken(token);
            int userid = requestValidate.GetCurrUserId();
            return new Tuple<bool, int>(succ, userid);
        }

        protected Response<ResxResult> GetResult(string resultCode, string message, ResxResult body = null)
        {
            Response<ResxResult> response = new Response<ResxResult>();
            response.Head = new GMSF.HeadDefine.ResponseHead()
            {
                ResultCode = resultCode,
                ResultMessage = message
            };
            response.Body = body;

            return response;
        }
    }
}