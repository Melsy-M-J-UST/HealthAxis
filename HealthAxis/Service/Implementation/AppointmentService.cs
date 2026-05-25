using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Repository.Implementation;
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

        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date)
        {
            if (patient == null)
            {
                throw new ArgumentException("Patient is required.");
            }

            if (doctor == null)
            {
                throw new ArgumentException("Doctor is required.");
            }

            if (date.Date < DateTime.Today)
            {
                throw new PastDateException("Cannot book an appointment in the past.");
            }
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new DoctorUnavailableException("Doctor is unavailable on sundays.");
            }

            if (!doctor.IsPractising)
            {
                throw new DoctorUnavailableException("Doctor is not active.");
            }


            var hasConflict = _repository.GetAppointmentsByPatient(patient.PatientId)
                .Any(a => a.Doctor.DoctorId == doctor.DoctorId);

            if (hasConflict)
            {
                throw new AppointmentConflictException("Patient already has an appointment with this doctor on the selected date.");
            }

            var availableSlot = _repository.GetNextAvailableSlotAvoidingPatientConflicts(doctor.DoctorId, date, patient.PatientId);

            if (availableSlot == null)
            {
                availableSlot = _repository.GetNextAvailableSlot(doctor.DoctorId, date);
            }

            if (availableSlot == null)
            {
                throw new DoctorUnavailableException("No available slots for this doctor on the selected date.");
            }
            if (_repository.PatientHasAppointmentAt(patient.PatientId, date, availableSlot))
            {
                throw new AppointmentConflictException("Patient already has an appointment at the selected date and time slot.");
            }

            var appointment = new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date.Date,
                Slot = availableSlot,
                Status = Appointment.AppointmentStatus.Confirmed
            };
            return _repository.AddAppointment(appointment);
        }

        public bool CancelAppointment(int appointmentId, string reason)
        {
            var appointment = _repository.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return false;
            }
            appointment.Cancel(reason);
            _repository.Remove(appointment);
            return true;
        }
        public List<Appointment> GetUpcomingAppointments()
        {
            return _repository.GetAllAppointments()
                .Where(a =>
                    a.ScheduledDate.Date >= DateTime.Today &&
                    a.Status == Appointment.AppointmentStatus.Confirmed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.Doctor.DoctorName)
                .ToList();
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

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return _repository.GetAppointmentsByPatient(patientId);
        }
    }
}
