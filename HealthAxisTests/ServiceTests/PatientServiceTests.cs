using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services.Impl;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace HealthAxisTests.ServiceTests
{
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _mockRepo;
        private PatientService _service;

        public PatientServiceTests()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllPatients_ReturnsList()
        {
            var patients = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "John" }
            };

            _mockRepo.Setup(r => r.GetAllPatients()).Returns(patients);

            var result = _service.GetAllPatients();

            Assert.Single(result);
            Assert.Equal("John", result[0].FullName);
        }

        [Fact]
        public void RegisterPatient_ReturnsPatient()
        {
            var patient = new Patient { PatientId = 1, FullName = "Anna" };

            _mockRepo.Setup(r => r.RegisterPatient(patient)).Returns(patient);

            var result = _service.RegisterPatient(patient);

            Assert.NotNull(result);
            Assert.Equal("Anna", result.FullName);
        }

        [Fact]
        public void GetPatientById_ReturnsPatient()
        {
            var patient = new Patient { PatientId = 1, FullName = "Sam" };

            _mockRepo.Setup(r => r.GetPatientById(1)).Returns(patient);

            var result = _service.GetPatientById(1);

            Assert.NotNull(result);
            Assert.Equal("Sam", result.FullName);
        }

        [Fact]
        public void UpdatePatient_Null_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => _service.UpdatePatient(null!));
        }

    }
}