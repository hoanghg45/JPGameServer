using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class ReportShiftController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Admin/ReportShift
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult InShift(int inShift)
        {

            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var shift = db.InShifts.Find(inShift);
                var sp = db.InShifts.OrderBy(x => x.Id > 0).ToList().LastOrDefault();
                return Json(
                new
                {
                    status = "success",
                    shift = shift,

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
        public JsonResult Adds(string FirstShiftMoney)
        {

            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var cashiers = db.NFCReaders.OrderBy(x => x.ReaderID == user.ReaderID).ToList().LastOrDefault().Cashier1;
                var cashiersName = cashiers.Name;
                InShift inShift = new InShift()
                {
                    IdUsers = user.UserID,
                    Cashiers = cashiers.CashierID,
                    FirstShiftMoney = float.Parse(FirstShiftMoney),
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    CreateBy = user.Name,
                    ModifyBy = user.Name,
                    Status = true
                };
                db.InShifts.Add(inShift);
                db.SaveChanges();
                var sp = db.InShifts.OrderBy(x => x.Id > 0).ToList().LastOrDefault();
                return Json(
                new
                {
                    status = "success",
                    cashiersName= cashiersName,
                    sp= sp,
                    userName = user.Name,

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
        public JsonResult Adds1(string FirstShiftMoney,string EndShiftMoney,string RealMoneySale)
        {

            try
            {
                string UserID = Session["UserID"].ToString();
                var user = db.Users.Find(UserID);
                var cashiers = db.NFCReaders.OrderBy(x => x.ReaderID == user.ReaderID).ToList().LastOrDefault().Cashier1;
                var cashiersName = cashiers.Name;
                OutShift outShift = new OutShift()
                {
                    IdUsers = user.UserID,
                    Cashiers = cashiers.CashierID,
                    RealMoneySale = float.Parse(RealMoneySale),
                    EndShiftMoney = float.Parse(EndShiftMoney),
                    FirstShiftMoney = float.Parse(FirstShiftMoney),
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    CreateBy = user.Name,
                    ModifyBy = user.Name,
                    Status = true
                };
                db.OutShifts.Add(outShift);
                db.SaveChanges();
                var sp = db.OutShifts.OrderBy(x => x.Id > 0).ToList().LastOrDefault();
                return Json(
                new
                {
                    status = "success",
                    cashiersName= cashiersName,
                    sp= sp,
                    userName = user.Name,

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