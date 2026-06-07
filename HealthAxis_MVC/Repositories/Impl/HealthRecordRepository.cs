using System.Collections.Generic;
using System.Linq;
using HealthAxis_MVC.Models;
using static HealthAxis_MVC.Database.AppContextDB;

namespace HealthAxis_MVC.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        public List<HealthRecord> GetAll()
        {
            return Records;
        }

        public HealthRecord GetById(int id)
        {
            return Records.First(x => x.HealthRecordId == id);
        }

        public void Add(HealthRecord record)
        {
            Records.Add(record);
        }

        public void Update(int id, HealthRecord record)
        {
            var existing = Records.First(x => x.HealthRecordId == id);

            existing.PatientId = record.PatientId;
            existing.DoctorId = record.DoctorId;
            existing.VisitDate = record.VisitDate;
            existing.Diagnosis = record.Diagnosis;
            existing.Prescription = record.Prescription;
            existing.Notes = record.Notes;
        }
    }
}