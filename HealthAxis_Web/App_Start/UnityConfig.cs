using System.Web.Http;
using Unity;
using Unity.WebApi;
using HealthAxis_Web.Database;
using HealthAxis_MVC.Repositories;
using HealthAxis_MVC.Repositories.Impl;
using HealthAxis_MVC.Services;
using HealthAxis_MVC.Services.Impl;
using AutoMapper;
using HealthAxis_Web.App_Start;

namespace HealthAxis_Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<AppDBContext>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IDoctorService, DoctorServiceImpl>();
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            container.RegisterInstance<IMapper>(mapper);

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}