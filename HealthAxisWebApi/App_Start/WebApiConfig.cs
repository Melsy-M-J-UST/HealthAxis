using System.Web.Http;
using Newtonsoft.Json;

namespace HealthAxisWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Return JSON instead of XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Prevent EF circular reference JSON errors
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
                ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}