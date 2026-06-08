using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        IEnumerable<AppointmentDto> GetAll();
        IEnumerable<AppointmentDto> GetByPatient(int patientId);
        IEnumerable<AppointmentDto> GetByDoctor(int doctorId);
        bool Book(AppointmentDto dto, out string errorMessage);
        bool UpdateStatus(AppointmentDto dto, out string errorMessage);
        bool Delete(int id);
    }
}
