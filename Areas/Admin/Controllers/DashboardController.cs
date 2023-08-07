using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            string reader = Session["ReaderID"].ToString();
            MemberCardComponent MC = new MemberCardComponent();
            MC.RegisterNotification(reader);
            return View();
        }
    }
}