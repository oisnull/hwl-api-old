using HWL.MQGroupDistribute.message;
using System.Web.Mvc;

namespace HWL.Test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int u = 1)
        {

            GroupActionUnit.AddGroupUser();
            MQGroupMessageUnit.AddGroupMessage();

            return Content(Service.User.UserUtility.BuildToken(u));
        }
    }
}