using HealthAxis.Shared.DTOs;
using System;
using System.Collections.Generic;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        IEnumerable<AppointmentDto> GetAll();

        IEnumerable<AppointmentDto> GetByPatient(int patientId);

        IEnumerable<AppointmentDto> GetByDoctor(int doctorId);

        IEnumerable<AppointmentDto> GetTodaySchedule(int doctorId);

        IEnumerable<AppointmentDto> GetWeeklySchedule(
            int doctorId,
            DateTime startDate);

        bool Book(
            AppointmentDto dto,
            out string errorMessage);

        bool UpdateStatus(
            int id,
            AppointmentStatusUpdateDto dto,
            out string errorMessage);

        bool Delete(int id);
    }
}