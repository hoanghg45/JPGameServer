using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        readonly DBEntities db = new DBEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            
            return View();
        }

        //public JsonResult CheckLogin()
        //{

        //}
    }
}