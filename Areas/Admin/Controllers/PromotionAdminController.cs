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

        [HttpPost]
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
                    Status = true
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
                string format = "dd/MM/yyyy h:mm tt";


                // Sử dụng DateTime.ParseExact()
                Start = DateTime.ParseExact(startDateTime, format, CultureInfo.InvariantCulture);
                End = DateTime.ParseExact(endDateTime, format, CultureInfo.InvariantCulture);
                return (Start, End);
            }
            return (null, null);
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
    }
}