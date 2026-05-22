using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Service.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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
                new Patient { PatientId = 1, PatientName = "John" }
            };
            _mockRepo.Setup(r => r.GetAllPatients()).Returns(patients);
            var result = _service.GetAllPatients();
            Assert.Single(result);
            Assert.Equal("John", result[0].PatientName);
        }
        [Fact]
        public void RegisterPatient_ReturnsPatient()
        {
            var patient = new Patient { PatientId = 1, PatientName = "Anna" };
            _mockRepo.Setup(r => r.RegisterPatient(patient)).Returns(patient);
            var result = _service.RegisterPatient(patient);
            Assert.NotNull(result);
            Assert.Equal("Anna", result.PatientName);
        }
        [Fact]
        public void GetPatientById_ReturnsPatient()
        {
            var patient = new Patient { PatientId = 1, PatientName = "Sam" };
            _mockRepo.Setup(r => r.GetPatientById(1)).Returns(patient);
            var result = _service.GetPatientById(1);
            Assert.NotNull(result);
            Assert.Equal("Sam", result.PatientName);
        }
    }
}
