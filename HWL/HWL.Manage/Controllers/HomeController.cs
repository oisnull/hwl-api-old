using HWL.Entity.Extends;
using HWL.Manage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Manage.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginDo()
        {
            string name = Request.Params["name"];
            string pwd = Request.Params["pwd"];

            AdminService service = new AdminService();
            Admin result = service.Login(name, pwd);


            if (result.LoginStatus != AdminLoginStatus.Success)
            {
                return Json(new { state = -1, error = "用户名不存在或者密码错误" });
            }
            else
            {
                AdminSession.Set(result);
                return Json(new { state = 1, gto = "/Main/Index" });
            }
        }

        public ActionResult Logout()
        {
            AdminSession.Clear();
            return Redirect("/Home/Login");
        }
	}
}