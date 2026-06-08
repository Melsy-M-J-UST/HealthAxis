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
        bool IsSlotAvailable(int doctorId, DateTime date, string timeSlot);
        Appointment Add(Appointment appointment);
        bool UpdateStatus(int appointmentId, string status, string cancellationReason);
        bool Delete(int id);
    }
}