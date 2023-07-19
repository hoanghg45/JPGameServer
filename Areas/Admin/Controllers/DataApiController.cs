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

        // POST api/<controller>
        public void Post([FromBody] string value)
        {

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