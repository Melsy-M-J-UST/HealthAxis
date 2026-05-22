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
            public void AddRecord(HealthRecord record)
            {
                Database.HealthRecords.Add(record);
                Database.SaveChanges();
            }

            public List<HealthRecord> GetRecordsByPatient(int patientId)
            {
                return Database.HealthRecords
                    .Where(r => r.PatientId == patientId)
                    .ToList();
            }

            public List<HealthRecord> GetRecordsByDoctor(int doctorId)
            {
                return Database.HealthRecords
                    .Where(r => r.DoctorId == doctorId)
                    .ToList();
            }
        }
    }
}
