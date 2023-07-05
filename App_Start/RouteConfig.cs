using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JPGame
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
name: "Information",
url: "thong-tin-nguoi-dung",
defaults: new { controller = "Home", action = "Information", id = UrlParameter.Optional }
);
            routes.MapRoute(
   name: "SignIn",
   url: "dang-nhap",
   defaults: new { controller = "Login", action = "SignIn", id = UrlParameter.Optional }
);
            routes.MapRoute(
               name: "SignUp",
               url: "dang-ki",
               defaults: new { controller = "Login", action = "SignUp", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "home",
           url: "trang-chu",
           defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
       );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
