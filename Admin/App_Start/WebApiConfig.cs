using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Admin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "FullResult",
               routeTemplate: "api/{controller}/{action}/{testID}/{userID}"
           );

           // config.Routes.MapHttpRoute(
           //    name: "DeleteImage",
           //    routeTemplate: "api/{controller}/{action}/{path}"
           //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
