using HWL.H5.Models;
using HWL.Manage.Service;
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
            ViewBag.AppUrl = new AppService().GetAppLastVersionUrl();
            return View();
        }

        public ActionResult QRShare()
        {
            string url = "http://" + Request.Url.Authority + "/home/shareapp";
            QRCodeBuild.CreateQR(url);
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}