using HtmlAgilityPack;
using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class GameAdminController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Admin/GameAdmin
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
            var data = db.Games.Find(id);
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
                    _FileName = sub11 + sub21 + "games" + file.FileName;
                    file.SaveAs(Server.MapPath("/img/" + _FileName));
                    return "/img/" + _FileName;
                }
            }
            return "";
        }
        [HttpGet]
        public JsonResult DataTable(int page, string search)
        {
            var data = db.Games.Select(a => new
            {
                a.Id,
                a.Name,
                a.CreateDate,
                a.CreateBy,
                a.ModifyDate,
                a.ModifyBy,
                a.Status,
                a.Hot
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
        [HttpPost,ValidateInput(false)]
        public JsonResult Adds(Game game)
        {
            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                if (game.Hot == null)
                {
                    game.Hot = false;
                }
                game.Des = documentHTML(game.Des, "games");
                game.CreateDate = DateTime.Now;
                game.ModifyDate = DateTime.Now;
                game.CreateBy = user.Name;
                game.ModifyBy = user.Name;
                game.Status = true;
                db.Games.Add(game);
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
        public JsonResult Edits(Game game)
        {
            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var data = db.Games.Find(game.Id);
                if (game.Hot == null)
                {
                    game.Hot = false;
                }
                data.Hot = game.Hot;
                data.PointReview = game.PointReview;
                data.Name = game.Name;
                data.Slug = game.Slug;
                data.Title = game.Title;
                data.Des = documentHTML(game.Des, "games");
                data.Image = game.Image;
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
                var data = db.Games.Find(id);
                db.Games.Remove(data);
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
        public string documentHTML(string content, string name)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var srcList = doc.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue("src", null))
                .Where(s => !String.IsNullOrEmpty(s))
                .ToList();
            for (int i = 0; i < srcList.Count(); i++)
            {
                string imageData = srcList[i];
                if (imageData.Contains("data:image"))
                {
                    string base64Data = imageData.Split(',')[1];
                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                        img.Save(Server.MapPath("/img/" + name + i) + ".png");
                    }
                    content = content.Replace(imageData, "/img/" + name + i + ".png");
                }
            }
            return content;
        }
    }
}