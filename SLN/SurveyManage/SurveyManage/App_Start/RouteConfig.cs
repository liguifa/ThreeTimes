using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyManage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Staff", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Scan",
                url: "{controller}/{action}/{id}/{CID}",
                defaults: new { controller = "Company", action = "ScanTest", id = UrlParameter.Optional, CID = UrlParameter.Optional }
            );
        }
    }
}