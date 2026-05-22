using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        public HealthRecord AddRecord(HealthRecord record)
        {

            if (record == null)
            {
                throw new Exception("Health record cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(record.Diagnosis))
            {
                throw new Exception("Diagnosis cannot be empty.");
            }

            _repository.AddRecord(record);
            return record;
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
