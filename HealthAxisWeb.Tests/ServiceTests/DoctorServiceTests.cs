using System;
using System.Collections.Generic;
using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Impl;
using HealthAxisWebApp.Repositories.Interfaces;
using Moq;
using Xunit;

namespace HealthAxis.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> doctorRepositoryMock;
        private readonly DoctorService doctorService;

        public DoctorServiceTests()
        {
            doctorRepositoryMock = new Mock<IDoctorRepository>();
            doctorService = new DoctorService(doctorRepositoryMock.Object);
        }

        [Fact]
        public void GetAllDoctors_ReturnsDoctors()
        {
            var doctors = new List<Doctor>
            {
                CreateValidDoctor(1)
            };

            doctorRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(doctors);

            var result = doctorService.GetAllDoctors();

            Assert.Single(result);
            Assert.Equal(1, result[0].DoctorId);
            Assert.Equal("Amit Verma", result[0].FullName);
        }

        [Fact]
        public void GetDoctorById_WhenDoctorExists_ReturnsDoctor()
        {
            var doctor = CreateValidDoctor(1);

            doctorRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(doctor);

            var result = doctorService.GetDoctorById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);
        }

        [Fact]
        public void GetDoctorById_WhenDoctorDoesNotExist_ReturnsNull()
        {
            doctorRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Doctor)null);

            var result = doctorService.GetDoctorById(99);

            Assert.Null(result);
        }

        [Fact]
        public void GetUpcomingAppointmentCount_ReturnsRepositoryValue()
        {
            doctorRepositoryMock
                .Setup(r => r.GetUpcomingAppointmentCount(5))
                .Returns(3);

            var result = doctorService.GetUpcomingAppointmentCount(5);

            Assert.Equal(3, result);
        }

        [Fact]
        public void AddDoctor_WhenDoctorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                doctorService.AddDoctor(null));
        }

        [Fact]
        public void AddDoctor_WhenNameIsEmpty_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor();
            doctor.FullName = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                doctorService.AddDoctor(doctor));
        }

        [Fact]
        public void AddDoctor_WhenNameHasInvalidCharacters_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor();
            doctor.FullName = "Dr123";

            Assert.Throws<ArgumentException>(() =>
                doctorService.AddDoctor(doctor));
        }

        [Fact]
        public void AddDoctor_WhenNameHasOnlyLettersAndSpaces_DoesNotThrow()
        {
            var doctor = CreateValidDoctor();
            doctor.FullName = "Amit Verma";

            var exception = Record.Exception(() => doctorService.AddDoctor(doctor));

            Assert.Null(exception);
        }

        [Fact]
        public void AddDoctor_WhenExperienceNegative_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor();
            doctor.YearsOfExperience = -1;

            Assert.Throws<ArgumentException>(() =>
                doctorService.AddDoctor(doctor));
        }

        [Fact]
        public void AddDoctor_WhenConsultationFeeZeroOrLess_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor();
            doctor.ConsultationFee = 0;

            Assert.Throws<ArgumentException>(() =>
                doctorService.AddDoctor(doctor));
        }

        [Fact]
        public void AddDoctor_WhenConsultationFeeNegative_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor();
            doctor.ConsultationFee = -100;

            Assert.Throws<ArgumentException>(() =>
                doctorService.AddDoctor(doctor));
        }


        [Fact]
        public void AddDoctor_WhenValid_CallsRepositoryAdd()
        {
            var doctor = CreateValidDoctor();

            doctorService.AddDoctor(doctor);

            doctorRepositoryMock.Verify(
                r => r.Add(doctor),
                Times.Once);
        }

        [Fact]
        public void UpdateDoctor_WhenDoctorIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                doctorService.UpdateDoctor(null));
        }

        [Fact]
        public void UpdateDoctor_WhenInvalidFee_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor();
            doctor.ConsultationFee = -100;

            Assert.Throws<ArgumentException>(() =>
                doctorService.UpdateDoctor(doctor));
        }

        [Fact]
        public void UpdateDoctor_WhenNameHasInvalidCharacters_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor(1);
            doctor.FullName = "Doctor9";

            Assert.Throws<ArgumentException>(() =>
                doctorService.UpdateDoctor(doctor));
        }

        [Fact]
        public void UpdateDoctor_WhenExperienceNegative_ThrowsArgumentException()
        {
            var doctor = CreateValidDoctor(1);
            doctor.YearsOfExperience = -5;

            Assert.Throws<ArgumentException>(() =>
                doctorService.UpdateDoctor(doctor));
        }


        [Fact]
        public void UpdateDoctor_WhenValid_CallsRepositoryUpdate()
        {
            var doctor = CreateValidDoctor(1);

            doctorService.UpdateDoctor(doctor);

            doctorRepositoryMock.Verify(
                r => r.Update(doctor),
                Times.Once);
        }

        [Fact]
        public void DeleteDoctor_WhenDoctorDoesNotExist_ThrowsKeyNotFoundException()
        {
            doctorRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Doctor)null);

            Assert.Throws<KeyNotFoundException>(() =>
                doctorService.DeleteDoctor(99));
        }

        [Fact]
        public void DeleteDoctor_WhenDoctorExists_CallsRepositoryDelete()
        {
            var doctor = CreateValidDoctor(1);

            doctorRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(doctor);

            doctorService.DeleteDoctor(1);

            doctorRepositoryMock.Verify(
                r => r.Delete(1),
                Times.Once);
        }

        private static Doctor CreateValidDoctor(int id = 0)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Amit Verma",
                Specialisation = 1,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }
    }
}