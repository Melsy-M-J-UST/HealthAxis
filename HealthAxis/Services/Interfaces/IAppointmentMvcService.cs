using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IAppointmentMvcService
    {
        bool Book(AppointmentDto dto, out string errorMessage);

        IEnumerable<AppointmentDto> GetPatientAppointments(int patientId);

        IEnumerable<AppointmentDto> GetDoctorAppointments(int doctorId);

        bool UpdateStatus(AppointmentDto dto, out string errorMessage);

        bool DeleteAppointment(int id, out string errorMessage);
    }
}