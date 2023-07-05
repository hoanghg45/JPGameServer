using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        readonly DBEntities db = new DBEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult CheckLogin(FormCollection collection)
        {

            var username = collection["username"];
            var password = collection["password"];
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(
             new
             {
                 status = "error",
                 message = "Vui lòng nhập đầy đủ thông tin!"

             }
             , JsonRequestBehavior.AllowGet
             );
            }
            if (!db.Users.Any(u => u.UserName.Trim().Equals(username)))
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
            var user = db.Users.Where(u => u.UserName.Trim().Equals(username)).FirstOrDefault();
            var checkpass = BCrypt.Net.BCrypt.Verify(password, user.Password.Trim());
            if (!checkpass)
            {
                return Json(
              new
              {
                  status = "error",
                  message = "Mật khẩu không đúng vui lòng kiểm tra lại!"

              }
              , JsonRequestBehavior.AllowGet
              );
            }
            Session["UserID"] = user.UserID;


            return Json(
              new
              {
                  status = "success",
                  message = "Đang nhập thành công!"

              }
              , JsonRequestBehavior.AllowGet
              );



        }
    }
}