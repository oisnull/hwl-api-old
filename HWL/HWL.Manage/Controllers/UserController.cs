using HWL.Manage.Service;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Manage.Controllers
{
    public class UserController : BaseController
    {

        private UserService userService = new UserService();

        // GET: User
        public ActionResult List(int page = 0)
        {
            int pageCount = 20;
            int recordCount = 0;

            ViewBag.UseList = userService.GetUserList(page, pageCount);
            ViewBag.PageHtml = CommonCs.GetPageHtmlStr(recordCount, pageCount, page, 8, "/User/List/", "");
            return View();
        }
    }
}