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
            return View(db.Sliders.Where(x => x.TypeSlider1.NameType == "Banner trang chủ").ToList());
        }
        public ActionResult Information()
        {
            return View();
        }
        public ActionResult AccountInformation()
        {
            return View();
        }
        public ActionResult ChangePass()
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
                var account = (Account)Session["account"];

                if(account != null)
                {
                    var user = db.Accounts.Find(account.AccountID);
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
        public JsonResult Game()
        {
            try
            {
                var data = db.Games.Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Des,
                    a.Image,
                    a.Title,
                    a.ModifyDate
                }).OrderByDescending(x=>x.ModifyDate).Take(3);
                return Json(new { code = 200, data }, JsonRequestBehavior.AllowGet);
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