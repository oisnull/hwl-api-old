using HWL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int u = 1)
        {
            return Content(Service.User.UserUtility.BuildToken(u));
        }


    }
}