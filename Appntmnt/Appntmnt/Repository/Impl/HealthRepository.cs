using Appntmnt.Data;
using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Repository.Impl
{
    public class HealthRepository : IHealthRepository
    {
        private readonly AppDbContext _context;

        public HealthRepository(AppDbContext context)
        {
            _context = context;
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            record.RecordId = _context.GetNextHealthRecordId();
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
