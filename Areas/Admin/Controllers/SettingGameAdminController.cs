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
    public class SettingGameAdminController : Controller
    {
        private DBEntities db = new DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            var data = db.SettingGames.Find(id);
            return View(data);
        }
        [HttpGet]
        public JsonResult DataTable(int page, string search)
        {
            var data = db.SettingGames.Select(a => new
            {
                a.Id,
                a.Name,
                a.CreateDate,
                a.CreateBy,
                a.ModifyDate,
                a.ModifyBy,
                a.Status,
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
        public JsonResult Adds(SettingGame settingGame)
        {

            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                if (db.SettingGames.Find(settingGame.Id) != null)
                {
                    return Json(new{status = "error",msg="Mã trò chơi đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                settingGame.CreateDate = DateTime.Now;
                settingGame.ModifyDate = DateTime.Now;
                settingGame.CreateBy = user.Name;
                settingGame.ModifyBy = user.Name;
                settingGame.Status = true;
                db.SettingGames.Add(settingGame);
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
        public JsonResult Edits(SettingGame settingGame)
        {
            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var s = db.SettingGames.Find(settingGame.Id);
                s.Name = settingGame.Name;
                s.Price = settingGame.Price;
                s.CreateBy = user.Name;
                s.ModifyBy = user.Name;
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
        public JsonResult Delete(string id)
        {
            try
            {
                var data = db.SettingGames.Find(id);
                db.SettingGames.Remove(data);
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