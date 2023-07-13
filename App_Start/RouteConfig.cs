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
name: "Contact",
url: "lien-he",
defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Promotiondetail",
url: "chi-tiet-khuyen-mai/{meta}/{id}",
defaults: new { controller = "Promotion", action = "Detail", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Promotion",
url: "khuyen-mai",
defaults: new { controller = "Promotion", action = "Index", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Blogdetail",
url: "chi-tiet-tin-tuc/{meta}/{id}",
defaults: new { controller = "Blog", action = "Detail", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Blog",
url: "tin-tuc",
defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "gamedetail",
url: "chi-tiet-tro-choi/{meta}/{id}",
defaults: new { controller = "Game", action = "Detail", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "Game",
url: "tro-choi",
defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "ChangePass",
url: "doi-mat-khau",
defaults: new { controller = "Home", action = "ChangePass", id = UrlParameter.Optional }
);
            routes.MapRoute(
name: "AccountInformation",
url: "thong-tin-tai-khoan",
defaults: new { controller = "Home", action = "AccountInformation", id = UrlParameter.Optional }
);
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
