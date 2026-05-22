using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services.Impl;

namespace HealthAxisTest.ServiceTests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRepository> _repoMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _repoMock = new Mock<IHealthRepository>();
            _service = new HealthRecordService(_repoMock.Object);
        }

        private Patient CreatePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = $"Patient{id}"
            };
        }

        private Doctor CreateDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = $"Doctor{id}"
            };
        }

        
        [Fact]
        public void AddRecord_Valid_ShouldAddRecord()
        {
            
            var record = new HealthRecord
            {
                Patient = CreatePatient(1),
                Doctor = CreateDoctor(1),
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Tablet"
            };

            _repoMock.Setup(r => r.AddRecord(record))
                .Returns(record);

            
            var result = _service.AddRecord(record);

           
            Assert.NotNull(result);
            Assert.Equal("Fever", result.Diagnosis);

            _repoMock.Verify(r => r.AddRecord(record), Times.Once);
        }

        
        [Fact]
        public void AddRecord_Null_ShouldThrowException()
        {
             & Assert
            var ex = Assert.Throws<Exception>(() =>
                _service.AddRecord(null)
            );

            Assert.Contains("cannot be null", ex.Message);
        }

       
        [Fact]
        public void AddRecord_EmptyDiagnosis_ShouldThrowException()
        {
           
            var record = new HealthRecord
            {
                Patient = CreatePatient(1),
                Doctor = CreateDoctor(1),
                VisitDate = DateTime.Today,
                Diagnosis = "" 
            };

          
            var ex = Assert.Throws<Exception>(() =>
                _service.AddRecord(record)
            );

            Assert.Contains("Diagnosis", ex.Message);
        }

        
        [Fact]
        public void GetRecordsByPatient_ShouldReturnSortedDescending()
        {
            
            var patient = CreatePatient(1);

            var records = new List<HealthRecord>
            {
                new HealthRecord { Patient = patient, VisitDate = DateTime.Today.AddDays(-2) },
                new HealthRecord { Patient = patient, VisitDate = DateTime.Today }
            };

            _repoMock.Setup(r => r.GetRecordsByPatient(1))
                .Returns(records);

            
            var result = _service.GetRecordsByPatient(1);

            
            Assert.Equal(2, result.Count);
            Assert.True(result[0].VisitDate >= result[1].VisitDate);
        }

        
        [Fact]
        public void GetRecordsByDoctor_ShouldReturnSortedDescending()
        {
            
            var doctor = CreateDoctor(1);

            var records = new List<HealthRecord>
            {
                new HealthRecord { Doctor = doctor, VisitDate = DateTime.Today.AddDays(-5) },
                new HealthRecord { Doctor = doctor, VisitDate = DateTime.Today.AddDays(-1) }
            };

            _repoMock.Setup(r => r.GetRecordsByDoctor(1))
                .Returns(records);

            
            var result = _service.GetRecordsByDoctor(1);

           
            Assert.Equal(2, result.Count);
            Assert.True(result[0].VisitDate >= result[1].VisitDate);
        }
    }
}