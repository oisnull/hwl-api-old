using HWL.Entity;
using HWL.Resx.Models;
using System.Web.Http;

namespace HWL.Resx.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public string Index()
        {
            return "没有权限访问";
        }
    }
}
