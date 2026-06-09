using System;
using System.Collections.Generic;
using HealthAxisWebApp;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxis.Shared.Services.Interfaces;

namespace HealthAxis.Shared.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository repository;

        public HealthRecordService(IHealthRecordRepository repository)
        {
            this.repository = repository;
        }

        public List<HealthRecord> GetAllRecords()
        {
            return repository.GetAll();
        }

        public HealthRecord GetRecordById(int id)
        {
            return repository.GetById(id);
        }

        public void AddRecord(HealthRecord record)
        {
            ValidateRecord(record);

            repository.Add(record);
        }

        public void UpdateRecord(HealthRecord record)
        {
            ValidateRecord(record);

            repository.Update(record);
        }

        public void DeleteRecord(int id)
        {
            HealthRecord record = repository.GetById(id);

            if (record == null)
            {
                throw new KeyNotFoundException("Health record not found.");
            }

            repository.Delete(id);
        }

        private static void ValidateRecord(HealthRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.PatientId <= 0)
            {
                throw new ArgumentException(
                    "Invalid Patient.",
                    nameof(record));
            }

            if (record.DoctorId <= 0)
            {
                throw new ArgumentException(
                    "Invalid Doctor.",
                    nameof(record));
            }

            if (record.VisitDate < DateTime.Now)
            {
                throw new ArgumentException(
                    "Visit Date cannot be in the past.",
                    nameof(record));
            }

            if (string.IsNullOrWhiteSpace(record.Diagnosis))
            {
                throw new ArgumentException(
                    "Diagnosis is required.",
                    nameof(record));
            }

            if (string.IsNullOrWhiteSpace(record.Prescription))
            {
                throw new ArgumentException(
                    "Prescription is required.",
                    nameof(record));
            }
        }
    }
}