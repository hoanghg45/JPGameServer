using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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
        readonly string reader = "ef7f36c1fc1d31c7";
        // GET: Admin/MemberCard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DataTable(int page = 0, string search ="")
        {
            var data = db.MemberCards.Select(a => new
            {
                a.MemberCardID,
                Level = a.MemberCardLevel.CardLevel.LevelName.Trim(),
                Owner = a.Accounts.Any()? a.Accounts.FirstOrDefault().AccountName.Trim() : "",
                Status = a.Status.HasValue && a.Status == true,
                CreateAt= a.CreateDate
                
            });

            data = data.WhereIf(!string.IsNullOrEmpty(search), a => a.MemberCardID.Contains(search));

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
                if (!card.MemberCardLevel.CardLevel.ID.Trim().Equals("level1"))
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
                var currUser = (Session["UserID"].ToString());
                string OldCardID = collection["CurrCardID"];
                string NewCardID = collection["NewCardID"];
                var MemberCardLevel = db.MemberCards.Find(collection["MemberCardLevelID"]);
                var oldCard = db.MemberCards.Find(OldCardID);
                var newCard = db.MemberCards.Find(NewCardID);
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
                var FirstLevel = db.CardLevels.Where(l => l.ID.Trim().Equals("level1")).FirstOrDefault();
                //Nếu không thay đổi cấp độ
                if (oldCard.MemberCardLevel.CardLevel.LevelName.Equals(newCard.MemberCardLevel.CardLevel.LevelName))
                {
                    //if(oldCard.Balance < FirstLevel.LevelFee || newCard.Balance )
                    oldCard.Balance = FinalMoney;
                    oldCard.Points = FinalPoint;
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

                    //Hủy thẻ cũ
                    oldCard.Status = false;
                    //Chuyển tiền sang thẻ mới
                    newCard.Balance = FinalMoney;
                    newCard.Points = FinalPoint;
                    newCard.Status = true;
                    newCard.CreateDate = DateTime.Now;
                 
                    //Nếu thẻ welcome được nâng cấp
                    if (oldCard.MemberCardLevel.CardLevel.LevelName.Trim().Equals("Welcome"))
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
                        acc.MemberCardID = newCard.MemberCardID;
                    }
                    else
                    {
                        var acc = oldCard.Accounts.FirstOrDefault();
                        if (acc == null)
                        {
                            return this.Json(
                          new
                          {
                              status = "Error",
                              message = "Tài khoản sở hữu thẻ cũ không tồn tại!"

                          }
                          , JsonRequestBehavior.AllowGet
                          );
                        }
                        acc.MemberCardID = newCard.MemberCardID;
                    }


                }
                //Lưu thời gian nạp tiền
                var chargeRecord = new MemberCardChargeRecord
                {
                    MemberCardID = newCard.MemberCardID,
                    Money = ChargeMoney,
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

                           
                            var CardID = workSheet.Cells[row, 2].Text;
                            var Type = workSheet.Cells[row, 4].Text;
                            if (string.IsNullOrEmpty(CardID) || string.IsNullOrEmpty(Type))
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
    }
}
