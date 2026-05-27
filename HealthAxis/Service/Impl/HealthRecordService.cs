using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services;
using System;
using System.Collections.Generic;

namespace HealthAxis.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRepository _repository;

        public HealthRecordService(IHealthRepository repository)
        {
            _repository = repository;
        }

        public HealthRecord? AddRecord(HealthRecord record)
        {

            if (record == null)
            {
                throw new ArgumentException("Health record cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(record.Diagnosis))
            {
                throw new ArgumentException("Diagnosis cannot be empty.");
            }

            var result = _repository.AddRecord(record);

            if (result == null)
            {
                throw new InvalidOperationException("A health record already exists for this appointment.");
            }

            return result;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _repository
                .GetRecordsByPatient(patientId)
                .OrderByDescending(record => record.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _repository
                .GetRecordsByDoctor(doctorId)
                .OrderByDescending(record => record.VisitDate)
                .ToList();
        }
    }
}