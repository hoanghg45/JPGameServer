using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Hosting;
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
                a.IP,
                a.Name,
                a.CreateDate,
                a.CreateBy,
                a.ModifyDate,
                a.ModifyBy,
                a.Status,
            });


            //Xử lí phân trang
            var z = data.ToList();
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
                s.Status = true;
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
        //[HttpPost]
        //public JsonResult Upload(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
        //        {
        //            string currentDirectory = HostingEnvironment.MapPath("~");
        //            string fileName = file.FileName;
        //            string fileContentType = file.ContentType;
        //            byte[] fileBytes = new byte[file.ContentLength];
        //            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
        //            using (var package = new ExcelPackage(file.InputStream))
        //            {
        //                ExcelWorksheet currentSheet = package.Workbook.Worksheets.First();
        //                var workSheet = currentSheet;
        //                var noOfCol = workSheet.Dimension.End.Column;
        //                var noOfRow = workSheet.Dimension.End.Row;
        //                ImportError[] importError = new ImportError[noOfRow];
        //                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
        //                {
        //                    try
        //                    {
        //                        if (workSheet.Cells[rowIterator, 1].Value != null)
        //                        {
        //                            var id = workSheet.Cells[rowIterator, 1].Value == null ? null : workSheet.Cells[rowIterator, 1].Value.ToString();
        //                            var name = workSheet.Cells[rowIterator, 2].Value == null ? null : workSheet.Cells[rowIterator, 2].Value.ToString();
        //                            if (name == null)
        //                            {
        //                                importError[rowIterator - 1] = new ImportError(rm.GetString("nonamesentered").ToString(), rowIterator);
        //                                continue;
        //                            }
        //                            if (id == null)
        //                            {
        //                                importError[rowIterator - 1] = new ImportError(rm.GetString("havenotenteredthecode").ToString(), rowIterator);
        //                                continue;
        //                            }
        //                            var checkColor = db.Colors.SingleOrDefault(x => x.Id == id);
        //                            if (checkColor == null)
        //                            {
        //                                var user = (User)Session["user"];
        //                                var color = new Color();
        //                                color.Id = id;
        //                                color.Name = name;
        //                                color.Status = true;
        //                                color.CreateDate = DateTime.Now;
        //                                color.CreateBy = user.Name;
        //                                color.ModifyDate = DateTime.Now;
        //                                color.ModifyBy = user.Name;
        //                                db.Colors.Add(color);
        //                                db.SaveChanges();
        //                            }
        //                            else
        //                            {
        //                                importError[rowIterator - 1] = new ImportError(rm.GetString("alreadyhavecode").ToString(), rowIterator);
        //                                continue;
        //                            }
        //                        }
        //                    }
        //                    catch (DbEntityValidationException ex)
        //                    {
        //                        foreach (var error in ex.EntityValidationErrors)
        //                        {
        //                            foreach (var validationError in error.ValidationErrors)
        //                            {
        //                                Console.WriteLine("Lỗi xác thực: {0}", validationError.ErrorMessage);
        //                            }
        //                        }
        //                    }
        //                }
        //                importError = importError.Where(enem => enem != null).ToArray();
        //                return Json(new { code = 200, msg = importError.Select(i => i.ToString()).ToList() }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { code = 500, msg = rm.GetString("false").ToString() + " !!!" + e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}