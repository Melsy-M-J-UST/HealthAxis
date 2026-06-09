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
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;

        public HealthRecordService(IHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        public IEnumerable<HealthRecordDto> GetByPatient(int patientId)
        {
            return _healthRecordRepository
                .GetByPatient(patientId)
                .Select(Map);
        }

        public bool Create(HealthRecordDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            var healthRecord = new HealthRecord
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                VisitDate = DateTime.Now,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            _healthRecordRepository.Add(healthRecord);

            return true;
        }

        private HealthRecordDto Map(HealthRecord record)
        {
            return new HealthRecordDto
            {
                RecordId = record.RecordId,

                PatientId = record.PatientId,
                PatientName = record.Patient != null
                    ? record.Patient.FullName
                    : null,

                DoctorId = record.DoctorId,
                DoctorName = record.Doctor != null
                    ? record.Doctor.FullName
                    : null,

                DoctorSpecialisation = record.Doctor != null
                    ? (SpecialisationEnum?)EnumMapper.ParseEnum<SpecialisationEnum>(
                        record.Doctor.Specialisation)
                    : null,

                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };
        }
    }
}