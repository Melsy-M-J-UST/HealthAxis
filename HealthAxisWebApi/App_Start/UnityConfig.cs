using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Impl;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxisWebApp;
using HealthAxisWebApp.Repositories;
using HealthAxisWebApp.Repositories.Interfaces;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace HealthAxisWebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // EF DbContext
            container.RegisterType<HealthAxisDBEntities>(new HierarchicalLifetimeManager());

            // Repositories
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();

            // Services
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            GlobalConfiguration.Configuration.DependencyResolver =
                new UnityDependencyResolver(container);
        }
    }
}