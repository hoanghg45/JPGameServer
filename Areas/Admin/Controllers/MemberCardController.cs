using JPGame.Areas.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class MemberCardController : Controller
    {
        readonly DBEntities db = new DBEntities();
        // GET: Admin/MemberCard
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/MemberCard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/MemberCard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MemberCard/Create
        [HttpPost]
        public JsonResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                string CardID = "Card1";
                var MemberCardLevel = db.MemberCards.Find(collection["MemberCardLevelID"]);
                var MemberCard = new MemberCard {
                    MemberCardID = CardID,
                    MemberCardLevelID = Int16.Parse(collection["MemberCardLevelID"]),
                    Points = double.Parse(collection["PointReview"].Replace(",","")),
                };

                return this.Json(
                 new
                 {
                     status = "Success",


                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
            catch
            {
                return this.Json(
                 new
                 {
                     status = "Success",


                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
        } 
        public ActionResult MoneyCharge()
        {
            return View();
        }

        // POST: Admin/MemberCard/Create
        [HttpPost]
        public JsonResult MoneyCharge(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                string CardID = "Card1";
                var MemberCardLevel = db.MemberCards.Find(collection["MemberCardLevelID"]);
                var MemberCard = new MemberCard {
                    MemberCardID = CardID,
                    MemberCardLevelID = Int16.Parse(collection["MemberCardLevelID"]),
                    Points = double.Parse(collection["PointReview"].Replace(",","")),
                };

                return this.Json(
                 new
                 {
                     status = "Success",


                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
            catch
            {
                return this.Json(
                 new
                 {
                     status = "Success",


                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
        }

        // GET: Admin/MemberCard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/MemberCard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/MemberCard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/MemberCard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
