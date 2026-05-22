using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service.Implementation
{
    public class AppointmentService: IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            this._repository = repository;
        }

        public Appointment BookAppointment(Appointment newAppointment)
        {
            if (newAppointment.ScheduledDate.Date <= DateTime.Today)
            {
                throw new AppointmentNotFoundException("Appointment must be scheduled in the future.");
            }

            var existingAppointments = _repository.GetAllAppointments();

            bool isSlotTaken = existingAppointments.Any(app =>
                app.Doctor.DoctorId == newAppointment.Doctor.DoctorId &&
                app.ScheduledDate.Date == newAppointment.ScheduledDate.Date &&
                app.Slot == newAppointment.Slot &&
                app.Status != Appointment.AppointmentStatus.Cancelled
            );

            if (isSlotTaken)
            {
                throw new AppointmentNotFoundException("This slot is already booked for the doctor.");
            }

            return _repository.BookAppointment(newAppointment);
        }

        public bool CancelAppointment(int appointmentId, string reason)
        {
            return _repository.CancelAppointment(appointmentId, reason);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _repository.GetAllAppointments();
        }

        public Appointment? GetAppointmentById(int appointmentId)
        {
            return _repository.GetAppointmentById(appointmentId);
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            return _repository.GetAppointmentsByDoctor(doctorId);
        }

        public List<Appointment> GetAppointmentsBypatient(int patientId)
        {
            return _repository.GetAppointmentsByPatient(patientId);
        }
    }
}
