using HealthAxis.Api.Data;
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
        public AppointmentService(IAppointmentRepository appointmentRepository) { _appointmentRepository = appointmentRepository; }
        public IEnumerable<AppointmentDto> GetAll() { return _appointmentRepository.GetAll().Select(MapToDto); }
        public IEnumerable<AppointmentDto> GetByPatient(int patientId) { return _appointmentRepository.GetByPatient(patientId).Select(MapToDto); }
        public IEnumerable<AppointmentDto> GetByDoctor(int doctorId) { return _appointmentRepository.GetByDoctor(doctorId).Select(MapToDto); }
        public bool Book(AppointmentDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (dto.PatientId <= 0) { errorMessage = "Patient is required."; return false; }
            if (dto.DoctorId <= 0) { errorMessage = "Doctor is required."; return false; }
            if (dto.ScheduledDate < DateTime.Today) { errorMessage = "Appointment date cannot be in the past."; return false; }
            if (string.IsNullOrWhiteSpace(dto.TimeSlot)) { errorMessage = "Time slot is required."; return false; }
            if (!_appointmentRepository.IsSlotAvailable(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)) { errorMessage = "This slot is already booked."; return false; }
            _appointmentRepository.Add(new Appointment { PatientId = dto.PatientId, DoctorId = dto.DoctorId, ScheduledDate = dto.ScheduledDate, TimeSlot = dto.TimeSlot, Status = AppointmentStatus.Pending.ToString(), CancellationReason = null });
            return true;
        }
        public bool UpdateStatus(AppointmentDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (dto.AppointmentId <= 0) { errorMessage = "Invalid appointment."; return false; }
            if (dto.Status != "Confirmed" && dto.Status != "Cancelled" && dto.Status != "Completed") { errorMessage = "Invalid status."; return false; }
            if (dto.Status == "Cancelled" && string.IsNullOrWhiteSpace(dto.CancellationReason)) { errorMessage = "Cancellation reason is required."; return false; }
            return _appointmentRepository.UpdateStatus(dto.AppointmentId, dto.Status, dto.Status == "Cancelled" ? dto.CancellationReason : null);
        }
        public bool Delete(int id) { return _appointmentRepository.Delete(id); }
        private AppointmentDto MapToDto(Appointment a)
        {
            return new AppointmentDto { AppointmentId = a.AppointmentId, PatientId = a.PatientId, PatientName = a.Patient != null ? a.Patient.FullName : null, DoctorId = a.DoctorId, DoctorName = a.Doctor != null ? a.Doctor.FullName : null, DoctorSpecialisation = a.Doctor != null ? a.Doctor.Specialisation : null, ScheduledDate = a.ScheduledDate, TimeSlot = a.TimeSlot, Status = a.Status, CancellationReason = a.CancellationReason };
        }
    }
}
