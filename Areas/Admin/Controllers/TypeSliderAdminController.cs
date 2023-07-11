using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class TypeSliderAdminController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Admin/TypeSliderAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            var data = db.TypeSliders.Find(id);
            return View(data);
        }
        [HttpGet]
        public JsonResult DataTable(int page,string search)
        {
            var data = db.TypeSliders.Select(a => new
            {
                a.Id,
               a.NameType,
               a.CreateDate,
               a.CreateBy,
               a.ModifyDate,
               a.ModifyBy,
               a.Status
            }).Where(x=>x.NameType.Contains(search)||x.NameType.ToLower().Contains(search));


            //Xử lí phân trang

            //Số dữ liệu trên 1 trang
            int pageSize = 10;
            page = (page > 0) ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalBill = data.Count();
            float totalNumsize = (totalBill / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            data = data.OrderByDescending(d => d.CreateDate).Skip(start).Take(pageSize);

            var fromto = PaginationExtension.FromTo(totalBill, (int)page, pageSize);

            int from = fromto.Item1;
            int to = fromto.Item2;
            return this.Json(
          new
          {
              data,
              pageCurrent = page,
              numSize,
              total = totalBill,
              size = pageSize,
              from,
              to

          }
          , JsonRequestBehavior.AllowGet
          );
        }
        [HttpPost]
        public JsonResult Adds(TypeSlider typeSlider)
        {
            string UserID = Session["UserID"].ToString();
            var user = db.Users.Find(UserID);
            try
            {
                typeSlider.CreateDate = DateTime.Now;
                typeSlider.ModifyDate = DateTime.Now;
                typeSlider.CreateBy = user.Name;
                typeSlider.ModifyBy = user.Name;
                typeSlider.Status = true;
                db.TypeSliders.Add(typeSlider);
                db.SaveChanges();
                return Json(
                new
                {
                    status = "success",


                }
                , JsonRequestBehavior.AllowGet
            );
            }
            catch (Exception ex)
            {
                return Json(
                new
                {
                    status = "error",
                    message = ex,

                }
                , JsonRequestBehavior.AllowGet
                );
            }

        }
        [HttpPost]
        public JsonResult Edits(TypeSlider typeSlider)
        {
            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var data = db.TypeSliders.Find(typeSlider.Id);
                data.NameType = typeSlider.NameType;
                data.ModifyDate = DateTime.Now;
                data.ModifyBy = user.Name;
                data.Status = true;
                db.SaveChanges();
                return Json(
                new
                {
                    status = "success",


                }
                , JsonRequestBehavior.AllowGet
            );
            }
            catch (Exception ex)
            {
                return Json(
                new
                {
                    status = "error",
                    message = ex,

                }
                , JsonRequestBehavior.AllowGet
                );
            }

        }
    }
}