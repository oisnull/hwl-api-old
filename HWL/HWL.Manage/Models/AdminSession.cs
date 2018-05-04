using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWL.Manage
{
    public class AdminSession
    {
        #region Session用户操作
        public static Admin Get()
        {
            Admin currentAdmin = HttpContext.Current.Session["SystemAdmin"] as Admin;
            return currentAdmin;
        }

        public static void Set(Admin a)
        {
            if (a != null)
            {
                HttpContext.Current.Session["SystemAdmin"] = a;
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Remove("SystemAdmin");
        }
        #endregion
    }
}