using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Controllers
{
    public class HomeController : Controller
    {
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Information()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public JsonResult CheckSession()
        {
            try
            {
                var user = (Account)Session["account"];
                if(user!= null)
                {
                    return Json(new { code = 200, user = user}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult LogOut()
        {
            try
            {
                Session["account"] = null;
                return Json(new { code = 200,url="/trang-chu/" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}