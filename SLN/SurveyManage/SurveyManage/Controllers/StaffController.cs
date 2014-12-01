using Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Common.Models;

namespace SurveyManage.Controllers
{
    public class StaffController : Controller
    {
        //
        // GET: /Staff/
        public static SessionController sc = null;
        public ActionResult Index(string id)
        {
            long ID=1;
            try
            {
                ID = Convert.ToInt64(id);
            }
            catch 
            {
                ID = 1;
            }
            ID = ID > 0 ? ID : 1;
            int pageSize = 20;
            staff sf = new StaffHanding().GetStaffMsg(Convert.ToInt64(sc.GetStaffSession()));
            testInfo ti = new TestInfoOperation().GetTestInfo(sf.staff_OwnCompanyId)[0];
            List<test> tests = new TestHanding().GetTests(ti.testInfo_Id, (int)ID, pageSize);
            ViewBag.tests = tests;
            ViewBag.title = ti.testInfo_Name;
            ViewBag.dataSize = new TestHanding().GetTestCount(Convert.ToInt32(ti.testInfo_Id));
            int maxPage=new TestHanding().GetMaxPage(pageSize, Convert.ToInt32(ti.testInfo_Id));;
            ViewBag.maxDataSize = maxPage;
            ViewBag.index = ID<maxPage?ID:maxPage;
            ViewBag.staffId = sf.staff_Id;
            ViewBag.cookies = sf.staff_Spare;
            ViewBag.msg = sf;
            ViewBag.companyName = new Companyhandling().GetOwnCompanyName(sf.staff_OwnCompanyId);
            return sf.staff_Password==Md5.GetMd5Word("123456")?View("/Views/Staff/sureMsg.cshtml"):View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public string LoginIn(string user, string pwd,string yzm)
        {
            List<object> result = new StaffHanding().Login(user, pwd, yzm, (string)Session["ver"]);
            if (result.Contains(1))
            {
                sc.ClearSession();
                sc.SetStaffSession(result[2].ToString());
            }

            return result.ToJson() ;
        }

        public string ModifyPassword(string name,string email,string pwd)
        {
            return new StaffHanding().ModifyPassword(Convert.ToInt64(sc.GetStaffSession()),name,email,pwd).ToJson();
        }

        public void RecordAnswers(string datas)
        {
            new StaffHanding().RecordAnswers(Convert.ToInt64(sc.GetStaffSession()),datas);
        }
        

        public string SubmitKey(string key)
        {
            staff sf = new StaffHanding().GetStaffMsg(Convert.ToInt64(sc.GetStaffSession()));
            testInfo ti = new TestInfoOperation().GetTestInfo(sf.staff_OwnCompanyId)[0];
            List<object> res = new KeyOperation().SubmitKey(key, Convert.ToInt64(sc.GetStaffSession()), ti.testInfo_Id);
            if (res.Contains(1))
            {
                sc.ClearSession();
            }
            return res.ToJson();
        }

        public ActionResult VerCode()
        {
            VerificationCode vc = new VerificationCode();
            Session["ver"] = vc.GetVerCode();
            return File(vc.StreamVerCod(), @"image/jpeg");
        }

    }
}
