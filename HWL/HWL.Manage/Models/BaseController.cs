using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Manage
{
    public class BaseController : Controller
    {
        protected Admin currentAdmin { get; set; }

        public BaseController()
        {
            currentAdmin = AdminSession.Get();//从session中获取
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (currentAdmin == null)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    //filterContext.HttpContext.Response.StatusCode = 406;
                    //filterContext.Result = new JsonResult()
                    //{
                    //    Data = new { status = 406, error = "登录超时，请重新登录！" },
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};
                    Response.AppendHeader("sessionstatus", "timeout");
                    Response.Write("<script>TimeoutError();</script>");
                    Response.End();
                }
                else
                {
                    //filterContext.Result = new RedirectResult("/Home/Login?null");
                    filterContext.Result = RedirectToRoute("Default", new { Controller = "Home", Action = "Login" });
                }
            }
            else
                base.OnActionExecuting(filterContext);

        }

        //protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        //{
        //    if (currentAdmin == null)
        //    {
        //        if (!requestContext.HttpContext.Response.IsRequestBeingRedirected)
        //        {
        //            requestContext.HttpContext.Response.Write("<script>window.location='/Home/Login?null'</script>");
        //            requestContext.HttpContext.Response.End();
        //        }
        //        return null;
        //    }
        //    return base.BeginExecute(requestContext, callback, state);
        //}
    }
}