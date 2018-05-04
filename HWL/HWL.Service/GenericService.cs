using GMSF;
using GMSF.Model;
using HWL.Service.Generic.Body;
using HWL.Service.Generic.Service;
using System;

namespace HWL.Service
{
    public class GenericService
    {
        public static Response<SendEmailResponseBody> SendEmail(Request<SendEmailRequestBody> request)
        {
            var context = new ServiceContext<SendEmailRequestBody>(request, new RequestValidate(false, false));
            return ContextProcessor.Execute(context, r =>
            {
                return new SendEmail(r.Body).Execute();
            });
        }
        public static Response<SendSMSResponseBody> SendSMS(Request<SendSMSRequestBody> request)
        {
            var context = new ServiceContext<SendSMSRequestBody>(request, new RequestValidate(false, false));
            return ContextProcessor.Execute(context, r =>
            {
                return new SendSMS(r.Body).Execute();
            });
        }
        public static Response<CheckVersionResponseBody> CheckVersion(Request<CheckVersionRequestBody> request)
        {
            var context = new ServiceContext<CheckVersionRequestBody>(request, new RequestValidate(false, false));
            return ContextProcessor.Execute(context, r =>
            {
                return new CheckVersion(r.Body).Execute();
            });
        }

        //public static Response CheckVersion(Request request)
        //{
        //    if (request == null)
        //    {
        //        throw new ArgumentNullException("request");
        //    }

        //    return ContextProcessor<General.CheckVersion.Response>.Execute(CommonService.GetCurrentContext(request, false), r =>
        //    {
        //        var requestBody = request.Convert<General.CheckVersion.Request>();
        //        return new General.CheckVersion.Service(requestBody).Execute();
        //    });
        //}

        //public static Response UpImage(Request request)
        //{
        //    if (request == null)
        //    {
        //        throw new ArgumentNullException("request");
        //    }

        //    return ContextProcessor<General.UpImage.Response>.Execute(CommonService.GetCurrentContext(request, false), r =>
        //    {
        //        var requestBody = request.Convert<General.UpImage.Request>();
        //        return new General.UpImage.Service(requestBody).Execute();
        //    });
        //}
    }
}
