using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Moq;
using Xunit;
using HealthAxis.Exceptions;

namespace HealthAxisTests.ServiceTests
{
    public class DoctorServiceTests
    {
        private Mock<IDoctorRepository> _mockRepo;
        private DoctorService _service;

        public DoctorServiceTests()
        {
            _mockRepo = new Mock<IDoctorRepository>();
            _service = new DoctorService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllDoctors_ReturnsList()
        {
            var doctors = new List<Doctor>
        {
            new Doctor { DoctorId = 1, DoctorName = "Dr A" },
            new Doctor { DoctorId = 2, DoctorName = "Dr B" }
        };

            _mockRepo.Setup(r => r.GetAllDoctors()).Returns(doctors);

            var result = _service.GetAllDoctors();

            Assert.Equal(2, result.Count);
            Assert.Equal("Dr A", result[0].DoctorName);

            _mockRepo.Verify(r => r.GetAllDoctors(), Times.Once);
        }

        [Fact]
        public void AddDoctor_ReturnsDoctor()
        {
            var doctor = new Doctor { DoctorId = 1, DoctorName = "Dr X" };

            _mockRepo.Setup(r => r.AddDoctor(doctor)).Returns(doctor);

            var result = _service.AddDoctor(doctor);

            Assert.NotNull(result);
            Assert.Equal("Dr X", result.DoctorName);

            _mockRepo.Verify(r => r.AddDoctor(doctor), Times.Once);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_ReturnsDoctors()
        {
            var doctors = new List<Doctor>
        {
            new Doctor { DoctorName = "Cardio Doc", Specialisation = Doctor.Specialisations.Cardiologist }
        };

            _mockRepo.Setup(r =>
                r.SearchDoctorBySpecialisation(Doctor.Specialisations.Cardiologist))
                .Returns(doctors);

            var result = _service.SearchDoctorBySpecialisation(Doctor.Specialisations.Cardiologist);

            Assert.Single(result);
            Assert.Equal("Cardio Doc", result[0].DoctorName);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(111)]
        [InlineData(999)]
        [InlineData(11)]
        [InlineData(56)]
        public void GetDoctorById_InvalidId_ThrowsException(int doctorId)
        {
            var exception = Assert.Throws<DoctorNotFoundException>(() => _service.GetDoctorById(doctorId));
            Assert.NotNull(exception);
        }
        [Fact]
        public void UpgradeDoctorBySpecialisation_ValidDoctor_ReturnsTrue()
        {
            var doctor = new Doctor { DoctorId = 1, DoctorName = "Dr Y" };
            _mockRepo.Setup(r => r.UpdateDoctor(doctor)).Returns(true);
            var result = _service.UpdateDoctor(doctor);
            Assert.True(result);
            _mockRepo.Verify(r => r.UpdateDoctor(doctor), Times.Once);
        }
        [Fact]
        public void UpgradeDoctorBySpecialisation_NullDoctor_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _service.UpdateDoctor(null!));
        }
    }
}