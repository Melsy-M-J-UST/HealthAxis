using HealthAxisMVC.Exceptions;
using HealthAxisMVC.Models;
using HealthAxisMVC.Repositories;
using HealthAxisMVC.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date)
        {
            try
            {
                if (patient == null)
                    throw new HealthAppException("Patient is required");

                if (doctor == null)
                    throw new HealthAppException("Doctor is required");

                if (date.Date < DateTime.Today)
                    throw new HealthAppException("Cannot book appointment in past");

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    throw new HealthAppException("Doctor unavailable on Sunday");

                if (!doctor.IsActive)
                    throw new HealthAppException("Doctor is not active");

                var slot = _appointmentRepository
                    .GetNextAvailableSlotAvoidingPatientConflicts(doctor.DoctorId, date, patient.PatientId);

                if (slot == null)
                {
                    slot = _appointmentRepository
                        .GetNextAvailableSlot(doctor.DoctorId, date);
                }

                if (slot == null)
                    throw new HealthAppException("No slots available");

                // here we can allow the patient to book multiple doctors on the same day
                if (_appointmentRepository.PatientHasAppointmentAt(patient.PatientId, date, slot))
                    throw new HealthAppException("Patient already has appointment at this time");

                var appointment = new Appointment
                {
                    Patient = patient,
                    Doctor = doctor,
                    ScheduledDate = date.Date,
                    TimeSlot = slot,
                    Status = Appointment.StatusOption.Pending
                };

                return _appointmentRepository.Add(appointment);
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }

        public void CancelAppointment(int appointmentId, string reason)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(appointmentId);

                if (appointment == null)
                    throw new HealthAppException("Appointment not found");

                if (appointment.Status == Appointment.StatusOption.Completed)
                    throw new HealthAppException("Completed appointment cannot be cancelled");

                appointment.Cancel(reason); 
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAll();
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            try
            {
                return _appointmentRepository.GetById(appointmentId);
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            try
            {
                return _appointmentRepository.GetByPatientId(patientId);
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            try
            {
                return _appointmentRepository.GetByDoctorId(doctorId);
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            return _appointmentRepository.GetAll()
                .Where(a =>
                    a.ScheduledDate.Date >= DateTime.Today &&
                    a.Status != Appointment.StatusOption.Cancelled &&
                    a.Status != Appointment.StatusOption.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public void ConfirmAppointment(int id)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(id);

                if (appointment == null)
                    throw new HealthAppException("Appointment not found");

                if (appointment.Status != Appointment.StatusOption.Pending)
                    throw new HealthAppException("Only pending appointments can be confirmed");

                appointment.Status = Appointment.StatusOption.Confirmed;
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }

        public void CompleteAppointment(int id)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(id);

                if (appointment == null)
                    throw new HealthAppException("Appointment not found");

                if (appointment.Status != Appointment.StatusOption.Confirmed)
                    throw new HealthAppException("Only confirmed appointments can be completed");

                appointment.Status = Appointment.StatusOption.Completed;
            }
            catch (Exception ex)
            {
                throw new HealthAppException(ex.Message);
            }
        }
    }
}