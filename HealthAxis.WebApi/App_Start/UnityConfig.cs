using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services;
using HealthAxis.Api.Services.Interfaces;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HealthAxis.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<HealthAxisEntities>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();
            container.RegisterType<IAuthService, AuthService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
