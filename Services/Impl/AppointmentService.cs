using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using HAP_Pod4_ConsoleApp_au.Services;
using HAP_Pod4_ConsoleApp_au.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HAP_Pod4_ConsoleApp_au.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            this._repo = repo;
        }

        public Appointment BookAppointment(Appointment newAppointment)
        {
            if (newAppointment.ScheduledDate.Date <= DateTime.Today)
            {
                throw new AppointmentConflictException("Appointment must be scheduled in the future.");
            }

            var existingAppointments = _repo.GetAllAppointments();

            bool isSlotTaken = existingAppointments.Any(app =>
                app.Doctor.DoctorId == newAppointment.Doctor.DoctorId &&
                app.ScheduledDate.Date == newAppointment.ScheduledDate.Date &&
                app.TimeSlot == newAppointment.TimeSlot &&
                app.Status != Appointment.StatusOption.Cancelled
            );

            if (isSlotTaken)
            {
                throw new AppointmentConflictException("This slot is already booked for the doctor.");
            }

            return _repo.BookAppointment(newAppointment);
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
