using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services.Impl;
using Moq;
using System.Collections.Generic;
using Xunit;

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
                new Doctor { DoctorId = 1, FullName = "Dr A" },
                new Doctor { DoctorId = 2, FullName = "Dr B" }
            };

            _mockRepo.Setup(r => r.GetAllDoctors()).Returns(doctors);

            var result = _service.GetAllDoctors();

            Assert.Equal(2, result.Count);
            Assert.Equal("Dr A", result[0].FullName);

            _mockRepo.Verify(r => r.GetAllDoctors(), Times.Once);
        }

        [Fact]
        public void AddDoctor_ReturnsDoctor()
        {
            var doctor = new Doctor { DoctorId = 1, FullName = "Dr X" };

            _mockRepo.Setup(r => r.AddDoctor(doctor)).Returns(doctor);

            var result = _service.AddDoctor(doctor);

            Assert.NotNull(result);
            Assert.Equal("Dr X", result.FullName);

            _mockRepo.Verify(r => r.AddDoctor(doctor), Times.Once);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_ReturnsDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { FullName = "Cardio Doc", Specialisation = Doctor.SpecialisationOption.Cardiologist }
            };

            _mockRepo.Setup(r =>
                r.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Cardiologist))
                .Returns(doctors);

            var result = _service.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Cardiologist);

            Assert.Single(result);
            Assert.Equal("Cardio Doc", result[0].FullName);
        }

        [Fact]
        public void GetById_NotFound_ShouldThrow()
        {
            _mockRepo.Setup(r => r.GetById(1)).Returns((Doctor?)null);

            Assert.Throws<DoctorNotFoundException>(() => _service.GetById(1));
        }
        [Fact]
        public void GetById_ValidId_ShouldReturnDoctor()
        {
            var doctor = new Doctor { DoctorId = 1, FullName = "Dr A" };

            _mockRepo.Setup(r => r.GetById(1))
                     .Returns(doctor);

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Dr A", result.FullName);

            _mockRepo.Verify(r => r.GetById(1), Times.Once);
        }
        [Fact]
        public void SearchDoctorBySpecialisation_NoDoctors_ShouldThrow()
        {
            _mockRepo.Setup(r => r.SearchDoctorBySpecialisation(It.IsAny<Doctor.SpecialisationOption>()))
                     .Returns(new List<Doctor>());

            Assert.Throws<DoctorNotFoundException>(() =>
                _service.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Cardiologist));
        }

    }
}