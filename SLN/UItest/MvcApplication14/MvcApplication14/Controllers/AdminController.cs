using MvcApplication14;
using MvcApplication14.CS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication14.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 请求公司列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PartChart(int pageIndex)
        {

            Company com = new Company();
            com.ID = "第" +pageIndex+ "页";
            com.phone = "1";
            com.username = "哈哈";
            com.phone = "11111";
            ViewBag.c = new List<Company>() { com };
            
            return PartialView();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Part(int pageIndex)
        {
            return PartialView();
        }
    }
}
