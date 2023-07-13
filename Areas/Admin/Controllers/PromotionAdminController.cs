using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{   [SessionCheck]

    public class PromotionAdminController : Controller
    {
        
        // GET: Admin/PromotionAdmin
        DBEntities db = new DBEntities();
        // GET: Admin/ModulesAdmin
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult CreatePromotion()
        {
            return View();
        }
      

        [HttpPost, ValidateInput(false)]
        public JsonResult CreatePromotion(FormCollection collection)
        {
            string UserID = Session["UserID"].ToString();
           
            try
            {
                string saleTime = collection["SaleTime"].Trim();
                var SaleTime = GetTwoDate(saleTime);
                var Rate = Double.Parse(collection["Rate"]);
                var promotion = new Promotion {
                    
                    Content = collection["Content"].Trim(),
                    Title = collection["Title"].Trim(),
                    From = SaleTime.Item1,
                    To = SaleTime.Item2,
                    CreateBy = UserID,
                    CreateDate = DateTime.Now,
                    Rate = (Rate /100),
                    Status = true,
                    Description= collection["Description"]
                    
                };

                db.Promotions.Add(promotion);
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
        public (Nullable<DateTime>, Nullable<DateTime>) GetTwoDate(string input)
        {
            DateTime Start;
            DateTime End;
            // Sử dụng phương thức String.Substring()
            int separatorIndex = input.IndexOf(" - ");
            if (separatorIndex != -1)
            {
                string startDateTime = input.Substring(0, separatorIndex).Trim();
                string endDateTime = input.Substring(separatorIndex + 3).Trim();
                string format = "dd/MM/yyyy hh:mm tt";


                // Sử dụng DateTime.ParseExact()
                Start = DateTime.ParseExact(startDateTime, format, CultureInfo.InvariantCulture);
                End = DateTime.ParseExact(endDateTime, format, CultureInfo.InvariantCulture);
                return (Start, End);
            }
            return (null, null);
        }
        [HttpGet]
        public ActionResult EditPromotion(int id)
        {
           var promotion =  db.Promotions.Find(id);
            return View(promotion);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult EditPromotion(FormCollection collection)
        {
            string UserID = Session["UserID"].ToString().Trim();

            try
            {
               
                if (string.IsNullOrEmpty(collection["ID"]))
                {
                    return Json(
                  new
                  {
                      status = "error",
                      message = "Khuyến mãi không tồn tại, vui lòng thử lại!"

                  }
                  , JsonRequestBehavior.AllowGet
                    );
                }
                int id = Int16.Parse(collection["ID"]);
                if (!db.Promotions.Any(p => p.ID == id))
                {
                    return Json(
                 new
                 {
                     status = "error",
                     message = "Khuyến mãi không tồn tại, vui lòng thử lại!"

                 }
                 , JsonRequestBehavior.AllowGet
                   );
                }
                var promotion = db.Promotions.Find(Int16.Parse(collection["ID"]));
                string saleTime = collection["SaleTime"].Trim();
                var SaleTime = GetTwoDate(saleTime);
                var Rate = Double.Parse(collection["Rate"]);


                promotion.Content = collection["Content"].Trim();
                promotion.Title = collection["Title"].Trim();
                promotion.From = SaleTime.Item1;
                promotion.To = SaleTime.Item2;
                promotion.CreateBy = UserID;
                promotion.CreateDate = DateTime.Now;
                promotion.Rate = (Rate / 100);
                promotion.Status = true;
                promotion.Description = collection["Description"];
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

        [HttpGet]
        public JsonResult DataTable(int page = 0)
        {
            var data = db.Promotions.Select(a => new
            {
                a.ID,
                Title = a.Title.Trim(),
                Content = a.Content.Trim(),
                Rate = a.Rate*100,
                a.From,
                a.To,
                a.CreateDate,
                a.Status

            });


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


        [HttpGet]
        public JsonResult GetPromotion()
        {
            string UserID = Session["UserID"].ToString();

            if (string.IsNullOrEmpty(UserID))
            {
                return Json(
             new
             {
                 status = "error",
                 message = "Tài khoản của bạn không tồn tại vui lòng kiểm tra lại!"

             }
             , JsonRequestBehavior.AllowGet
             );
            }
            if (!db.Users.Any(u => u.UserID.Trim().Equals(UserID)))
            {
                return Json(
                new
                {
                    status = "error",
                    message = "Tài khoản của bạn không tồn tại vui lòng kiểm tra lại!"

                }
                , JsonRequestBehavior.AllowGet
                );

            }
            var module = db.Modules.Select(m => new
            {
                Address = m.Address.Trim(),
                Hotline = m.Hotline.Trim(),
                Email = m.Email.Trim(),
                m.AboutMe

            }).FirstOrDefault();




            return Json(
              new
              {
                  status = "success",
                  module

              }
              , JsonRequestBehavior.AllowGet
              );



        }
        [HttpPost]
        public JsonResult DeletePromotion(int id)
        {
            string UserID = Session["UserID"].ToString();

            try
            {
                if(id == 0 || !db.Promotions.Any(p => p.ID == id))
                {
                        return Json(
                  new
                  {
                      status = "error",
                      message = "Khuyến mãi không tồn tại, vui lòng thử lại!",

                  }
                  , JsonRequestBehavior.AllowGet
                  );
                }
                var promotion = db.Promotions.Find(id);
                db.Promotions.Remove(promotion);
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