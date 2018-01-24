using HWL.Entity;
using HWL.Resx.Models;
using System.Web.Http;

namespace HWL.Resx.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public ResponseResult Index()
        {
            return new ResponseResult() { Status = ResultStatus.Failed, Message = "没有权限访问" };
        }
    }
}
