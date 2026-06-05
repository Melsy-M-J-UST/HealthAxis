using HealthAxis_MVC.Database;
using HealthAxis_MVC.Repositories;
using HealthAxis_MVC.Services;
using HealthAxis_MVC.Repositories.Impl;
using HealthAxis_MVC.Services.Impl;
using System.Web.Mvc;
using System;
using Unity;
using Unity.Mvc5;

namespace HealthAxis_MVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<AppContextDB>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IDoctorService , DoctorService>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IPatientService, PatientService>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}