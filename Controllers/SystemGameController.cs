using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Controllers
{
    public class SystemGameController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: SystemGame
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult StartGame(string res)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<string>(res);
                return Json(new { code = 200, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}