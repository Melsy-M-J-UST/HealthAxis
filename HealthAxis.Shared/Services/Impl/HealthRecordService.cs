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

        public HealthRecordService(
            IHealthRecordRepository repository)
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
            repository.Delete(id);
        }

        private void ValidateRecord(
            HealthRecord record)
        {
            if (record.PatientId <= 0)
            {
                throw new Exception(
                    "Invalid Patient.");
            }

            if (record.DoctorId <= 0)
            {
                throw new Exception(
                    "Invalid Doctor.");
            }

            if (record.VisitDate < DateTime.Now)
            {
                throw new Exception(
                    "Visit Date cannot be in the past.");
            }

            if (string.IsNullOrWhiteSpace(
                record.Diagnosis))
            {
                throw new Exception(
                    "Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(
                record.Prescription))
            {
                throw new Exception(
                    "Prescription is required.");
            }
        }
    }

}
