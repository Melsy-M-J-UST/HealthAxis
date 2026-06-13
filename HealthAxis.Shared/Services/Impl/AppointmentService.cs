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

        public List<Appointment> GetWeeklyAppointments(int doctorId)
        {
            return appointmentRepository.GetWeeklyAppointments(doctorId);
        }

        public void AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            ValidateAppointment(appointment);

            appointment.Status = 0;

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

            appointment.Status = 2;
            appointment.CancellationReason = reason;

            appointmentRepository.Update(appointment);
        }

        public void DeleteAppointment(int id)
        {
            var appointment = appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException(AppointmentNotFoundMessage);
            }

            appointmentRepository.Delete(id);
        }

        private void ValidateAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

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

            var existingAppointments = appointmentRepository
                .GetAll()
                .Where(a =>
                    a.AppointmentId != appointment.AppointmentId &&
                    a.Status != 2 && // Not Cancelled
                    a.ScheduledDate.Date == appointment.ScheduledDate.Date)
                .ToList();

            // Rule 1:
            // Same patient + same doctor + same date = NOT allowed
            bool patientAlreadyBookedSameDoctorSameDate =
                existingAppointments.Any(a =>
                    a.PatientId == appointment.PatientId &&
                    a.DoctorId == appointment.DoctorId);

            if (patientAlreadyBookedSameDoctorSameDate)
            {
                throw new InvalidOperationException(
                    "Patient already has an appointment with this doctor on this date.");
            }

            // Rule 2:
            // Same patient + same date + same slot with ANY doctor = NOT allowed
            bool patientAlreadyHasAppointmentInSameSlot =
                existingAppointments.Any(a =>
                    a.PatientId == appointment.PatientId &&
                    a.TimeSlot == appointment.TimeSlot);

            if (patientAlreadyHasAppointmentInSameSlot)
            {
                throw new InvalidOperationException(
                    "Patient already has another appointment during this time slot.");
            }

            // Rule 3:
            // Same doctor + same date + same slot = NOT allowed
            bool doctorSlotAlreadyBooked =
                existingAppointments.Any(a =>
                    a.DoctorId == appointment.DoctorId &&
                    a.TimeSlot == appointment.TimeSlot);

            if (doctorSlotAlreadyBooked)
            {
                throw new InvalidOperationException(
                    "Selected time slot is already booked.");
            }
        }
    }
}