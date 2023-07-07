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
        [HttpPost]
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
                    module.ModifyBy = UserID;
                    module.ModifyDate = DateTime.Now;
                    var oldModule = db.Modules.FirstOrDefault();
                    oldModule = module;
                }
                db.SaveChanges();

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