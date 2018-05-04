using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Manage.Controllers
{
    public class MainController : BaseController
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View(base.currentAdmin);
        }
       
        public ActionResult Default()
        {
            return View();
        }

    }
}