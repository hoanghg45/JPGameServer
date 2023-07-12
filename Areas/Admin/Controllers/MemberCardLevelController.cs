using JPGame.Areas.Admin.Extension;
using JPGame.Areas.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    [SessionCheck]
    public class MemberCardLevelController : Controller
    {
        // GET: Admin/MemberCardLevel

        DBEntities db = new DBEntities();
        // GET: Admin/AccountsAdmin
        public ActionResult Index()
        {
            var levels = db.CardLevels.ToList();
            ViewBag.levels = levels;
            var gifts = db.Gifts.ToList();
            ViewBag.gifts = gifts;
            var vip = db.VIPGifts.FirstOrDefault();
            ViewBag.vip = vip;
            return View();
        }
        [HttpGet]
        public JsonResult DataTable(int page = 0)
        {
            var data = db.MemberCardLevels.Select(a => new
            {
                a.LevelID,
                CardLevel = a.CardLevel.LevelName.Trim(),
                a.CardLevel.LevelFee,
                GiftLevelName = a.Gift.GiftLevelName.Trim(),
                Vipzone = a.VIP.Value?"Có":"Không",
                
            });


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
            data = data.OrderByDescending(d => d.LevelID).Skip(start).Take(pageSize);

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
        
        public JsonResult GetLevelFee(string LevelID)
        {
            try
            {
                if (string.IsNullOrEmpty(LevelID) || !db.CardLevels.Any(l => l.ID.Equals(LevelID)))
                {
                    return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Level không tồn tại vui lòng thử lại"

                    }
                    , JsonRequestBehavior.AllowGet
                    );
                }
                var level = db.CardLevels.Find(LevelID);
                var cardlevel = new
                {
                    level.ID,
                    level.LevelFee,
                    //level.MemberCardLevels,
                    level.Color
                };
                    return this.Json(
                new
                {
                    status = "Success",
                    cardlevel = cardlevel

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
                message =e.InnerException

              }
              , JsonRequestBehavior.AllowGet
              );
                }
        }
        [HttpGet]
        public JsonResult GetVipGifts()
        {
            try
            {
               
                var vip = db.VIPGifts.FirstOrDefault();
                var vipgift = new
                {
                    vip.VIPGiftID,
                    vip.Moctail,
                    vip.VipRoom,
                    vip.Discount
                };
                    return this.Json(
                new
                {
                    status = "Success",
                    vipgift

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
                message =e.InnerException

              }
              , JsonRequestBehavior.AllowGet
              );
                }
        }
        public JsonResult GetGiftInformation(string GiftID)
        {
            try
            {
                if (string.IsNullOrEmpty(GiftID) || !db.Gifts.Any(l => l.ID.Equals(GiftID)))
                {
                    return this.Json(
                    new
                    {
                        status = "Error",
                        message = "Quà tặng không tồn tại vui lòng thử lại"

                    }
                    , JsonRequestBehavior.AllowGet
                    );
                }
                var Gift = db.Gifts.Find(GiftID);
                var gift = new
                {
                    Gift.ID,
                    Gift.PointPlus,
                    RewardRate = Math.Round(Gift.RewardRate.Value * 100),
                    Personal = Gift.PersonalGift != null && Gift.PersonalGift.Personal.Value,
                    Holiday = Gift.PersonalGift != null && Gift.PersonalGift.Holiday.Value,
                    Special = Gift.PersonalGift != null && Gift.PersonalGift.SpecialDay,
                    AvailableTemplates = Gift.SpecialMemory != null && Gift.SpecialMemory.AvailableTemplates.Value,
                    CustomizeAvailableTemplate = Gift.SpecialMemory != null && Gift.SpecialMemory.CustomizeAvailableTemplate.Value,

                };
                return this.Json(
            new
            {
                status = "Success",
                gift

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
              message = e.InnerException

          }
          , JsonRequestBehavior.AllowGet
          );
            }
        }
    [HttpPost]
    public JsonResult AddMemberCard(FormCollection collection)
        {
            try
            {

                bool vip = !string.IsNullOrEmpty(collection["CheckVIP"]) && collection["CheckVIP"].Equals("on");
                var MemberCard = new MemberCardLevel
                {
                    CardLevelID = collection["CardLevel"],
                    GiftLevelID = collection["Gift"],
                    VIP = vip,
                };
                db.MemberCardLevels.Add(MemberCard);
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
                message =e.InnerException

              }
              , JsonRequestBehavior.AllowGet
              );
                }
        }


    }

}