using HealthAxis.Mvc.Services;
using HealthAxis.Mvc.Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthAxis.Mvc
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IAuthMvcService, AuthMvcService>();
            container.RegisterType<IPatientMvcService, PatientMvcService>();
            container.RegisterType<IDoctorMvcService, DoctorMvcService>();
            container.RegisterType<IAppointmentMvcService, AppointmentMvcService>();
            container.RegisterType<IHealthRecordMvcService, HealthRecordMvcService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
