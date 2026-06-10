using HealthAxis.Shared.Models;
using System;
using System.Collections.Generic;

namespace HealthAxisWebApp.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();

        Appointment GetById(int id);

        void Add(Appointment appointment);

        void Update(Appointment appointment);

        void Delete(int id);

        List<Appointment> GetByPatientId(int patientId);

        List<Appointment> GetTodayAppointments(int doctorId);

        List<Appointment> GetWeeklyAppointments(int doctorId);

        bool IsSlotAvailable(int doctorId, DateTime date, int timeSlot);

    }
}
