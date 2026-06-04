using HealthCareWebApp.Data;
using HealthCareWebApp.Repository;
using HealthCareWebApp.Repository.Implementation;
using HealthCareWebApp.Service;
using HealthCareWebApp.Service.Implementation;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthCareWebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<Database>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}