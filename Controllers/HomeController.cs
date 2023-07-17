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
        public ActionResult Error()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public JsonResult Module()
        {
            try
            {
                var module = db.Modules.Find(3);
                return Json(new {
                    code = 200, 
                    logo = module.Logo,
                    bannerGame = module.BannerGame,
                    bannerBlog = module.BannerBlog,
                    bannerPromotion = module.BannerPromotion,
                
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckSession()
        {
            try
            {
                var account = (Account)Session["account"];
                if (account != null)
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
                    a.Slug,
                    a.Name,
                    a.Des,
                    a.Image,
                    a.Title,
                    a.ModifyDate
                }).OrderByDescending(x=>x.ModifyDate).Take(3);
                var dataHot = db.Games.Where(x=>x.Hot==true).Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Slug,
                    a.Des,
                    a.PointReview,
                    a.Image,
                    a.Title,
                    a.ModifyDate
                }).OrderByDescending(x => x.ModifyDate).Take(4);
                return Json(new { code = 200, data, dataHot }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Promotion()
        {
            try
            {
                var data = db.Promotions.Select(a => new
                {
                    a.ID,
                    a.Slug,
                    a.Content,
                    a.Title,
                    a.Description,
                    a.ModifyDate,
                    a.ModifyBy,
                }).OrderByDescending(x => x.ModifyDate);
                return Json(new { code = 200, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Blog()
        {
            try
            {
                var data = db.Blogs.Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Des,
                    a.Image,
                    a.Title,
                    a.ModifyDate,
                    a.ModifyBy,
                }).OrderByDescending(x => x.ModifyDate).Take(2);
                var dataHot = db.Blogs.Where(x=>x.Hot==true).Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Des,
                    a.Image,
                    a.Title,
                    a.ModifyDate,
                    a.ModifyBy,
                }).OrderByDescending(x => x.ModifyDate).Take(3);
                return Json(new { code = 200, data, dataHot }, JsonRequestBehavior.AllowGet);
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