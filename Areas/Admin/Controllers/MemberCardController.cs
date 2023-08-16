using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using NinjaNye.SearchExtensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            ViewBag.Level = db.CardLevels.ToList();
            return View();
        }
        [HttpGet]
        public JsonResult DataTable(int page = 0, string search ="", string level= "",bool? status =null)
        {
            var data = db.MemberCards.Select(a => new
            {
               
                Code =a.Code39,
                LevelID = a.MemberCardLevel.CardLevel.ID.Trim(),
                Level = a.MemberCardLevel.CardLevel.LevelName.Trim(),
                Owner = a.Accounts.Any()? a.Accounts.FirstOrDefault().AccountName.Trim() : "",
                StatusDes =  a.Status.Value? "Đã kích hoạt":"Chưa kích hoạt",
                Status =  a.Status.Value,
                CreateAt= a.CreateDate
                
            });

            var c = data.ToList();
            // Lọc theo loại thẻ (type)
            data = data.WhereIf(!string.IsNullOrEmpty(level), a => a.LevelID.Equals(level))
                   // Lọc theo trạng thái (status)
                   .WhereIf(status.HasValue, a => a.Status == status);

            // tìm kiếm 
            if (!string.IsNullOrEmpty(search))
            {
               data = data.Search(x => x.Code,
                            x => x.Level,
                            x => x.Owner,
                            x =>x.Code

                                 ).Containing(search.Trim());
            }

             c = data.ToList();
            


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
        }
        // GET: Admin/MemberCard/Details/5
        public ActionResult Details(string id)
        {
            string ReaderID = Session["ReaderID"].ToString();
            ViewBag.ReaderID = ReaderID;

            var unHaveCardMembers = db.Accounts.Where(a => string.IsNullOrEmpty(a.MemberCardID)).ToList();
            ViewBag.unHaveCardMembers = unHaveCardMembers;
            return View(model:id);
        }

        // GET: Admin/MemberCard/Create
        public ActionResult Create()
        {
            string ReaderID = Session["ReaderID"].ToString();
            ViewBag.ReaderID = ReaderID;
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
                var user = db.Users.Find(currUser);
                string CardID = collection["CardID"];
                string level = collection["MemberCardLevelID"];
                var MemberCardLevel = db.CardLevels.Find(collection["MemberCardLevelID"]);
                var card = db.MemberCards.Where(m => m.Code39.Equals(CardID)).FirstOrDefault();
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
                string accname = collection["AccountName"];
                if (!string.IsNullOrEmpty(accname))
                {
                   
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
                //Thêm thông tin dữ liệu khách nếu không muốn tạo tài khoản
                else
                {
                    if (level.Trim().Equals("level1"))
                    {
                        card.Phone = collection["Phone"];
                        card.Name = collection["FullName"];
                    }
                }

                //
                card.Balance = Double.Parse(collection["Money"].Replace(",", ""));
                card.Points = Double.Parse(collection["Point"].Replace(",", ""));
                card.Status = true;
                card.ModifyDate = DateTime.Now;
                var paytype = int.Parse(collection["radiospay"]);
                string paycode = collection["Paycode"] == ""? null: collection["Paycode"].ToString();
                string cashier = db.NFCReaders.Find(user.ReaderID).Cashier1.Name;
               
                var chargeRecord = new MemberCardChargeRecord
                {
                    MemberCardID = card.MemberCardID,
                    Money = Double.Parse(collection["MoneyPay"].Replace(",", "")),
                    ChargeDate = DateTime.Now,
                    CreateBy = currUser,
                    TypePay = paytype,
                    Paycode = paycode,
                    RecordType = "Create",
                    Cashier = cashier
                   
                };
                ///mã khuyến mãi

                if (!string.IsNullOrEmpty(collection["Promotion"]))
                {
                    var promotion = db.PromotionVouchers.Find(collection["Promotion"]);
                    chargeRecord.PromotionDes = promotion.Des;
                    chargeRecord.PromotionID = promotion.PromotionCode;
                    promotion.Status = false;
                }
                ///
                if (!string.IsNullOrEmpty(collection["Discount"]))
                {
                    chargeRecord.PromotionDes = $"Khuyến mãi {collection["Discount"]}% ";
                }
                ///
                db.MemberCardChargeRecords.Add(chargeRecord);




                db.SaveChanges();
                return this.Json(
                 new
                 {
                     status = "Success",

                     userID = user.UserID,
                     userName = user.Name,
                     sp = new { chargeRecord.RecordID, chargeRecord.ChargeDate },
                     cashier = cashier,
                     member = accname
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
            string ReaderID = Session["ReaderID"].ToString();
            ViewBag.ReaderID = ReaderID;
            return View();
        }

        // POST: Admin/MemberCard/MoneyCharge

        [HttpPost]
        public JsonResult MoneyCharge(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var currUser = (Session["UserID"].ToString());
                var user = db.Users.Find(currUser);
                string OldCardID = collection["CurrCardID"];
                string NewCardID = collection["NewCardID"];
                var MemberCardLevel = db.MemberCardLevels.Find(collection["MemberCardLevelID"]);
                var oldCard = db.MemberCards.Where(m => m.Code39.Equals(OldCardID)).FirstOrDefault();
                var newCard = db.MemberCards.Where(m => m.Code39.Equals(NewCardID)).FirstOrDefault();
                if (oldCard == null)
                {
                    return this.Json(
                new
                {
                    status = "Error",
                    message = "Thẻ cũ không tồn tại, vui lòng kiểm tra lại!"

                }
                , JsonRequestBehavior.AllowGet
                );
                }

                double ChargeMoney = Double.Parse(collection["MoneyPay"].Replace(",", ""));//Số tiền nạp
                double FinalMoney = Double.Parse(collection["Money"].Replace(",", ""));//Số tiền sau khi đã tính
                double FinalPoint = Double.Parse(collection["Point"].Replace(",", ""));//Số điểm sau khi đã tính
                var LevelFee = db.CardLevels.OrderBy(f => f.LevelFee).Select(f => f.LevelFee).ToArray();
                //Nếu không thay đổi cấp độ
                if (oldCard.MemberCardLevel.CardLevel.LevelName.Equals(newCard.MemberCardLevel.CardLevel.LevelName))
                {
                    double oldSumMoney = Double.Parse(collection["CurrTotal"].Replace(",", ""));
                    double newSumMoney = Double.Parse(collection["TotalMoneyPay"].Replace(",", ""));
                    string accname = collection["AccountName"];
                    if (!string.IsNullOrEmpty(accname) && !oldCard.Accounts.Any())
                    {
                       
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
                        acc.MemberCardID = oldCard.MemberCardID;
                    }
                    oldCard.Balance = FinalMoney;
                    oldCard.Points = FinalPoint;
                    oldCard.ModifyDate = DateTime.Now;
                }
                else
                {
                    if (newCard == null)
                    {
                        return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Thẻ mới không tồn tại, vui lòng kiểm tra lại!"

                    }
                    , JsonRequestBehavior.AllowGet
                    );
                    }

                   
                    //Chuyển tiền sang thẻ mới
                    newCard.Balance = FinalMoney;
                    newCard.Points = FinalPoint;
                    newCard.Status = true;
                    ///
                    
                    var oldRecord = db.MemberCardChargeRecords.Where(r => r.MemberCardID == oldCard.MemberCardID).ToList();
                    foreach(var r in oldRecord)
                    {
                        r.MemberCardID = newCard.MemberCardID;
                    }
                    var oldGamehis = db.ReportGameHistories.Where(r => r.IdCard == oldCard.MemberCardID).ToList();
                    foreach(var g in oldGamehis)
                    {
                        g.IdCard = newCard.MemberCardID;
                    }

                    newCard.ModifyDate = DateTime.Now;
                    //Nếu thẻ được nâng cấp
                    string accname = collection["AccountName"];
                    if (!string.IsNullOrEmpty(accname) && !oldCard.Accounts.Any())
                    {
                       
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
                        acc.MemberCardID = newCard.MemberCardID;
                    }
                    else
                    {
                        var acc = oldCard.Accounts.FirstOrDefault();
                        acc.MemberCardID = newCard.MemberCardID;
                    }
                    oldCard.Status = false;
                    oldCard.Balance = 0;
                    oldCard.Points = 0;
                }
                var paytype = int.Parse(collection["radiospay"]);
                string paycode = collection["Paycode"] == "" ? null : collection["Paycode"].ToString();
                string cashier = db.NFCReaders.Find(user.ReaderID).Cashier1.Name;
                //Hủy thẻ cũ
           
                //oldCard.Accounts.Clear();
                //oldCard.ReportGameHistories.Clear();
                //oldCard.MemberCardChargeRecords.Clear();
              

                //Lưu thời gian nạp tiền
                var chargeRecord = new MemberCardChargeRecord
                {
                    MemberCardID = newCard.MemberCardID,
                    Money = ChargeMoney,
                    ChargeDate = DateTime.Now,
                    CreateBy = currUser,                  
                    TypePay = paytype,
                    Paycode = paycode,
                    RecordType = "Recharge",
                    Cashier = cashier,
                   
                };
                ///mã khuyến mãi

                if (!string.IsNullOrEmpty(collection["Promotion"]))
                {
                    var promotion = db.PromotionVouchers.Find(collection["Promotion"]);
                    chargeRecord.PromotionDes = promotion.Des;
                    chargeRecord.PromotionID = promotion.PromotionCode;
                    promotion.Status = false;
                }
                if (!string.IsNullOrEmpty(collection["Discount"]))
                {
                    chargeRecord.PromotionDes = $"Khuyến mãi {collection["Discount"]}% "; 
                }

                db.MemberCardChargeRecords.Add(chargeRecord);


                db.SaveChanges();
                return this.Json(
                 new
                 {
                     status = "Success",
                     userID = user.UserID,
                     userName = user.Name,
                     sp = new {chargeRecord.RecordID,chargeRecord.ChargeDate },
                     cashier = cashier,
                     member = collection["AccountName"]

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
        public ActionResult MemberCardReissuance()
        {
            string ReaderID = Session["ReaderID"].ToString();
            ViewBag.ReaderID = ReaderID;
            return View();
        }

        [HttpPost]
        public JsonResult MemberCardReissuance(string oldCardID, string newCardID)
        {
            try
            {
                var oldCard = db.MemberCards.Where(c => c.Code39.Equals(oldCardID)).FirstOrDefault();
                var newCard = db.MemberCards.Where(c => c.Code39.Equals(newCardID)).FirstOrDefault();
                if(string.IsNullOrEmpty(oldCardID)|| string.IsNullOrEmpty(newCardID) || oldCard ==null|| newCard == null)
                {
                    return this.Json(new
                    {
                        status = "Error",
                        message = "Thông tin chưa đầy đủ, vui lòng thử lại!"

                    }, JsonRequestBehavior.AllowGet
                    );
                }
                //Chuyển tiền sang thẻ mới
                newCard.Balance = oldCard.Balance;
                newCard.Points = oldCard.Points;
                newCard.Status = true;
                ///

                var oldRecord = db.MemberCardChargeRecords.Where(r => r.MemberCardID == oldCard.MemberCardID).ToList();
                foreach (var r in oldRecord)
                {
                    r.MemberCardID = newCard.MemberCardID;
                }
                var oldGamehis = db.ReportGameHistories.Where(r => r.IdCard == oldCard.MemberCardID).ToList();
                foreach (var g in oldGamehis)
                {
                    g.IdCard = newCard.MemberCardID;
                }

                newCard.ModifyDate = DateTime.Now;
                //Nếu thẻ có Account

                if (oldCard.Accounts.Any())
                {


                    var acc = oldCard.Accounts.FirstOrDefault();
                    acc.MemberCardID = newCard.MemberCardID;
                }
              
                oldCard.Status = false;
                oldCard.Balance = 0;
                oldCard.Points = 0;
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
        [HttpPost]
        public JsonResult SaveReportRecharge(string userID, string idCard, string money, string paytype)
        {
            try
            {
                var user = db.Users.Find(userID.Trim());
                ReportRecharge reportRecharge = new ReportRecharge()
                {
                    IdCard = idCard,
                    IdUser = userID,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    CreateBy = user.Name,
                    ModifyBy = user.Name,
                    Status = true
                };
                db.ReportRecharges.Add(reportRecharge);
                db.SaveChanges();
                var sp = db.ReportRecharges.OrderBy(x => x.Status == true).ToList().LastOrDefault();
                var cashier = db.NFCReaders.Where(x => x.ReaderID == user.ReaderID).ToList().LastOrDefault().Cashier1.Name;
                string membername = db.MemberCards.Where(m => m.Code39.Equals(idCard)).FirstOrDefault().Accounts.Any() ? db.MemberCards.Find(idCard).Accounts.FirstOrDefault().FullName.Trim() : "";
                return this.Json(
                 new
                 {
                     status = "Success",
                     sp = sp,
                     cashier= cashier,
                     member = membername
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
        [HttpPost]
        public JsonResult SaveReportCreateCard(string userID, string idCard)
        {
            try
            {
                var user = db.Users.Find(userID.Trim());
                
                ReportCreateCard reportCreateCard = new ReportCreateCard()
                {
                    IdCard = idCard,
                    IdUser = userID,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    CreateBy = user.Name,
                    ModifyBy = user.Name,
                 
                    Status = true
                };
                db.ReportCreateCards.Add(reportCreateCard);
                db.SaveChanges();
                var sp = db.ReportCreateCards.OrderBy(x => x.Status == true).ToList().LastOrDefault();
                var cashier = db.NFCReaders.Where(x => x.ReaderID == user.ReaderID).ToList().LastOrDefault().Cashier1.Name;
                string membername = db.MemberCards.Where(m => m.Code39.Equals(idCard)).FirstOrDefault().Accounts.Any() ? db.MemberCards.Find(idCard).Accounts.FirstOrDefault().FullName.Trim() : "";
                return this.Json(
                 new
                 {
                     status = "Success",
                     sp = sp,
                     cashier = cashier,
                     member = membername
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
        [HttpPost]
        public JsonResult AddAccountForCard(string accountID, string cardID)
        {
            try
            {
                var acc = db.Accounts.Find(accountID.Trim());
                var card = db.MemberCards.Where(c => c.Code39.Equals(cardID.Trim())).FirstOrDefault();
                if (string.IsNullOrEmpty(accountID)|| string.IsNullOrEmpty(cardID) || acc == null || card == null)
                {
                    return this.Json(
                 new
                 {
                     status = "Error",
                     message = "Thông tin lỗi"

                 }
                 , JsonRequestBehavior.AllowGet
                 );
                }
                if (!string.IsNullOrEmpty(acc.MemberCardID))
                {
                    return this.Json(new
                    {
                        status = "Error",
                        message = "Tài khoản đã có thẻ thành viên"

                    }, JsonRequestBehavior.AllowGet
                );


                }
                if (card.Accounts.Any())
                {
                    return this.Json(new
                    {
                        status = "Error",
                        message = "Thẻ đã có người sở hữu"

                    }, JsonRequestBehavior.AllowGet
                );


                }
                acc.MemberCardID = card.MemberCardID;
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
                string reader = Session["ReaderID"].ToString();
                DateTime now = DateTime.Now;
                DateTime oneMinuteAgo = now.AddMinutes(-2).AddSeconds(-now.Second);

               
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
                        
                        MemberCardID = c.Code39,
                        c.MemberCardLevel.CardLevelID,
                        c.MemberCardLevel.CardLevel.LevelName,
                        c.MemberCardLevel.Gift.GiftLevelName,
                        RewardRate = Math.Round(c.MemberCardLevel.Gift.RewardRate.Value * 100),
                        c.Points,
                        Holiday = c.MemberCardLevel.Gift.PersonalGiftID == null ? false : c.MemberCardLevel.Gift.PersonalGift.Holiday,
                        Personal = c.MemberCardLevel.Gift.PersonalGiftID == null ? false : c.MemberCardLevel.Gift.PersonalGift.Personal,
                        SpecialDay = c.MemberCardLevel.Gift.PersonalGiftID != null && c.MemberCardLevel.Gift.PersonalGift.SpecialDay,
                        SpecialMemory = c.MemberCardLevel.Gift.SpecialMemory == null ? false : c.MemberCardLevel.Gift.SpecialMemory.AvailableTemplates,
                        CustomizeAvailableTemplate = c.MemberCardLevel.Gift.SpecialMemory == null ? false : c.MemberCardLevel.Gift.SpecialMemory.CustomizeAvailableTemplate,
                        c.MemberCardLevel.VIP,
                        Mocktail = (bool)c.MemberCardLevel.VIP ? c.MemberCardLevel.VIPGift.Moctail : false,
                        VipRoom = (bool)c.MemberCardLevel.VIP ? c.MemberCardLevel.VIPGift.VipRoom : false,
                        Total = c.MemberCardChargeRecords.Sum(i => i.Money),
                        isHaveOwner = c.Accounts.Any(),
                        Owner = c.Accounts.Any()? new
                        {
                            FullName = c.Accounts.Any() ? c.Accounts.FirstOrDefault().FullName : null,
                            DateOfBirth = c.Accounts.Any() ? c.Accounts.FirstOrDefault().DateOfBirth : null,
                            UserName = c.Accounts.Any() ? c.Accounts.FirstOrDefault().AccountName : null,
                            Email = c.Accounts.Any() ? c.Accounts.FirstOrDefault().Email : null,
                            Phone = c.Accounts.Any() ? c.Accounts.FirstOrDefault().Phone : null,
                        }:null,
                        Name = string.IsNullOrEmpty(c.Name)?"":c.Name.Trim(),
                        Phone = string.IsNullOrEmpty(c.Phone)?"":c.Phone.Trim(),

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
           
        }[HttpGet]
        public JsonResult GetDetailCard(string id)
        {
            try
            {
                
                var memberCard = db.MemberCards.Where(c => c.Code39.Equals(id))
                    .Select(c => new
                    {
                        
                        Code = c.Code39,
                        c.MemberCardLevel.CardLevelID,
                        c.MemberCardLevel.CardLevel.LevelName,
                        c.MemberCardLevel.Gift.GiftLevelName,
                        RewardRate = Math.Round(c.MemberCardLevel.Gift.RewardRate.Value * 100),
                        c.Points,
                        Holiday = c.MemberCardLevel.Gift.PersonalGiftID == null ? false : c.MemberCardLevel.Gift.PersonalGift.Holiday,
                        Personal = c.MemberCardLevel.Gift.PersonalGiftID == null ? false : c.MemberCardLevel.Gift.PersonalGift.Personal,
                        SpecialDay = c.MemberCardLevel.Gift.PersonalGiftID != null && c.MemberCardLevel.Gift.PersonalGift.SpecialDay,
                        SpecialMemory = c.MemberCardLevel.Gift.SpecialMemory == null ? false : c.MemberCardLevel.Gift.SpecialMemory.AvailableTemplates,
                        CustomizeAvailableTemplate = c.MemberCardLevel.Gift.SpecialMemory == null ? false : c.MemberCardLevel.Gift.SpecialMemory.CustomizeAvailableTemplate,
                        c.MemberCardLevel.VIP,
                        Mocktail = (bool)c.MemberCardLevel.VIP ? c.MemberCardLevel.VIPGift.Moctail : false,
                        VipRoom = (bool)c.MemberCardLevel.VIP ? c.MemberCardLevel.VIPGift.VipRoom : false,
                        Total = c.MemberCardChargeRecords.Sum(i => i.Money),
                        isHaveOwner = c.Accounts.Any(),
                        Owner = c.Accounts.Any()? new
                        {
                            FullName = c.Accounts.Any() ? c.Accounts.FirstOrDefault().FullName : null,
                            DateOfBirth = c.Accounts.Any() ? c.Accounts.FirstOrDefault().DateOfBirth : null,
                            UserName = c.Accounts.Any() ? c.Accounts.FirstOrDefault().AccountName : null,
                            Email = c.Accounts.Any() ? c.Accounts.FirstOrDefault().Email : null,
                            Phone = c.Accounts.Any() ? c.Accounts.FirstOrDefault().Phone : null,
                        }:null,

                        Name = string.IsNullOrEmpty(c.Name) ? "" : c.Name.Trim(),
                        Phone = string.IsNullOrEmpty(c.Phone) ? "" : c.Phone.Trim(),
                        c.Balance,
                        c.Status
                    }).FirstOrDefault();
              

                if (memberCard == null)
                {
                    return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Thẻ không tồn tại, vui lòng thử lại!"

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
            string reader = Session["ReaderID"].ToString();
            var live = db.LiveCards.ToList();
            var currCard = db.LiveCards.Where(c => c.ReaderID.Trim().Equals(reader) && c.ScanAt >= oneMinuteAgo && c.ScanAt <= now).FirstOrDefault();
          

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
            if (memberCard.Status.Value)
            {
                return this.Json(
             new
             {
                 status = "Error",
                 message = "Thẻ đã được sử dụng bởi người khác, vui lòng quét lại!"

             }
             , JsonRequestBehavior.AllowGet
             );

            }
            if (memberCard.Accounts.Any())
            {
                    return Json(
               new
               {
                   status = "Error",
                   message = "Thẻ đã được sử dụng!"

               }
               , JsonRequestBehavior.AllowGet
               );
            }
            return this.Json(
                new
                {
                    status = "Success",
                    card = memberCard.Code39

                }
                , JsonRequestBehavior.AllowGet
                );
        }
        [HttpGet]
        public ActionResult ClearMemberCard()
        {
            string ReaderID = Session["ReaderID"].ToString();
            ViewBag.ReaderID = ReaderID;
            ViewBag.MemberCardList = db.MemberCards.ToList();
            return View();
        }
        [HttpPost]
        public JsonResult ClearMemberCard(string MemberCardID)
        {
            var card = db.MemberCards.Where(c => c.Code39.Equals(MemberCardID)).FirstOrDefault();
            if(string.IsNullOrEmpty(MemberCardID) && card == null)
            {
                return this.Json(
                new
                {
                    status = "Error",
                    message = "Thẻ không tồn tại!"

                }
                , JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                card.Balance = 0;
                card.Status = false;
                card.Points = 0;
                var oldRecord = db.MemberCardChargeRecords.Where(r => r.MemberCardID == card.MemberCardID).ToList();
                foreach (var r in oldRecord)
                {
                    db.MemberCardChargeRecords.Remove(r);
                }
                var oldGamehis = db.ReportGameHistories.Where(r => r.IdCard == card.MemberCardID).ToList();
                foreach (var g in oldGamehis)
                {
                    db.ReportGameHistories.Remove(g);
                }
            }
            return this.Json(
                new
                {
                    status = "Success",
                   

                }
                , JsonRequestBehavior.AllowGet
                );
            db.SaveChanges();
        }

        [HttpGet]
        public JsonResult GetCurrentCardForReissuance(string level)
        {
            DateTime now = DateTime.Now;
            DateTime oneMinuteAgo = now.AddMinutes(-2).AddSeconds(-now.Second);
            string reader = Session["ReaderID"].ToString();
            var live = db.LiveCards.ToList();
            var currCard = db.LiveCards.Where(c => c.ReaderID.Trim().Equals(reader) && c.ScanAt >= oneMinuteAgo && c.ScanAt <= now).FirstOrDefault();
          

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

            if (!memberCard.MemberCardLevel.CardLevel.ID.Trim().Equals(level.Trim()))
            {
                return this.Json(
             new
             {
                 status = "Error",
                 message = "Loại thẻ không đúng so với thẻ cũ, vui lòng quét lại!"

             }
             , JsonRequestBehavior.AllowGet
             );
            }
            if (memberCard.Status.Value)
            {
                return this.Json(
             new
             {
                 status = "Error",
                 message = "Thẻ đã được sử dụng bởi người khác, vui lòng quét lại!"

             }
             , JsonRequestBehavior.AllowGet
             );

            }
            if (memberCard.Accounts.Any())
            {
                    return Json(
               new
               {
                   status = "Error",
                   message = "Thẻ đã được sử dụng!"

               }
               , JsonRequestBehavior.AllowGet
               );
            }
            return this.Json(
                new
                {
                    status = "Success",
                    card = memberCard.Code39

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

      

        // POST: Admin/MemberCard/Delete/5
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic her
               var memberCard = db.MemberCards.Where(c => c.Code39.Trim().Equals(id)).FirstOrDefault();
               if(string.IsNullOrEmpty(id) || memberCard == null)
                {
                    return Json(
               new
               {
                   status = "Error",
                   message = "Lỗi thử lại!"

               }
               , JsonRequestBehavior.AllowGet
               );
            }


                return Json(
                new
                {
                    status = "Success",
                    message = "Lỗi thử lại!"

                }
                , JsonRequestBehavior.AllowGet
                );


            }
            catch
            {
                return Json(
               new
               {
                   status = "Error",
                   message = "Lỗi thử lại!"

               }
               , JsonRequestBehavior.AllowGet
               );
            }
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var UserID = (Session["UserID"].ToString());
                    var User = db.Users.Find(UserID);
                    // Xử lý file Excel tại đây (lưu vào thư mục, đọc dữ liệu, ...)
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        // Chọn sheet trong file (ví dụ chọn sheet đầu tiên)
                        var workSheet = package.Workbook.Worksheets[0];
                        List<MemberCard> memberCards = new List<MemberCard>();
                        // Đọc dữ liệu từ sheet và xử lý theo nhu cầu của bạn
                        for (int row = 2; row <= workSheet.Dimension.Rows; row++)
                        {

                           
                            var CardID = workSheet.Cells[row, 1].Text.Trim().Substring(1);
                            var Code39 = workSheet.Cells[row, 2].Text.Trim();
                            
                            var Type = workSheet.Cells[row, 3].Text;
                            if(string.IsNullOrEmpty(CardID) && string.IsNullOrEmpty(Type))
                            {
                                break;
                            }
                            if (string.IsNullOrEmpty(CardID) || string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Code39))
                            {
                                return Json(new { status = false, message = $"Thông tin ở dòng {row} không đầy đủ!" });
                            }
                            Type = Type.ToLower();
                            if (!db.MemberCardLevels.Any(l => l.CardLevel.LevelName.Trim().ToLower().Equals(Type))) 
                            {
                                return Json(new { status = false, message = $"Loại thẻ ở dòng {row} không tồn tại!" });
                            }
                            var memberLevel = db.MemberCardLevels.Where(l => l.CardLevel.LevelName.Trim().ToLower().Equals(Type)).FirstOrDefault();
                            var memberCard = new MemberCard
                            {
                                MemberCardID = CardID.Trim(),
                                Balance = 0,
                                Points = 0,
                                Status = false,
                                MemberCardLevelID = memberLevel.LevelID,
                                Code39 = Code39.Trim(),
                                CreateDate = DateTime.Now,
                                CreateBy = User.Name.Trim(),

                            };
                            memberCards.Add(memberCard);
                        }
                        db.MemberCards.AddRange(memberCards);
                        db.SaveChanges();
                    }

                    return Json(new { status = true });
                }

                return Json(new { status = false, message = "Không có file nào được thêm" });
            }catch (Exception e)
            {
                return Json(new { status = false, message = e });
            }
           
        }
        public ActionResult ExportToExcel()
        {
            var data = db.MemberCards.Select(m => new {
                m.Code39,
                CardLevel = m.MemberCardLevel.CardLevel.LevelName.Trim(),
                Status = m.Status.Value ? "Đã kích hoạt" : "Chưa kích hoạt",
                Owner = m.Accounts.Any() ? m.Accounts.FirstOrDefault().AccountName : "",
                

            }).ToList(); // Lấy dữ liệu từ cơ sở dữ liệu
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm tiêu đề cho các cột
                worksheet.Cells[1, 1].Value = "Mã thẻ";
                worksheet.Cells[1, 2].Value = "Cấp độ thẻ";
                worksheet.Cells[1, 3].Value = "Trạng thái";
                worksheet.Cells[1, 4].Value = "Sở hữu";
                // Thêm dữ liệu từ data vào các ô
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Code39;
                    worksheet.Cells[i + 2, 2].Value = data[i].CardLevel;
                    worksheet.Cells[i + 2, 3].Value = data[i].Status;
                    worksheet.Cells[i + 2, 4].Value = data[i].Owner;
                }
                var headerCells = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns];
                worksheet.View.FreezePanes(2, 1);
                // Set their text to bold, italic and underline.
                headerCells.Style.Font.Bold = true;
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                worksheet.Cells["A:AZ"].AutoFitColumns();
                var range = worksheet.Cells[worksheet.Dimension.Address];
                range.AutoFilter = true;
                ///Setting thêm cho sheet detail
                //Select only the header cells
                // Lưu package thành file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "membercard-data.xlsx");
            }
        }
    }
}
