using HealthAxis.Shared.DTOs;
using System;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IAppointmentMvcService
    {
        bool Book(
            AppointmentDto dto,
            out string error);

        IEnumerable<AppointmentDto> GetByPatient(int patientId);

        IEnumerable<AppointmentDto> GetByDoctor(int doctorId);

        IEnumerable<AppointmentDto> Today(int doctorId);

        IEnumerable<AppointmentDto> Weekly(
            int doctorId,
            DateTime startDate);

        bool UpdateStatus(
            int id,
            AppointmentStatusUpdateDto dto,
            out string error);
    }
}