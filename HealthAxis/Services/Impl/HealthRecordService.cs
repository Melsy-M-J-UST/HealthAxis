using HealthAxis.Models;
using HealthAxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;

        public HealthRecordService(IHealthRecordRepository repository)
        {
            _repository = repository;
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            if (record == null)
                throw new Exception("Health record cannot be null.");

            if (string.IsNullOrWhiteSpace(record.Diagnosis))
                throw new Exception("Diagnosis cannot be empty.");

            return _repository.AddRecord(record);
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _repository
                .GetRecordsByPatient(patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _repository
                .GetRecordsByDoctor(doctorId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }
    }
}
