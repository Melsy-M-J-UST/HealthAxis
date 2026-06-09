using HealthAxisMVC.Repositories;
using HealthAxisMVC.Repositories.Impl;
using HealthAxisMVC.Services;
using HealthAxisMVC.Services.Impl;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthAxisMVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IAppointmentService, AppointmentService>();


            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}