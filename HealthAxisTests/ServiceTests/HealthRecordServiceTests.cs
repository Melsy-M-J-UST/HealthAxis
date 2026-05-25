using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Service.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace HealthAxis.Tests.Services
{
    public class HealthServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _repoMock;
        private readonly HealthRecordService _service;
        public HealthServiceTests()
        {
            _repoMock = new Mock<IHealthRecordRepository>();
            _service = new HealthRecordService(_repoMock.Object);
        }
        [Fact]
        public void AddRecord_ValidRecord_ShouldCallRepository()
        {
            var record = new HealthRecord
            {
                Diagnosis = "Cold",
                VisitedDate = DateTime.Now,
                Patient = new Patient { PatientId = 1 },
                Doctor = new Doctor { DoctorId = 1 }
            };
            var result = _service.AddRecord(record);
            Assert.NotNull(result);
            _repoMock.Verify(r => r.AddRecord(record), Times.Once);
        }
        [Fact]
        public void AddRecord_NullRecord_ShouldThrowException()
        {
            Assert.Throws<InvalidHealthRecordException>(() => _service.AddRecord(null));
        }
        [Fact]
        public void GetRecordsByPatient_ShouldReturnSortedList()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { VisitedDate = DateTime.Now.AddDays(-1) },
                new HealthRecord { VisitedDate = DateTime.Now }
            };
            _repoMock.Setup(r => r.GetRecordsByPatient(1)).Returns(records);
            var result = _service.GetRecordsByPatient(1);
            Assert.Equal(2, result.Count);
            Assert.True(result[0].VisitedDate >= result[1].VisitedDate);
        }
    }
}