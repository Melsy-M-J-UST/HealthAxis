using System;
using System.Collections.Generic;
using HealthAxis.Shared.Services.Impl;
using HealthAxisWebApp;
using HealthAxisWebApp.Repositories.Interfaces;
using Moq;
using Xunit;

namespace HealthAxis.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> healthRecordRepositoryMock;
        private readonly HealthRecordService healthRecordService;

        public HealthRecordServiceTests()
        {
            healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            healthRecordService = new HealthRecordService(
                healthRecordRepositoryMock.Object);
        }

        [Fact]
        public void GetAllRecords_ReturnsRecords()
        {
            var records = new List<HealthRecord>
            {
                CreateValidHealthRecord(1)
            };

            healthRecordRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(records);

            var result = healthRecordService.GetAllRecords();

            Assert.Single(result);
            Assert.Equal(1, result[0].RecordId);
        }

        [Fact]
        public void GetRecordById_WhenExists_ReturnsRecord()
        {
            var record = CreateValidHealthRecord(1);

            healthRecordRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(record);

            var result = healthRecordService.GetRecordById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
        }

        [Fact]
        public void GetRecordById_WhenNotExists_ReturnsNull()
        {
            healthRecordRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((HealthRecord)null);

            var result = healthRecordService.GetRecordById(99);

            Assert.Null(result);
        }

        [Fact]
        public void AddRecord_WhenRecordIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                healthRecordService.AddRecord(null));
        }

        [Fact]
        public void AddRecord_WhenPatientIdInvalid_ThrowsArgumentException()
        {
            var record = CreateValidHealthRecord();
            record.PatientId = 0;

            Assert.Throws<ArgumentException>(() =>
                healthRecordService.AddRecord(record));
        }

        [Fact]
        public void AddRecord_WhenDoctorIdInvalid_ThrowsArgumentException()
        {
            var record = CreateValidHealthRecord();
            record.DoctorId = 0;

            Assert.Throws<ArgumentException>(() =>
                healthRecordService.AddRecord(record));
        }

        [Fact]
        public void AddRecord_WhenVisitDateInPast_ThrowsArgumentException()
        {
            var record = CreateValidHealthRecord();
            record.VisitDate = DateTime.Today.AddDays(-1);

            Assert.Throws<ArgumentException>(() =>
                healthRecordService.AddRecord(record));
        }

        [Fact]
        public void AddRecord_WhenDiagnosisMissing_ThrowsArgumentException()
        {
            var record = CreateValidHealthRecord();
            record.Diagnosis = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                healthRecordService.AddRecord(record));
        }

        [Fact]
        public void AddRecord_WhenPrescriptionMissing_ThrowsArgumentException()
        {
            var record = CreateValidHealthRecord();
            record.Prescription = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                healthRecordService.AddRecord(record));
        }

        [Fact]
        public void AddRecord_WhenValid_CallsRepositoryAdd()
        {
            var record = CreateValidHealthRecord();

            healthRecordService.AddRecord(record);

            healthRecordRepositoryMock.Verify(
                r => r.Add(record),
                Times.Once);
        }

        [Fact]
        public void UpdateRecord_WhenRecordIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                healthRecordService.UpdateRecord(null));
        }

        [Fact]
        public void UpdateRecord_WhenInvalid_ThrowsArgumentException()
        {
            var record = CreateValidHealthRecord();
            record.Diagnosis = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                healthRecordService.UpdateRecord(record));
        }

        [Fact]
        public void UpdateRecord_WhenValid_CallsRepositoryUpdate()
        {
            var record = CreateValidHealthRecord(1);

            healthRecordService.UpdateRecord(record);

            healthRecordRepositoryMock.Verify(
                r => r.Update(record),
                Times.Once);
        }

        [Fact]
        public void DeleteRecord_WhenRecordNotFound_ThrowsKeyNotFoundException()
        {
            healthRecordRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((HealthRecord)null);

            Assert.Throws<KeyNotFoundException>(() =>
                healthRecordService.DeleteRecord(99));
        }

        [Fact]
        public void DeleteRecord_WhenRecordExists_CallsRepositoryDelete()
        {
            var record = CreateValidHealthRecord(1);

            healthRecordRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(record);

            healthRecordService.DeleteRecord(1);

            healthRecordRepositoryMock.Verify(
                r => r.Delete(1),
                Times.Once);
        }

        private static HealthRecord CreateValidHealthRecord(int id = 0)
        {
            return new HealthRecord
            {
                RecordId = id,
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                VisitDate = DateTime.Today.AddDays(1),
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Rest advised"
            };
        }
    }
}