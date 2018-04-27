using GMSF;
using GMSF.Model;
using HWL.Service.Circle.Body;
using HWL.Service.Circle.Service;

namespace HWL.Service
{
    public class CircleService
    {
        public static Response<AddCircleInfoResponseBody> AddCircleInfo(Request<AddCircleInfoRequestBody> request)
        {
            var context = new ServiceContext<AddCircleInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddCircleInfo(r.Body).Execute();
            });
        }
        public static Response<AddCommentInfoResponseBody> AddCommentInfo(Request<AddCommentInfoRequestBody> request)
        {
            var context = new ServiceContext<AddCommentInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new AddCommentInfo(r.Body).Execute();
            });
        }
        public static Response<DeleteCircleInfoResponseBody> DeleteCircleInfo(Request<DeleteCircleInfoRequestBody> request)
        {
            var context = new ServiceContext<DeleteCircleInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteCircleInfo(r.Body).Execute();
            });
        }
        public static Response<DeleteCommentInfoResponseBody> DeleteCommentInfo(Request<DeleteCommentInfoRequestBody> request)
        {
            var context = new ServiceContext<DeleteCommentInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new DeleteCommentInfo(r.Body).Execute();
            });
        }
        public static Response<GetCircleDetailResponseBody> GetCircleDetail(Request<GetCircleDetailRequestBody> request)
        {
            var context = new ServiceContext<GetCircleDetailRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetCircleDetail(r.Body).Execute();
            });
        }
        public static Response<GetCircleInfosResponseBody> GetCircleInfos(Request<GetCircleInfosRequestBody> request)
        {
            var context = new ServiceContext<GetCircleInfosRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetCircleInfos(r.Body).Execute();
            });
        }
        public static Response<GetUserCircleInfosResponseBody> GetUserCircleInfos(Request<GetUserCircleInfosRequestBody> request)
        {
            var context = new ServiceContext<GetUserCircleInfosRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetUserCircleInfos(r.Body).Execute();
            });
        }
        public static Response<SetLikeInfoResponseBody> SetLikeInfo(Request<SetLikeInfoRequestBody> request)
        {
            var context = new ServiceContext<SetLikeInfoRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new SetLikeInfo(r.Body).Execute();
            });
        }
        public static Response<GetCircleCommentsResponseBody> GetCircleComments(Request<GetCircleCommentsRequestBody> request)
        {
            var context = new ServiceContext<GetCircleCommentsRequestBody>(request, new RequestValidate());
            return ContextProcessor.Execute(context, r =>
            {
                return new GetCircleComments(r.Body).Execute();
            });
        }
    }
}
