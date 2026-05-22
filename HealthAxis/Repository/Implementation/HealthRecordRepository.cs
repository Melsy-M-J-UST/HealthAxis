using HealthAxis.Data;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository.Implementation
{
    public class HealthRecordRepository: IHealthRecordRepository
    {
        private readonly Database _Db;

        public HealthRecordRepository(Database Db)
        {
            _Db = Db;
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            record.RecordId = _Db.GetNextHealthRecordId();
            _Db.HealthRecords.Add(record);
            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _Db.HealthRecords
                .Where(r => r.Patient.PatientId == patientId)
                .ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _Db.HealthRecords
                .Where(r => r.Doctor.DoctorId == doctorId)
                .ToList();
        }
    }
}
