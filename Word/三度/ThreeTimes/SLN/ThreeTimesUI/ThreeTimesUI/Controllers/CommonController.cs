using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThreeTimesUI.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        public ActionResult Login()
        {

            return View();
        }
        public string LoginCheck(string username, string password, string yzm)
        {

            return "{\"res\":\"ok\"}";
        }
        public ActionResult sureMsg()
        {

            return View();
        }
    }
}
