using JPGame.Areas.Admin.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
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
    }

}