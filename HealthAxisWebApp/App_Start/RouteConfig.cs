using System.Web.Mvc;
using System.Web.Routing;

namespace HealthAxisWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 1. ADD THIS: Custom route to allow the plural "/Patients" URL
            routes.MapRoute(
                name: "Patients",
                url: "Patients/{action}/{id}",
                defaults: new { controller = "Patient", action = "Index", id = UrlParameter.Optional }
            );

            // 2. MODIFY THIS: Change the default controller from "Home" to "Patient"
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Patient", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}