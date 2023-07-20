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
                var currUser = (Session["UserID"].ToString());
                string CardID = collection["CardID"];
                var MemberCardLevel = db.MemberCards.Find(collection["MemberCardLevelID"]);
                var card = db.MemberCards.Find(CardID);
                if (card == null)
                {
                            return this.Json(
                       new
                       {
                           status = "Error",
                           message = "Thẻ không tồn tại, vui lòng kiểm tra lại!"

                       }
                       , JsonRequestBehavior.AllowGet
                       );
                }
                if (!card.MemberCardLevel.CardLevel.LevelName.Trim().Equals("Welcome"))
                {
                    string accname = collection["AccountName"];
                    var acc = db.Accounts.Where(a => a.AccountName.Trim().Equals(accname)).FirstOrDefault();
                    if(string.IsNullOrEmpty(accname) || acc == null)
                    {
                        return this.Json(
                      new
                      {
                          status = "Error",
                          message = "Tài khoản không tồn tại!"

                      }
                      , JsonRequestBehavior.AllowGet
                      );
                    }
                    acc.MemberCardID = card.MemberCardID;

                }
                card.Balance = Double.Parse(collection["Money"].Replace(",", ""));
                card.Points = Double.Parse(collection["Point"].Replace(",", ""));
                card.Status = true;
                var chargeRecord = new MemberCardChargeRecord
                {
                    MemberCardID = card.MemberCardID,
                    Money = card.Balance,
                    ChargeDate = DateTime.Now,
                    CreateBy = currUser,
                };
                db.MemberCardChargeRecords.Add(chargeRecord);
                db.SaveChanges();
                return this.Json(
                 new
                 {
                     status = "Success",


                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
            catch(Exception e)
            {
                return this.Json(
                 new
                 {
                     status = "Error",
                     message = e

                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
        } 
        public ActionResult MoneyCharge()
        {
            return View();
        }

        // POST: Admin/MemberCard/MoneyCharge

        [HttpPost]
        public JsonResult MoneyCharge(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                string CardID = collection["CardID"];
                var MemberCardLevel = db.MemberCards.Find(collection["MemberCardLevelID"]);
                var card = db.MemberCards.Find(CardID);
                if (card == null)
                {
                    return this.Json(
               new
               {
                   status = "Error",
                   message = "Thẻ không tồn tại, vui lòng kiểm tra lại!"

               }
               , JsonRequestBehavior.AllowGet
               );
                }
                if (!card.MemberCardLevel.CardLevel.LevelName.Trim().Equals("Welcome"))
                {
                    string accname = collection["AccountName"];
                    var acc = db.Accounts.Where(a => a.AccountName.Trim().Equals(accname)).FirstOrDefault();
                    if (string.IsNullOrEmpty(accname) || acc == null)
                    {
                        return this.Json(
                      new
                      {
                          status = "Error",
                          message = "Tài khoản không tồn tại!"

                      }
                      , JsonRequestBehavior.AllowGet
                      );
                    }
                    acc.MemberCardID = card.MemberCardID;

                }
                card.Balance = Double.Parse(collection["Money"].Replace(",", ""));
                card.Points = Double.Parse(collection["Point"].Replace(",", ""));
                card.Status = true;
                db.SaveChanges();
                return this.Json(
                 new
                 {
                     status = "Success",


                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
            catch (Exception e)
            {
                return this.Json(
                 new
                 {
                     status = "Error",
                     message = e

                 }
                 , JsonRequestBehavior.AllowGet
                 );
            }
        }
        [HttpGet]
        public JsonResult GetCurrentCardForCharge()
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime oneMinuteAgo = now.AddMinutes(-2).AddSeconds(-now.Second);

                string reader = "7374d2c8e7b943c7";
                var currCard = db.LiveCards.Where(c => c.ReaderID.Equals(reader) && c.ScanAt >= oneMinuteAgo && c.ScanAt <= now).FirstOrDefault();
                //var currCard = db.LiveCards.Where(c => c.ReaderID.Equals(reader)).FirstOrDefault();

                if (currCard == null)
                {
                    return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Vui lòng quét lại thẻ!"

                    }
                    , JsonRequestBehavior.AllowGet
                    );
                }
                var memberCard = db.MemberCards.Where(c => c.MemberCardID.Equals(currCard.CardID))
                    .Select(c => new
                    {
                        c.MemberCardID,
                        c.MemberCardLevel.CardLevelID,
                        c.MemberCardLevel.CardLevel.LevelName,
                        c.MemberCardLevel.Gift.GiftLevelName,
                        RewardRate = Math.Round(c.MemberCardLevel.Gift.RewardRate.Value * 100),
                        c.MemberCardLevel.Gift.PointPlus,
                        Holiday = c.MemberCardLevel.Gift.PersonalGiftID == null ? false : c.MemberCardLevel.Gift.PersonalGift.Holiday,
                        Personal = c.MemberCardLevel.Gift.PersonalGiftID == null ? false : c.MemberCardLevel.Gift.PersonalGift.Personal,
                        SpecialDay = c.MemberCardLevel.Gift.PersonalGiftID != null && c.MemberCardLevel.Gift.PersonalGift.SpecialDay,
                        SpecialMemory = c.MemberCardLevel.Gift.SpecialMemory == null ? false : c.MemberCardLevel.Gift.SpecialMemory.AvailableTemplates,
                        CustomizeAvailableTemplate = c.MemberCardLevel.Gift.SpecialMemory == null ? false : c.MemberCardLevel.Gift.SpecialMemory.CustomizeAvailableTemplate,
                        c.MemberCardLevel.VIP,
                        Mocktail = (bool)c.MemberCardLevel.VIP ? c.MemberCardLevel.VIPGift.Moctail : false,
                        VipRoom = (bool)c.MemberCardLevel.VIP ? c.MemberCardLevel.VIPGift.VipRoom : false,
                        Total = c.MemberCardChargeRecords.Sum(i => i.Money),
                        Owner = c.Accounts.Any()? c.Accounts.FirstOrDefault().FullName :null,
                        c.Balance,
                        c.Status
                    }).FirstOrDefault();
                if (!memberCard.Status.HasValue || !memberCard.Status.Value)
                {
                    return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Thẻ này chưa được kích hoạt vui lòng kiểm tra lại!"

                    }
                    , JsonRequestBehavior.AllowGet
                    );
                }

                if (memberCard == null)
                {
                    return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Vui lòng quét lại thẻ!"

                    }
                    , JsonRequestBehavior.AllowGet
                    );
                }



                return this.Json(
                    new
                    {
                        status = "Success",
                        card = memberCard

                    }
                    , JsonRequestBehavior.AllowGet
                    );
            }catch(Exception e)
            {
                return this.Json(
                    new
                    {
                        status = "Success",
                        message = e.Message

                    }
                    , JsonRequestBehavior.AllowGet
                    );
            }
           
        }
        [HttpGet]
        public JsonResult GetCurrentCardForCreate(string level)
        {
            DateTime now = DateTime.Now;
            DateTime oneMinuteAgo = now.AddMinutes(-2).AddSeconds(-now.Second);
             
            string reader = "7374d2c8e7b943c7";
            var currCard = db.LiveCards.Where(c => c.ReaderID.Equals(reader) && c.ScanAt >= oneMinuteAgo && c.ScanAt <= now).FirstOrDefault();
            //var currCard = db.LiveCards.Where(c => c.ReaderID.Equals(reader)).FirstOrDefault();
            
            if (currCard == null)
            {
                return this.Json(
                new
                {
                    status = "Error",
                    message = "Vui lòng quét lại thẻ!"

                }
                , JsonRequestBehavior.AllowGet
                );
            }
            var memberCard = db.MemberCards.Find(currCard.CardID);

            if (memberCard == null)
            {
                return this.Json(
                new
                {
                    status = "Error",
                    message = "Vui lòng quét lại thẻ!"

                }
                , JsonRequestBehavior.AllowGet
                );
            }

            if (!memberCard.MemberCardLevel.CardLevel.LevelName.Trim().Equals(level))
            {
                return this.Json(
             new
             {
                 status = "Error",
                 message = "Loại thẻ không đúng so với số tiền nạp, vui lòng quét lại!"

             }
             , JsonRequestBehavior.AllowGet
             );
            }

            return this.Json(
                new
                {
                    status = "Success",
                    card = memberCard.MemberCardID

                }
                , JsonRequestBehavior.AllowGet
                );
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
