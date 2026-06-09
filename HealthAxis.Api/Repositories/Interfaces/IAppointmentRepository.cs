using HealthAxis.Api.Data;
using System;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAll();

        Appointment GetById(int id);

        IEnumerable<Appointment> GetByPatient(int patientId);

        IEnumerable<Appointment> GetByDoctor(int doctorId);

        IEnumerable<Appointment> GetByDoctorAndDate(int doctorId, DateTime date);

        IEnumerable<Appointment> GetByDoctorAndDateRange(
            int doctorId,
            DateTime start,
            DateTime end);

        bool IsSlotAvailable(
            int doctorId,
            DateTime date,
            string slot);

        Appointment Add(Appointment appointment);

        bool UpdateStatus(
            int id,
            string status,
            string reason);

        bool Delete(int id);
    }
}