using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ToDoWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Tasks", action = "List" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "Tasks", action = "List" }
            //);

            routes.MapRoute(
                name: "DeleteEdit",
                url: "{controller}/{action}/{TaskID}",
                defaults: new { controller = "Tasks", action = "Edit", TaskID = -1 }
            );
        }
    }
}
