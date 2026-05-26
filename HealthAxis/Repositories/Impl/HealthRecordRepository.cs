using HealthAxis.Data;
using HealthAxis.Models;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly Database _dbContext;

        public HealthRecordRepository(Database dbContext)
        {
            _dbContext = dbContext;
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            record.HealthRecordId = _dbContext.GetNextHealthRecordId();
            _dbContext.HealthRecords.Add(record);
            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _dbContext.HealthRecords
                .Where(r => r.Patient.PatientId == patientId)
                .ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _dbContext.HealthRecords
                .Where(r => r.Doctor.DoctorId == doctorId)
                .ToList();
        }
    }
}