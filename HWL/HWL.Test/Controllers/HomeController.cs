using HWL.MQGroupDistribute.message;
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
        public ActionResult Index()
        {
            GroupActionUnit.AddGroupUser();
            MQGroupMessageUnit.AddGroupMessage();
            return View();
        }
    }
}