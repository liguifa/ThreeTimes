using SurveyManage.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyManage
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication, IHttpModule
    {
        static SessionController sc = new SessionController();
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public void Init(System.Web.HttpApplication context)
        {
            //context.AcquireRequestState += new EventHandler(LoginCheck);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {

            HttpApplication app = sender as HttpApplication;
            string[] url = app.Request.Url.ToString().Split('/');
            AdminController.sc = sc;
            StaffController.sc = sc;
            HRController.sc = sc;
            if (!(url.Length >= 5 && (url[4] == "Login" || url[4] == "LoginIn" || url[4] == "VerCode" || url[4] == "requestData")))
            {
                //TODO  loginChecker;
                if (sc.GetAdminSession() == null && sc.GetHrSession() == null && sc.GetStaffSession() == null)
                {

                    //Context.RewritePath();
                    Context.Response.Redirect("/Staff/Login");
                }
                else if (sc.GetStaffSession() != null && url[3] != "Staff")
                {
                    Context.Response.Redirect("/Staff/Login");
                }
                else if (sc.GetAdminSession() != null && url[3] != "Admin" && url[3] != "Company" && url[3] != "Questions")
                {
                    Context.Response.Redirect("/Staff/Login");
                }
                else if (sc.GetHrSession() != null && url[3] != "Hr")
                {
                    Context.Response.Redirect("/Staff/Login");
                }
                else if (url[3] == "Session")
                {
                    Context.Response.Redirect("/Staff/Login");
                }
            }
        }


    }
}