using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;

        public HealthRecordService(IHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        public IEnumerable<HealthRecordDto> GetByPatient(int patientId)
        {
            var healthRecords = _healthRecordRepository.GetByPatient(patientId);

            return healthRecords.Select(record => new HealthRecordDto
            {
                RecordId = record.RecordId,
                PatientId = record.PatientId,
                PatientName = record.Patient != null ? record.Patient.FullName : null,
                DoctorId = record.DoctorId,
                DoctorName = record.Doctor != null ? record.Doctor.FullName : null,
                DoctorSpecialisation = record.Doctor != null ? record.Doctor.Specialisation : null,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes,
                AppointmentId = record.AppointmentId
            });
        }

        public HealthRecordDto Create(HealthRecordDto dto)
        {
            var healthRecord = new HealthRecord
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentId = dto.AppointmentId,
                VisitDate = DateTime.Now,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            var createdHealthRecord = _healthRecordRepository.Add(healthRecord);

            dto.RecordId = createdHealthRecord.RecordId;
            dto.VisitDate = createdHealthRecord.VisitDate;

            return dto;
        }
    }
}