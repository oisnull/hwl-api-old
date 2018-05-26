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
        public static Response<GetNearCircleInfosResponseBody> GetNearCircleInfos(Request<GetNearCircleInfosRequestBody> request)
        {
            var context = new ServiceContext<GetNearCircleInfosRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetNearCircleInfos(r.Body).Execute();
            });
        }
        public static Response<GetNearCircleDetailResponseBody> GetNearCircleDetail(Request<GetNearCircleDetailRequestBody> request)
        {
            var context = new ServiceContext<GetNearCircleDetailRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetNearCircleDetail(r.Body).Execute();
            });
        }

        public static Response<AddNearCommentResponseBody> AddNearComment(Request<AddNearCommentRequestBody> request)
        {
            var context = new ServiceContext<AddNearCommentRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddNearComment(r.Body).Execute();
            });
        }

        public static Response<SetNearLikeInfoResponseBody> SetNearLikeInfo(Request<SetNearLikeInfoRequestBody> request)
        {
            var context = new ServiceContext<SetNearLikeInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetNearLikeInfo(r.Body).Execute();
            });
        }

        public static Response<DeleteNearCommentResponseBody> DeleteNearComment(Request<DeleteNearCommentRequestBody> request)
        {
            var context = new ServiceContext<DeleteNearCommentRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteNearComment(r.Body).Execute();
            });
        }

        public static Response<GetNearCommentsResponseBody> GetNearComments(Request<GetNearCommentsRequestBody> request)
        {
            var context = new ServiceContext<GetNearCommentsRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetNearComments(r.Body).Execute();
            });
        }

        public static Response<GetNearLikesResponseBody> GetNearLikes(Request<GetNearLikesRequestBody> request)
        {
            var context = new ServiceContext<GetNearLikesRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetNearLikes(r.Body).Execute();
            });
        }

        public static Response<DeleteNearCircleInfoResponseBody> DeleteNearCircleInfo(Request<DeleteNearCircleInfoRequestBody> request)
        {
            var context = new ServiceContext<DeleteNearCircleInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteNearCircleInfo(r.Body).Execute();
            });
        }
    }
}
