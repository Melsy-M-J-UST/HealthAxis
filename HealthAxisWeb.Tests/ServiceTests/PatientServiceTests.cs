using System;
using System.Collections.Generic;
using HealthAxis.Shared.Services.Impl;
using HealthAxisWebApp;
using HealthAxisWebApp.Repositories.Interfaces;
using Moq;
using Xunit;

namespace HealthAxis.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> patientRepositoryMock;
        private readonly PatientService patientService;

        public PatientServiceTests()
        {
            patientRepositoryMock = new Mock<IPatientRepository>();
            patientService = new PatientService(patientRepositoryMock.Object);
        }

        [Fact]
        public void GetAllPatients_ReturnsPatients()
        {
            var patients = new List<Patient>
            {
                CreateValidPatient(1)
            };

            patientRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(patients);

            var result = patientService.GetAllPatients();

            Assert.Single(result);
            Assert.Equal(1, result[0].PatientId);
            Assert.Equal("Arun Kumar", result[0].FullName);
        }

        [Fact]
        public void GetPatientById_WhenPatientExists_ReturnsPatient()
        {
            var patient = CreateValidPatient(1);

            patientRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(patient);

            var result = patientService.GetPatientById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public void GetPatientById_WhenPatientDoesNotExist_ReturnsNull()
        {
            patientRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Patient)null);

            var result = patientService.GetPatientById(99);

            Assert.Null(result);
        }

        [Fact]
        public void AddPatient_WhenPatientIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                patientService.AddPatient(null));
        }

        [Fact]
        public void AddPatient_WhenNameIsEmpty_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.FullName = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenNameHasInvalidCharacters_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.FullName = "Arun123";

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenEmailIsEmpty_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.Email = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenEmailFormatInvalid_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.Email = "invalid-email";

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenPhoneIsEmpty_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.PhoneNumber = string.Empty;

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenPhoneInvalid_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.PhoneNumber = "12345";

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenDateOfBirthIsTodayOrFuture_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.DateOfBirth = DateTime.Today;

            Assert.Throws<ArgumentException>(() =>
                patientService.AddPatient(patient));
        }

        [Fact]
        public void AddPatient_WhenValid_SetsCreatedDate()
        {
            var patient = CreateValidPatient();
            var before = DateTime.Now.AddSeconds(-1);

            patientService.AddPatient(patient);

            var after = DateTime.Now.AddSeconds(1);

            Assert.InRange(patient.CreatedDate, before, after);
        }

        [Fact]
        public void AddPatient_WhenValid_CallsRepositoryAdd()
        {
            var patient = CreateValidPatient();

            patientService.AddPatient(patient);

            patientRepositoryMock.Verify(
                r => r.Add(patient),
                Times.Once);
        }

        [Fact]
        public void UpdatePatient_WhenPatientIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                patientService.UpdatePatient(null));
        }

        [Fact]
        public void UpdatePatient_WhenInvalid_ThrowsArgumentException()
        {
            var patient = CreateValidPatient();
            patient.Email = "wrong-email";

            Assert.Throws<ArgumentException>(() =>
                patientService.UpdatePatient(patient));
        }

        [Fact]
        public void UpdatePatient_WhenValid_CallsRepositoryUpdate()
        {
            var patient = CreateValidPatient(1);

            patientService.UpdatePatient(patient);

            patientRepositoryMock.Verify(
                r => r.Update(patient),
                Times.Once);
        }

        [Fact]
        public void DeletePatient_WhenPatientDoesNotExist_ThrowsKeyNotFoundException()
        {
            patientRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Patient)null);

            Assert.Throws<KeyNotFoundException>(() =>
                patientService.DeletePatient(99));
        }

        [Fact]
        public void DeletePatient_WhenPatientExists_CallsRepositoryDelete()
        {
            var patient = CreateValidPatient(1);

            patientRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(patient);

            patientService.DeletePatient(1);

            patientRepositoryMock.Verify(
                r => r.Delete(1),
                Times.Once);
        }

        private static Patient CreateValidPatient(int id = 0)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Arun Kumar",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = 0,
                PhoneNumber = "9876543210",
                Email = "arun@example.com",
                InsuranceID = "INS1001",
                CreatedDate = DateTime.Now
            };
        }
    }
}