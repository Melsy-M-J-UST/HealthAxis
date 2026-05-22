using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public Appointment BookAppointment(Appointment newAppointment)
        {
            if (newAppointment.ScheduledDate <= DateTime.Now)
                throw new AppointmentConflictException("Date must be future");

            var existing = _repo.GetAllAppointments();

            bool isTaken = existing.Any(a =>
                a.Doctor.DoctorId == newAppointment.Doctor.DoctorId &&
                a.ScheduledDate.Date == newAppointment.ScheduledDate.Date &&
                a.Slot == newAppointment.Slot &&
                a.Status != Appointment.AppointmentStatus.Cancelled
            );

            if (isTaken)
                throw new AppointmentConflictException("Slot already booked");

            return _repo.BookAppointment(
                newAppointment.Patient,
                newAppointment.Doctor,
                newAppointment.ScheduledDate,
                newAppointment.Slot
            );
        }

        public bool CancelAppointment(int appointmentId, string reason)
        {
            return _repo.CancelAppointment(appointmentId, reason);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _repo.GetAllAppointments();
        }

        public Appointment? GetAppointmentById(int appointmentId)
        {
            return _repo.GetAppointmentById(appointmentId);
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            return _repo.GetAppointmentsByDoctor(doctorId);
        }

        public List<Appointment> GetAppointmentsBypatient(int patientId)
        {
            return _repo.GetAppointmentsByPatient(patientId);
        }
    }
}