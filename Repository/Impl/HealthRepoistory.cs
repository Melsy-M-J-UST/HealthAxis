using HAP_Pod4_ConsoleApp_au.Models;
using System;
using System.Collections.Generic;

namespace HAP_Pod4_ConsoleApp_au.Repositories
{
    public class HealthRepository : IHealthRepository
    {
        private readonly List<HealthRecord> _healthRecords;

        public HealthRepository()
        {
            _healthRecords = new List<HealthRecord>();
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            _healthRecords.Add(record);

            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            List<HealthRecord> patientRecords =
                new List<HealthRecord>();

            foreach (var record in _healthRecords)
            {
                if (record.Patient.PatientId == patientId)
                {
                    patientRecords.Add(record);
                }
            }

            return patientRecords;
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            List<HealthRecord> doctorRecords =
                new List<HealthRecord>();

            foreach (var record in _healthRecords)
            {
                if (record.Doctor.DoctorId == doctorId)
                {
                    doctorRecords.Add(record);
                }
            }

            return doctorRecords;
        }
    }
}