using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MediaRadarExercise
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "AdData",
                "AdData/{startMonth}-{endMonth}",
                new { controller = "AdData", action = "Index" },
                new { startMonth = @"\d+", endMonth = @"\d+" }
             );

            routes.MapRoute(
                "AdData-Filter",
                "AdData/{action}/{startMonth}-{endMonth}",
                new { controller = "AdData", action = "Index" },
                new { startMonth = @"\d+", endMonth = @"\d+" }
             );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }
    }
}