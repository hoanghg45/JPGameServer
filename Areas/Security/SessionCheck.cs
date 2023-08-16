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
        public string[] roles { get; set; }
        DBEntities db = new DBEntities();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            

           
            if (session != null && session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                {"Area", "Admin" },
                                { "Controller", "LoginAdmin" },
                                { "Action", "Index" }
                                });
            }
            else
            {
                if (roles != null && roles.Count() > 0)
                {
                    var check = roles.Contains(db.Users.Find(session["UserID"].ToString()).Role.Trim());
                    if (!check)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                {"Area", "Admin" },
                                { "Controller", "Dashboard" },
                                { "Action", "Index" }
                                    });
                    }
                }
             
            }
        }
    }
}