using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPGame;
namespace JPGame.Controllers
{
    public class LoginController : Controller
    {
        private DBEntities db = new DBEntities(); 
        // GET: Login
        public ActionResult SignUp()
        {
            return View();
        }
    }
}