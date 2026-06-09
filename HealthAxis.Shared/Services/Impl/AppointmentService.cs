using System;
using System.Collections.Generic;
using System.Linq;
using HealthAxisWebApp;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxis.Shared.Services.Interfaces;

namespace HealthAxis.Shared.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IDoctorRepository doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository)
        {
            this.appointmentRepository =
                appointmentRepository;

            this.doctorRepository =
                doctorRepository;
        }

        public List<Appointment> GetAllAppointments()
        {
            return appointmentRepository.GetAll();
        }

        public Appointment GetAppointmentById(int id)
        {
            return appointmentRepository.GetById(id);
        }

        public void AddAppointment(
            Appointment appointment)
        {
            ValidateAppointment(appointment);

            appointment.Status = 0;

            if (appointment.CancellationReason == null)
            {
                appointment.CancellationReason =
                    string.Empty;
            }

            appointmentRepository.Add(appointment);
        }

        public void UpdateAppointment(
            Appointment appointment)
        {
            ValidateAppointment(appointment);

            appointmentRepository.Update(
                appointment);
        }

        public void ConfirmAppointment(int id)
        {
            Appointment appointment =
                appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            if (appointment.Status != 0)
            {
                throw new Exception(
                    "Only pending appointments can be confirmed.");
            }

            appointment.Status = 1;

            appointmentRepository.Update(
                appointment);
        }

        public void CompleteAppointment(int id)
        {
            Appointment appointment =
                appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            if (appointment.Status != 1)
            {
                throw new Exception(
                    "Only confirmed appointments can be completed.");
            }

            appointment.Status = 3;

            appointmentRepository.Update(
                appointment);
        }

        public void CancelAppointment(int id, string reason)
        {
            Appointment appointment =
                appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            if (appointment.Status == 3)
            {
                throw new Exception(
                    "Completed appointments cannot be cancelled.");
            }

            if (appointment.Status == 2)
            {
                throw new Exception(
                    "Appointment is already cancelled.");
            }

            appointment.Status = 2;
            appointment.CancellationReason = reason;

            appointmentRepository.Update(appointment);
        }

        public void DeleteAppointment(int id)
        {
            appointmentRepository.Delete(id);
        }

        private void ValidateAppointment(
            Appointment appointment)
        {
            if (appointment.PatientId <= 0)
            {
                throw new Exception(
                    "Invalid Patient.");
            }

            if (appointment.DoctorId <= 0)
            {
                throw new Exception(
                    "Invalid Doctor.");
            }

            if (appointment.ScheduledDate.Date <
                DateTime.Today)
            {
                throw new Exception(
                    "Past dates are not allowed.");
            }



            Doctor doctor =
                doctorRepository.GetById(
                    appointment.DoctorId);

            if (doctor == null)
            {
                throw new Exception(
                    "Doctor not found.");
            }

            if (!doctor.IsActive)
            {
                throw new Exception(
                    "Doctor is inactive.");
            }

            bool alreadyBooked =
                appointmentRepository
                .GetAll()
                .Any(a =>
                    a.AppointmentId !=
                        appointment.AppointmentId
                    &&
                    a.DoctorId ==
                        appointment.DoctorId
                    &&
                    a.ScheduledDate ==
                        appointment.ScheduledDate
                    &&
                    a.TimeSlot ==
                        appointment.TimeSlot
                    &&
                    a.Status != 2);

            if (alreadyBooked)
            {
                throw new Exception(
                    "Doctor already has an appointment in this slot.");
            }
        }
    }

}
