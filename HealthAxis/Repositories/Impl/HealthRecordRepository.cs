using HealthAxis.Data;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static HealthAxis.Repositories.Impl.HealthRecordRepository;

namespace HealthAxis.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        public class HealthRepository : IHealthRepository
        {
            private readonly Database _context;

            public HealthRepository(Database context)
            {
                _context = context;
            }
            public void AddRecord(HealthRecord record)
            {
                _ContextDb.HealthRecords.Add(record);
                _ContextDb.SaveChanges();
            }

            public List<HealthRecord> GetRecordsByPatient(int patientId)
            {
                return _ContextDb.HealthRecords
                    .Where(r => r.PatientId == patientId)
                    .ToList();
            }

            public List<HealthRecord> GetRecordsByDoctor(int doctorId)
            {
                return _ContextDb.HealthRecords
                    .Where(r => r.DoctorId == doctorId)
                    .ToList();
            }
        }
    }
}
