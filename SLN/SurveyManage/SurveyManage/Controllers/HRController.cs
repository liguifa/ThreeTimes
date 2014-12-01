using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Models;
using Verification;

namespace SurveyManage.Controllers
{
    public class HRController : Controller
    {
        //
        // GET: /HR/
        public static SessionController sc = null;
        public ActionResult Index()
        {
            admin a = new Admin().GetAdminMsg(Convert.ToInt64(sc.GetHrSession()));
            ViewBag.CompanyName = a.company.company_Name;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult sureMsg()
        {
            long id = Convert.ToInt64(sc.GetHrSession());
            admin a = new Admin().GetAdminMsg(id);
            ViewBag.name = a.admin_TrueName;
            ViewBag.companyName = a.company.company_Name;
            ViewBag.phone = a.admin_PhoneNum;
            ViewBag.email = a.admin_Email;
            return View();
        }

        public string LoginIn(string user, string pwd,string yzm)
        {
            List<object> res = new Admin().HrLogin(user, pwd,yzm,(string)Session["ver"]);
            if (res.Contains(1))
            {
                sc.ClearSession();
                sc.SetHrSession(res[1].ToString());
                res.Remove(res[1]);
            }
            return res.ToJson(); ;
        }

        public string GetTreeGridJson()
        {

            Companyhandling ch = new Companyhandling();
            admin a = new Admin().GetAdminMsg(Convert.ToInt64(sc.GetHrSession()));
            List<company> group = ch.SearchSonCompany(a.admin_CompanyID);
            
            return group.ToTreeGrid();
        }
        public string InspectionProcessTree(string id)
        {
            admin a = new Admin().GetAdminMsg(Convert.ToInt64(sc.GetHrSession()));
            return new Companyhandling().GetCompanyInspectionProcess(a.company.company_Id);
        }

        public ActionResult VerCode()
        {
            VerificationCode vc = new VerificationCode();
            Session["ver"] = vc.GetVerCode();
            return File(vc.StreamVerCod(), @"image/jpeg");
        }

    }
}
