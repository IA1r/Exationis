using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exationis
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Account",
                url: "UserProfile/{id}",
                defaults: new { controller = "Account", action = "UserProfile", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "SignOut",
                url: "SignOut",
                defaults: new { controller = "Account", action = "SignOut"}
             );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Faculty", id = UrlParameter.Optional }
            );

        }
    }
}