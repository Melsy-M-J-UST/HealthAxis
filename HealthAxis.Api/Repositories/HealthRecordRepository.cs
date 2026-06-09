using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthAxisEntities _context;

        public HealthRecordRepository(HealthAxisEntities context)
        {
            _context = context;
        }

        public IEnumerable<HealthRecord> GetByPatient(int patientId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public HealthRecord Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            _context.SaveChanges();

            return record;
        }
    }
}