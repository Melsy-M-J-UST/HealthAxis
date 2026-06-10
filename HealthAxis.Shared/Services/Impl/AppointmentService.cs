using System;
using System.Collections.Generic;
using System.Linq;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxis.Shared.Models;

namespace HealthAxis.Shared.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private const string AppointmentNotFoundMessage = "Appointment not found.";
        private const string DoctorNotFoundMessage = "Doctor not found.";

        private readonly IAppointmentRepository appointmentRepository;
        private readonly IDoctorRepository doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.doctorRepository = doctorRepository;
        }

        public List<Appointment> GetAllAppointments()
        {
            return appointmentRepository.GetAll();
        }

        public Appointment GetAppointmentById(int id)
        {
            return appointmentRepository.GetById(id);
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return appointmentRepository.GetByPatientId(patientId);
        }

        public List<Appointment> GetTodayAppointments(int doctorId)
        {
            return appointmentRepository.GetTodayAppointments(doctorId);
        }

        // ✅ NEW: Doctor Weekly Schedule
        public List<Appointment> GetWeeklyAppointments(int doctorId)
        {
            return appointmentRepository.GetWeeklyAppointments(doctorId);
        }

        // ✅ ADD
        public void AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            ValidateAppointment(appointment);

            appointment.Status = 0; // Pending

            if (appointment.CancellationReason == null)
            {
                appointment.CancellationReason = string.Empty;
            }

            appointmentRepository.Add(appointment);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            ValidateAppointment(appointment);

            appointmentRepository.Update(appointment);
        }

        public void ConfirmAppointment(int id)
        {
            var appointment = appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException(AppointmentNotFoundMessage);
            }

            if (appointment.Status != 0)
            {
                throw new InvalidOperationException(
                    "Only pending appointments can be confirmed.");
            }

            appointment.Status = 1;
            appointmentRepository.Update(appointment);
        }

        // ✅ COMPLETE
        public void CompleteAppointment(int id)
        {
            var appointment = appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException(AppointmentNotFoundMessage);
            }

            if (appointment.Status != 1)
            {
                throw new InvalidOperationException(
                    "Only confirmed appointments can be completed.");
            }

            appointment.Status = 3;
            appointmentRepository.Update(appointment);
        }

        // ✅ CANCEL
        public void CancelAppointment(int id, string reason)
        {
            var appointment = appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException(AppointmentNotFoundMessage);
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException(
                    "Cancellation reason is required.", nameof(reason));
            }

            if (appointment.Status == 3)
            {
                throw new InvalidOperationException(
                    "Completed appointments cannot be cancelled.");
            }

            if (appointment.Status == 2)
            {
                throw new InvalidOperationException(
                    "Appointment is already cancelled.");
            }

            appointment.Status = 2; // Cancelled
            appointment.CancellationReason = reason;

            appointmentRepository.Update(appointment);
        }

        // ✅ DELETE
        public void DeleteAppointment(int id)
        {
            var appointment = appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException(AppointmentNotFoundMessage);
            }

            appointmentRepository.Delete(id);
        }

        // ✅ VALIDATION (UPDATED)
        private void ValidateAppointment(Appointment appointment)
        {
            if (appointment.PatientId <= 0)
            {
                throw new ArgumentException("Invalid Patient.");
            }

            if (appointment.DoctorId <= 0)
            {
                throw new ArgumentException("Invalid Doctor.");
            }

            if (appointment.ScheduledDate.Date < DateTime.Today)
            {
                throw new ArgumentException("Past dates are not allowed.");
            }

            var doctor = doctorRepository.GetById(appointment.DoctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException(DoctorNotFoundMessage);
            }

            if (!doctor.IsActive)
            {
                throw new InvalidOperationException("Doctor is inactive.");
            }

            // ✅ IMPROVED SLOT CHECK
            bool isAvailable = appointmentRepository
                .IsSlotAvailable(
                    appointment.DoctorId,
                    appointment.ScheduledDate,
                    appointment.TimeSlot
                );

            if (!isAvailable)
            {
                throw new InvalidOperationException(
                    "Selected time slot is already booked.");
            }
        }
    }
}