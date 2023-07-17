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
        public JsonResult Get(string card)
        {
            try {
                var Card = new Card { CardID = card };
                db.Cards.Add(Card);
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