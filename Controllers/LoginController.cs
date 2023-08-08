using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPGame;
using OfficeOpenXml;

namespace JPGame.Controllers
{
    public class LoginController : Controller
    {
        private DBEntities db = new DBEntities(); 
        // GET: Login
        public ActionResult SignUp()
        {
            return View();
        } 
        public ActionResult SignIn()
        {
            return View();
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
                    _FileName = sub11 + sub21 + "avatar" + file.FileName;
                    file.SaveAs(Server.MapPath("/img/" + _FileName));
                    return "/img/" + _FileName;
                }
            }
            return "";
        }
        [HttpPost]
        public JsonResult Adds(Account formData)
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
                formData.Password = BCrypt.Net.BCrypt.HashPassword(formData.Password);
                db.Accounts.Add(formData);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Đã Có Tài Khoản " +formData.AccountName}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edits(Account formData)
        {
            try
            {
                var user = (Account)Session["account"];
                var account = db.Accounts.Find(user.AccountID);
                account.Avatar = formData.Avatar;
                account.FullName = formData.FullName;
                account.DateOfBirth = formData.DateOfBirth;
                account.Wedding = formData.Wedding;
                account.Phone = formData.Phone;
                account.Email = formData.Email;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Đã Có Tài Khoản " + formData.AccountName }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ChangePass(string pass,string new_pass)
        {
            try
            {
                var user = (Account)Session["account"];
                var account = db.Accounts.Find(user.AccountID);
                if (BCrypt.Net.BCrypt.Verify(pass, account.Password))
                {
                    account.Password = BCrypt.Net.BCrypt.HashPassword(new_pass);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Đổi Mật Khẩu Thành Công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 500,msg="Mật Khẩu Không Đúng!" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception e)
            {
                return Json(new { code = 500, }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Logins(Account formData)
        {
            try
            {
                var user = db.Accounts.SingleOrDefault(x => x.AccountName == formData.AccountName);
                if(user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(formData.Password, user.Password))

                    {
                        Session["account"] = user;
                        return Json(new { code = 200, msg = "Thành Công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = 500, msg = "Không Đúng Mật Khẩu!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = 500, msg = "Không Đúng Thông Tin Tài Khoản!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAccountInfo(string UserName)
        {
            try
            {
                var user = db.Accounts.Where(u => u.AccountName.Equals(UserName.Trim())).FirstOrDefault();
                if(user == null)
                {
                   return Json(new { code = 500, msg = "Tài khoản không tồn tại!" }, JsonRequestBehavior.AllowGet);


                    
                }
                if (!string.IsNullOrEmpty(user.MemberCardID))
                {
                    return Json(new { code = 500, msg = "Tài khoản đã có thẻ thành viên!" }, JsonRequestBehavior.AllowGet);

                }
                
                return Json(new { code = 200, data = user}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        
    }
}