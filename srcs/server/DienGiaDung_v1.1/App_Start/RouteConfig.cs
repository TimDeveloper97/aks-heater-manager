using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", view = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Context",
                url: "{controller}/{action}/{view}/{id}/{act}",
                defaults: new { 
                    view = UrlParameter.Optional, 
                    id = UrlParameter.Optional,
                    act = UrlParameter.Optional,
                }
            );
        }
    }
}
