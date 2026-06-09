using HealthAxis.Api.Data;
using HealthAxis.Api.Helpers;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<AppointmentDto> GetAll()
        {
            return _appointmentRepository
                .GetAll()
                .Select(Map);
        }

        public IEnumerable<AppointmentDto> GetByPatient(int patientId)
        {
            return _appointmentRepository
                .GetByPatient(patientId)
                .Select(Map);
        }

        public IEnumerable<AppointmentDto> GetByDoctor(int doctorId)
        {
            return _appointmentRepository
                .GetByDoctor(doctorId)
                .Select(Map);
        }

        public IEnumerable<AppointmentDto> GetTodaySchedule(int doctorId)
        {
            return _appointmentRepository
                .GetByDoctorAndDate(doctorId, DateTime.Today)
                .Select(Map);
        }

        public IEnumerable<AppointmentDto> GetWeeklySchedule(
            int doctorId,
            DateTime startDate)
        {
            return _appointmentRepository
                .GetByDoctorAndDateRange(
                    doctorId,
                    startDate.Date,
                    startDate.Date.AddDays(6))
                .Select(Map);
        }

        public bool Book(AppointmentDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (_patientRepository.GetById(dto.PatientId) == null)
            {
                errorMessage = "Invalid patient.";
                return false;
            }

            if (_doctorRepository.GetById(dto.DoctorId) == null)
            {
                errorMessage = "Invalid doctor.";
                return false;
            }

            if (dto.ScheduledDate.Date < DateTime.Today)
            {
                errorMessage = "Appointment date cannot be in the past.";
                return false;
            }

            if (!_appointmentRepository.IsSlotAvailable(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
            {
                errorMessage = "This slot is already booked.";
                return false;
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate.Date,
                TimeSlot = dto.TimeSlot,
                Status = AppointmentStatusEnum.Pending.ToString(),
                CancellationReason = null
            };

            _appointmentRepository.Add(appointment);

            return true;
        }

        public bool UpdateStatus(
            int id,
            AppointmentStatusUpdateDto dto,
            out string errorMessage)
        {
            errorMessage = string.Empty;

            if (dto.Status == AppointmentStatusEnum.Cancelled &&
                string.IsNullOrWhiteSpace(dto.CancellationReason))
            {
                errorMessage = "Cancellation reason is required.";
                return false;
            }

            return _appointmentRepository.UpdateStatus(
                id,
                dto.Status.ToString(),
                dto.CancellationReason);
        }

        public bool Delete(int id)
        {
            return _appointmentRepository.Delete(id);
        }

        private AppointmentDto Map(Appointment appointment)
        {
            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,

                PatientId = appointment.PatientId,
                PatientName = appointment.Patient != null
                    ? appointment.Patient.FullName
                    : null,

                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor != null
                    ? appointment.Doctor.FullName
                    : null,

                DoctorSpecialisation = appointment.Doctor != null
                    ? (SpecialisationEnum?)EnumMapper.ParseEnum<SpecialisationEnum>(
                        appointment.Doctor.Specialisation)
                    : null,

                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,

                Status = EnumMapper.ParseEnum<AppointmentStatusEnum>(
                    appointment.Status),

                CancellationReason = appointment.CancellationReason
            };
        }
    }
}