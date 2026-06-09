using System;
using System.Web.Http;
using WebActivatorEx;
using HealthAxisWebApi;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace HealthAxisWebApi
{
    public class SwaggerConfig
    {
        private static readonly object LockObject = new object();
        private static bool isRegistered;

        public static void Register()
        {
            lock (LockObject)
            {
                if (isRegistered)
                {
                    return;
                }

                isRegistered = true;

                GlobalConfiguration.Configuration
                    .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "HealthAxisWebApi");

                        // If duplicate action/path issues appear later, uncomment this:
                        // c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                    .EnableSwaggerUi(c =>
                    {
                        // Optional Swagger UI customizations can go here.
                    });
            }
        }
    }
}