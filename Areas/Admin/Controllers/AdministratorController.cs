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
    public class AdministratorController : Controller
    {
        
        private readonly DBEntities db = new DBEntities();
        // GET: Admin/Administator
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DataTable(int page = 0, string search ="")
        {
            try
            {
                var data = db.Users.Select(a => new
                {
                    ID = a.UserID.Trim(),
                    UserName = a.UserName.Trim(),
                    Name = a.Name.Trim(),
                    CreateAt = a.CreateDate,
                    Update = a.ModifyDate,
                    Role = a.Role1.Description.Trim(),
                    a.Status
                });
                ///Tìm kiếm
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(x => x.UserName.Contains(search) || x.UserName.ToLower().Contains(search));
                }

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
            }catch (Exception e)
            {
                    return this.Json(
                 new
                 {
                  error = e.Message

                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
           
        }

        // GET: Admin/Administator/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Administator/Create
        public ActionResult Create()
        {
           ViewBag.Roles = db.Roles.ToList();
            return View();
        }

        // POST: Admin/Administator/Create
        [HttpPost]
        public JsonResult Create(User user)
        {
            try
            {
                // TODO: Add insert logic here
                if(db.Users.Any(u => u.UserName.Equals(user.UserName)))
                {
                    return Json(
                    new
                    {
                        status = "error",
                        message = "Tài khoản đã có người sử dụng"

                    }
                    , JsonRequestBehavior.AllowGet);
                }
                //Tạo ID cho user
                var date = DateTime.Now;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(user.UserName);
                string base64String = Convert.ToBase64String(bytes);
                user.UserID = base64String + date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
                user.CreateDate = DateTime.Now;
                user.Status = true;
                db.Users.Add(user);
                db.SaveChanges();

                return Json(
                     new
                     {
                         status = "success",
                         
                     }
                     , JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(
                   new
                   {
                       status = "error",
                       message = "Không thành công!!!"

                   }
                   , JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Administator/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.Roles = db.Roles.ToList();
            var user = db.Users.Find(id);
            if(string.IsNullOrEmpty(id)|| user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: Admin/Administator/Edit/5
        [HttpPost]
        public JsonResult Edit(FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var user = db.Users.Find(collection["UserID"].Trim());
                if(user == null)
                {
                    return Json(
                                    new
                                    {
                                        status = false,
                                        message = "Tài khoản không tồn tại!!!"

                                    }
                                    , JsonRequestBehavior.AllowGet);
                }

                string formUserName = collection["UserName"].Trim();
                if(!formUserName.Equals(user.UserName.Trim()) && db.Users.Any(u => u.UserName.Equals(formUserName)))
                {
                    return Json(
                                    new
                                    {
                                        status = false,
                                        message = "Tài khoản đã tồn tại!!!"

                                    }
                                    , JsonRequestBehavior.AllowGet);
                }

                user.Name = collection["Name"].Trim();
                user.UserName = collection["UserName"].Trim();
                if (!string.IsNullOrEmpty(collection["newPassword"])){
                  user.Password =  BCrypt.Net.BCrypt.HashPassword(collection["newPassword"]);
                }
                db.SaveChanges();
                return Json(
                   new
                   {
                       status = true,
                    

                   }
                   , JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(
                   new
                   {
                       status = false,
                       message = "Không thành công!!!"

                   }
                   , JsonRequestBehavior.AllowGet);
            }
        }

   
       

        // POST: Admin/Administator/Delete/5
        [HttpPost]
        public JsonResult Delete(string id ="")
        {
            try
            {
                // TODO: Add delete logic here
                if(string.IsNullOrEmpty(id) || !db.Users.Any(u => u.UserID.Trim().Equals(id)))
                {
                    return Json(
                  new
                  {
                      status = "error",
                      message = "Tài khoản không tồn tại!!!"

                  }
                  , JsonRequestBehavior.AllowGet);
                }
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return Json(
               new
               {
                   status = "success",
               }
               , JsonRequestBehavior.AllowGet);


            }
            catch
            {
                return Json(
                  new
                  {
                      status = "error",
                      message = "Không thành công!!!"

                  }
                  , JsonRequestBehavior.AllowGet);
            }
        }
    }
}
