using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Forum_v1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "SubSort",
            url: "{controller}/{action}/{id}/{page}/{sortare}",
            defaults: new { controller = "Subjects", action = "Index" }
            );


            routes.MapRoute(
            name: "SubPaginat",
            url: "{controller}/{action}/{id}/{page}",
            defaults: new { controller = "Subjects", action = "Index" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
