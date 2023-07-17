using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Controllers
{
    public class PromotionController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Promotion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(string slug, int id)
        {

            return View(db.Promotions.Find(id));
        }
        [HttpGet]
        public JsonResult Promotion(int page, int pageSize)
        {
            try
            {
                var data = db.Promotions.Select(a => new
                {
                    a.ID,
                    a.Content,
                    a.Title,
                    a.Slug,
                    a.Image,
                    a.Description,
                    a.ModifyDate,
                    a.ModifyBy,
                }).OrderByDescending(x => x.ModifyDate).ToList();
                var pages = data.Count() % pageSize == 0 ? data.Count() / pageSize : data.Count() / pageSize + 1;
                data = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { code = 200, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}