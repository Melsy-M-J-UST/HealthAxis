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
        [Fact]
        public void GetPatientById_ThrowsException_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetPatientById(99)).Returns(null as Patient);
            Assert.Throws<HealthAxis.Exceptions.PatientNotFoundException>(() => _service.GetPatientById(99));
        }
        [Theory]
        [InlineData(1, "John", "2000-01-01", "1234567890")]
        [InlineData(2, "Anna", "1995-05-15", "0987654321")]
        public void UpdatePatient_ReturnsTrue_WhenValid(int id, string name, string dob, string contact)
        {
            var patient = new Patient
            {
                PatientId = id,
                PatientName = name,
                DateOfBirth = DateTime.Parse(dob),
                PhoneNumber = contact
            };
            _mockRepo.Setup(r => r.UpdatePatient(patient)).Returns(true);
            var result = _service.UpdatePatient(patient);
            Assert.True(result);
        }
        [Fact]
        public void UpdatePatient_ThrowsException_WhenInvalid()
        {
            Assert.Throws<ArgumentException>(() => _service.UpdatePatient(new Patient { PatientId = 1, DateOfBirth = DateTime.Now }));
        }
    }
}
