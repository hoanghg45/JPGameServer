using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class AccountsAdminController : Controller
    {
       
        DBEntities db = new DBEntities();
        // GET: Admin/AccountsAdmin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DataTable(int page =0)
        {
            var data = db.Accounts.Select(a => new
            {
                a.AccountID,
                AccountName = a.AccountName.Trim(),
                FullName = a.FullName.Trim(),
                PhoneNumber = a.Phone.Trim(),
                DateOfBirth = a.DateOfBirth ?? null,
                Email = a.Email.Trim(),
                CreateAt = a.CreateDate,
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
            data = data.OrderByDescending(d => d.CreateAt).Skip(start).Take(pageSize);
            
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Create(Account formData)
        {
            try
            {
                if (formData.AccountName != null)
                {
                    var date = DateTime.Now;
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(formData.AccountName);
                    string base64String = Convert.ToBase64String(bytes);
                    formData.AccountID = base64String + date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
                }
                formData.CreateDate = DateTime.Now;
                formData.ModifyDate = DateTime.Now;
                formData.Password = BCrypt.Net.BCrypt.HashPassword("123456");
                db.Accounts.Add(formData);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Đã Có Tài Khoản " + formData.AccountName }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}