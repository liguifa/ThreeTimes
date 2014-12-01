using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyManage.Controllers
{
    public class SessionController : Controller
    {
        //
        // GET: /Session/


        public SessionController GetSession()
        {
            return this;
        }
        public void SetAdminSession(string obj)
        {
            //if (Session["id"] == null)
            //{
            //    Session["id"] = 1;
            //}
            //else
            //{
            //    string s = Session["id"].ToString();
            //}
            if(System.Web.HttpContext.Current.Session["adminId"]==null){
                System.Web.HttpContext.Current.Session["adminId"] = obj;
            }
            else
            {
                string s = System.Web.HttpContext.Current.Session["adminId"].ToString();
            }
            
        }

        public void SetHrSession(string obj)
        {
            System.Web.HttpContext.Current.Session["hr"] = obj;
        }

        public void SetStaffSession(string obj)
        {
            System.Web.HttpContext.Current.Session["staff"] = obj;
        }

        public object GetAdminSession()
        {
            try
            {
                return System.Web.HttpContext.Current.Session["adminId"];
            }
            catch
            {
                return null;
            }
        }

        public string GetHrSession()
        {
            try
            {
                return (string)System.Web.HttpContext.Current.Session["hr"];
            }
            catch
            {
                return null;
            }
        }

        public string GetStaffSession()
        {
            try
            {
                return (string)System.Web.HttpContext.Current.Session["staff"];
            }
            catch
            {
                return null;
            }
        }

        public void ClearSession()
        {
            System.Web.HttpContext.Current.Session.Clear();
        }

    }
}
