using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Manage.Controllers
{
    public class ResxController : BaseController
    {
        public JsonResult Upfile(string folder = "")
        {
            string error = "";
            var paths = UpfileHandler.Process(Request.Files, folder, out error);
            if (string.IsNullOrEmpty(error))
            {
                return Json(new { state = -1, error = error });
            }
            return Json(new { state = 1 });
        }
    }
}