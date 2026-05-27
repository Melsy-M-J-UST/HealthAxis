using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxisTests.RepositoryTests
{
    public class HealthRecordRepositoryTests
    {
        private readonly Database _db;
        private readonly HealthRecordRepository _repository;

        public HealthRecordRepositoryTests()
        {
            _db = new Database();
            _repository = new HealthRecordRepository(_db);
        }
        [Fact]
        public void AddRecord_ShouldAddHealthRecord()
        {
            var record = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1, PatientName = "Test Patient" },
                Doctor = new Doctor { DoctorId = 1, DoctorName = "Test Doctor" },
                Diagnosis = "Test Diagnosis",
                VisitedDate = DateTime.Now
            };
            _repository.AddRecord(record);
            var records = _repository.GetRecordsByPatient(1);
            Assert.Single(records);
            Assert.Equal("Test Diagnosis", records[0].Diagnosis);
        }
    }
}
