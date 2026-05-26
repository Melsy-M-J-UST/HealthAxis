using System;
using System.Collections.Generic;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services.Impl;
using Moq;
using Xunit;

namespace HealthAxisTests.ServiceTests

{

    public class HealthRecordServiceTests
    {

        private readonly Mock<IHealthRecordRepository> _repoMock;

        private readonly HealthRecordService _service;


        public HealthRecordServiceTests()

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

                VisitDate = DateTime.Now,

                Patient = new Patient { PatientId = 1 },

                Doctor = new Doctor { DoctorId = 1 }

            };



            var result = _service.AddRecord(record);



            Assert.Null(result);

            _repoMock.Verify(r => r.AddRecord(record), Times.Once);

        }


        [Fact]

        public void AddRecord_NullRecord_ShouldThrowException()

        {


            Assert.Throws<Exception>(() => _service.AddRecord(null));

        }


        [Fact]

        public void GetRecordsByPatient_ShouldReturnSortedList()

        {


            var records = new List<HealthRecord>
            {

                new HealthRecord { VisitDate = DateTime.Now.AddDays(-1) },

                new HealthRecord { VisitDate = DateTime.Now }

            };


            _repoMock.Setup(r => r.GetRecordsByPatient(1))

                     .Returns(records);



            var result = _service.GetRecordsByPatient(1);



            Assert.Equal(2, result.Count);

            Assert.True(result[0].VisitDate >= result[1].VisitDate);

        }

    }

}