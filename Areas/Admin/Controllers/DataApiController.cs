using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace JPGame.Areas.Admin.Controllers
{
    public class DataApiController : Controller
    {
        DBEntities db = new DBEntities();

        [HttpGet]
        // GET api/<controller>/5
        public JsonResult GetScanCard(string card, string reader)
        {
            try {
                
                if (string.IsNullOrEmpty(card) ||string.IsNullOrEmpty(reader))
                {
                    return Json(
                    new
                    {
                        status = "fail",
                        message = "Lỗi hãy thử lại!"

                    }
                    , JsonRequestBehavior.AllowGet);
                }
                if(!db.MemberCards.Any(c => c.MemberCardID.Equals(card)))
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Thẻ không tồn tại vui lòng thử lại!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                var OldCard = db.LiveCards.Where(c => c.ReaderID.Equals(reader));
                db.LiveCards.RemoveRange(OldCard);

                var Card = new LiveCard { CardID = card, ReaderID = reader, ScanAt = DateTime.Now };
                db.LiveCards.Add(Card);
                db.SaveChanges();
                return Json(
                    new
                    {
                        status = "ok",

                    }
                    , JsonRequestBehavior.AllowGet);

            }catch(Exception e)
            {
                return Json(
                   new
                   {
                       status = "fail",
                       message = e

                   }
                   , JsonRequestBehavior.AllowGet);
            }


        }
        
        [System.Web.Http.HttpGet]
         public JsonResult CheckCardMoney(string card, string reader)
        {
            try {
                
                if (string.IsNullOrEmpty(card) ||string.IsNullOrEmpty(reader))
                {
                    return Json(
                    new
                    {
                        status = "fail",
                        message = "Lỗi hãy thử lại!"

                    }
                    , JsonRequestBehavior.AllowGet);
                }
                if(!db.MemberCards.Any(c => c.MemberCardID.Equals(card)))
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Thẻ không tồn tại vui lòng thử lại!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                var Card = db.MemberCards.Find(card);
                var Reader = db.SettingGames.Find(reader);
                if (reader == "225077781")
                {
                    if (Card.Balance < 250000)
                    {
                        return Json(
                         new
                         {
                             status = "fail",
                             message = "số dư khong đủ!"

                         }
                         , JsonRequestBehavior.AllowGet);
                    }
                }
                if (Reader == null)
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Reader khong dung!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                bool cardMoney = Card.Balance.HasValue && Card.Balance > 0 ;
                bool rsl = false;
                var status = "fail";
                if (cardMoney)
                {
                   if((Card.Balance - Reader.Price) >= 0)
                    {
                        status = "ok";
                        rsl = true;
                        if(Card.Points!= null)
                        {
                            Card.Points += Reader.PushPoint;
                        }
                        Card.Balance = Card.Balance - Reader.Price;
                        db.SaveChanges();
                        ReportGameHistory reportGameHistory = new ReportGameHistory()
                        {
                            IdGame = reader,
                            IdCard = card,
                            CreateDate = DateTime.Now,
                            ModifyDate = DateTime.Now,
                        };
                        db.ReportGameHistories.Add(reportGameHistory);
                        db.SaveChanges();
                    }
                }
                else
                {
                    status = "fail";
                    rsl = false;
                }

                return Json(
                    new
                    {

                        status = status,

                        message = rsl
                    }
                    , JsonRequestBehavior.AllowGet);

            }catch(Exception e)
            {
                return Json(
                   new
                   {
                       status = "fail",
                       message = e

                   }
                   , JsonRequestBehavior.AllowGet);
            }


        }
        [System.Web.Http.HttpGet]
        public JsonResult CheckAccount(string card, string reader)
        {
            try
            {

                if (string.IsNullOrEmpty(card) || string.IsNullOrEmpty(reader))
                {
                    return Json(
                    new
                    {
                        status = "fail",
                        message = "Lỗi hãy thử lại!"

                    }
                    , JsonRequestBehavior.AllowGet);
                }
                if (!db.MemberCards.Any(c => c.MemberCardID.Equals(card)))
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Thẻ không tồn tại vui lòng thử lại!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                var Card = db.MemberCards.Find(card);
                var Reader = db.SettingGames.Find(reader);
                if (Reader == null)
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Reader khong dung!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                bool rsl = false;
                var status = "fail";
                var cardInfo = db.MemberCards.Where(c => c.MemberCardID.Equals(card))
                    .Select(r => new
                    {
                        r.MemberCardLevel.CardLevel.LevelName,
                        r.Balance,
                        r.Points,

                    }).FirstOrDefault();
                    
                if (Card != null)
                {
                    status = "ok";
                    return Json(
                   new
                   {

                       status = status,

                       message = cardInfo
                   }
                   , JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = "fail";
                    rsl = false;
                }

                return Json(
                    new
                    {

                        status = status,

                        message = rsl
                    }
                    , JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(
                   new
                   {
                       status = "fail",
                       message = e

                   }
                   , JsonRequestBehavior.AllowGet);
            }


        }
        [System.Web.Http.HttpGet]
        public JsonResult Promotion(string IdPromotion)
        {
            try
            {
                var date = DateTime.Now;
                var currentDate = date.Day + date.Month * 31 * date.Year;
                if (string.IsNullOrEmpty(IdPromotion))
                {
                    return Json(
                    new
                    {
                        status = "fail",
                        message = "Lỗi hãy thử lại!"

                    }
                    , JsonRequestBehavior.AllowGet);
                }
                if (!db.PromotionVouchers.Any(c => c.PromotionCode.Equals(IdPromotion)))
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Mã Không Tồn Tại!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                var promotion = db.PromotionVouchers.Find(IdPromotion);
                if (promotion == null)
                {
                    return Json(
                   new
                   {
                       status = "fail",
                       message = "Lỗi hãy thử lại!"

                   }
                   , JsonRequestBehavior.AllowGet);
                }
                if(promotion.Status == false)
                {
                    return Json(
                  new
                  {
                      status = "fail",
                      message = "Mã Đã Được Sử Dụng!"

                  }
                  , JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (promotion.Type == 1)
                    {
                        var promotionInfo = db.PromotionVouchers.Where(c => c.PromotionCode.Equals(IdPromotion)&&c.Status==true)
                              .Select(r => new
                              {
                                  r.Type,
                                  r.StartTime,
                                  r.EndTime,
                                  r.ReceiveMoney,
                                  r.Des
                              }).Where(x=>x.StartTime.Value.Day+ x.StartTime.Value.Month*31* x.StartTime.Value.Year<currentDate&& x.EndTime.Value.Day + x.EndTime.Value.Month * 31 * x.EndTime.Value.Year>currentDate);
                        if (promotionInfo.Count() == 0)
                        {
                            return Json(
                                 new
                                 {
                                     status = "fail",
                                     message = "Mã Hết Hạn"

                                 }
                                 , JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(
                               new
                               {
                                   status = "ok",
                                   message = promotionInfo

                               }
                               , JsonRequestBehavior.AllowGet);
                        }
                   
                    }else if(promotion.Type == 2)
                    {
                        var promotionInfo = db.PromotionVouchers.Where(c => c.PromotionCode.Equals(IdPromotion))
                              .Select(r => new
                              {
                                  r.Type,
                                  r.StartTime,
                                  r.EndTime,
                                  r.MinimumMoney,
                                  r.VoucherDiscount,
                                  r.Des
                              }).Where(x => x.StartTime.Value.Day + x.StartTime.Value.Month * 31 * x.StartTime.Value.Year < currentDate && x.EndTime.Value.Day + x.EndTime.Value.Month * 31 * x.EndTime.Value.Year > currentDate);
                        if (promotionInfo.Count() == 0)
                        {
                            return Json(
                                 new
                                 {
                                     status = "fail",
                                     message = "Mã Hết Hạn"

                                 }
                                 , JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(
                               new
                               {
                                   status = "ok",
                                   message = promotionInfo

                               }
                               , JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(
                          new
                          {
                              status = "fail",
                              message ="Có Lỗi"

                          }
                          , JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(
                   new
                   {
                       status = "fail",
                       message = e

                   }
                   , JsonRequestBehavior.AllowGet);
            }


        }
        // POST api/<controller>
        [System.Web.Http.HttpPost]
        public JsonResult AddReader(List<SettingGame> data)
        {
            try
            {
                var filteredRecords = data.Where(r => !db.SettingGames.Any(er => er.Id == r.Id)).ToList();
                db.SettingGames.AddRange(filteredRecords);
                db.SaveChanges();
                return Json(
                    new
                    {
                        status = "ok",
                        message = "true"
                    }
                    , JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(
                   new
                   {
                       status = "fail",
                       message = "Không thành công!!!"

                   }
                   , JsonRequestBehavior.AllowGet);
            }
        }


        // POST api/<controller>
        [System.Web.Http.HttpGet]
        public JsonResult GetReaders()
        {
            try
            {
                var data = db.SettingGames.ToList();
                return Json(
                    new
                    {
                        status = "ok",
                        message = data
                    }
                    , JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(
                   new
                   {
                       status = "fail",
                       message = "Không thành công!!!"

                   }
                   , JsonRequestBehavior.AllowGet);
            }
        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}