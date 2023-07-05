using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JPGame.Areas.Security
{
    public class SessionCheck: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            HttpContextBase httpContext = filterContext.HttpContext;

           
            if (session != null && session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                {"Area", "Admin" },
                                { "Controller", "LoginAdmin" },
                                { "Action", "Index" }
                                });
            }
        }
    }
}