using HealthAxis.Models;
using HealthAxis.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using HealthAxis.Exceptions;

namespace HealthAxis.Service.Implementation
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;

        public HealthRecordService(IHealthRecordRepository repository)
        {
            _repository = repository;
        }

        public HealthRecord AddRecord(HealthRecord? record)
        {

            if (record == null)
            {
                throw new InvalidHealthRecordException("Health record cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(record.Diagnosis))
            {
                throw new InvalidHealthRecordException("Diagnosis cannot be empty.");
            }

            _repository.AddRecord(record);
            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _repository.GetRecordsByPatient(patientId).OrderByDescending(record => record.VisitedDate).ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _repository.GetRecordsByDoctor(doctorId).OrderByDescending(record => record.VisitedDate).ToList();
        }
    }
}
