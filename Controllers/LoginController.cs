using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPGame;
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
        [HttpPost]
        public JsonResult Adds(Account formData)
        {
            try
            {
                var date = DateTime.Now;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(formData.AccountName);
                string base64String = Convert.ToBase64String(bytes);
                formData.AccountID = base64String + date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
                formData.CreateDate = DateTime.Now;
                formData.ModifyDate = DateTime.Now;
                formData.Password = 
                //db.Projects.Add(project);
                //db.SaveChanges();
                return Json(new { code = 200, msg = "Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}