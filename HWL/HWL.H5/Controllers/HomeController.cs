using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.H5.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShareApp()
        {
            return View();
        }

        public ActionResult QRShare()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}