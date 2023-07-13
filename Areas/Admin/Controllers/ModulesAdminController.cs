using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class ModulesAdminController : Controller
    {
        DBEntities db = new DBEntities();
        // GET: Admin/ModulesAdmin
        public ActionResult Index()
        {
            return View();
        }
        public string UploadImage(HttpPostedFileBase file,string name)
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
                    _FileName = sub11 + sub21 + name + file.FileName;
                    file.SaveAs(Server.MapPath("/img/" + _FileName));
                    return "/img/" + _FileName;
                }
            }
            return "";
        }
        [HttpPost,ValidateInput(false)]
        public JsonResult CreateModule(Module module)
        {
            string UserID = Session["UserID"].ToString();
            try
            {
                
                if (!db.Modules.Any())
                {
                    module.CreateBy = UserID;
                    module.CreateDate = DateTime.Now;
                    db.Modules.Add(module);
                }
                else {
                 
                    var oldModule = db.Modules.FirstOrDefault();
                    oldModule.Logo = module.Logo;
                    oldModule.BannerGame = module.BannerGame;
                    oldModule.BannerBlog = module.BannerBlog;
                    oldModule.BannerPromotion = module.BannerPromotion;
                    oldModule.Address = module.Address;
                    oldModule.Email = module.Email;
                    oldModule.Hotline = module.Hotline;
                    oldModule.AboutMe = module.AboutMe;
                    oldModule.ModifyBy = UserID;
                    oldModule.ModifyDate = DateTime.Now;
                    db.SaveChanges();
                }
              

                return Json(
                new
                {
                    status = "success",


                }
                , JsonRequestBehavior.AllowGet
            );
            }catch (Exception ex)
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
        public JsonResult GetModule()
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
                m.AboutMe,
                m.Logo,
                m.BannerGame,
                m.BannerBlog,
                m.BannerPromotion
               
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