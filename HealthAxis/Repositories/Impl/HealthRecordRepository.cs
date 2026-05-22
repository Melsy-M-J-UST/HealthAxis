using HealthAxis.Data;
using HealthAxis.Models;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly Database _context;

        public HealthRecordRepository(Database context)
        {
            _context = context;
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            record.HealthRecordId = _context.GetNextHealthRecordId();
            _context.HealthRecords.Add(record);
            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _context.HealthRecords
                .Where(r => r.Patient.PatientId == patientId)
                .ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _context.HealthRecords
                .Where(r => r.Doctor.DoctorId == doctorId)
                .ToList();
        }
    }
}