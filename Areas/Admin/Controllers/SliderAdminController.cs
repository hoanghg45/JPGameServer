using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class SliderAdminController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Admin/Slider
        public ActionResult Index(int id)
        {
            var data = db.TypeSliders.Find(id);
            return View(data);
        }
        public ActionResult Add(int id)
        {
            var data = db.TypeSliders.Find(id);
            return View(data);
        }
        public ActionResult Edit(int id)
        {
            var data = db.Sliders.Find(id);
            return View(data);
        }
        public string UploadImage(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var now = DateTime.Now.ToString().Trim();
                    var index1 = now.IndexOf(" ");
                    var sub1 = now.Substring(0, index1);
                    var sub11 = sub1.Replace("/", "");
                    var index2 = now.IndexOf(" ", index1 + 1);
                    var sub2 = now.Substring(index1 + 1);
                    var sub21 = sub2.Replace(":", "");
                    string _FileName = "";
                    int index = file.FileName.IndexOf('.');
                    _FileName = sub11 + sub21 + "slider" + file.FileName;
                    file.SaveAs(Server.MapPath("/img/" + _FileName));
                    return "/img/" + _FileName;
                }
            }
            return "";
        }
        [HttpGet]
        public JsonResult DataTable(int page, string search,int id)
        {
            var data = db.Sliders.Where(x=>x.TypeSlider==id).Select(a => new
            {
                a.Id,
                a.Name,
                a.Des,
                a.Image,
                a.CreateDate,
                a.CreateBy,
                a.ModifyDate,
                a.ModifyBy,
                a.Status
            }).Where(x => x.Name.Contains(search) || x.Name.ToLower().Contains(search));


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
        [HttpPost, ValidateInput(false)]
        public JsonResult Adds(Slider slider)
        {
          
            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                slider.CreateDate = DateTime.Now;
                slider.ModifyDate = DateTime.Now;
                slider.CreateBy = user.Name;
                slider.ModifyBy = user.Name;
                slider.Status = true;
                db.Sliders.Add(slider);
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
        [HttpPost, ValidateInput(false)]
        public JsonResult Edits(Slider slider)
        {
            
            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var data = db.Sliders.Find(slider.Id);
                data.Name = slider.Name;
                data.Image = slider.Image;
                data.Des = slider.Des;
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
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var data = db.Sliders.Find(id);
                db.Sliders.Remove(data);
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        ex.Entries.Single().Reload();
                    }
                } while (saveFailed);
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