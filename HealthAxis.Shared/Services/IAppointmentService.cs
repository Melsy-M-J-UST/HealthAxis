using HealthAxis.Shared.Models;
using HealthAxisWebApp;
using System.Collections.Generic;

namespace HealthAxis.Shared.Services.Interfaces
{
    public interface IAppointmentService
    {
        List<Appointment> GetAllAppointments();

        Appointment GetAppointmentById(int id);

        void AddAppointment(Appointment appointment);

        void UpdateAppointment(Appointment appointment);

        void CancelAppointment(int id, string reason);
        void ConfirmAppointment(int id);

        void CompleteAppointment(int id);

        void DeleteAppointment(int id);
    }
}
