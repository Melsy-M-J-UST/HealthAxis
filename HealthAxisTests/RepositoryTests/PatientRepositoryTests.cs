using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repository.Implementation;

namespace HealthAxisTests.RepositoryTests
{
    public class PatientRepositoryTests
    {
        private readonly Database _db;
        private readonly PatientRepository _repository;
        public PatientRepositoryTests()
        {
            _db = new Database();
            _repository = new PatientRepository(_db);
        }
        [Fact]
        public void GetAllPatients()
        {
            var patients = _repository.GetAllPatients();
            Assert.NotNull(patients);
            Assert.Equal(5, patients.Count);
            Assert.Equal("Arun Kumar", patients[0].PatientName);
        }

        [Fact]
        public void GetById_GivenId_GetPatientWithSameID()
        {
            var patient = _repository.GetPatientById(1);
            Assert.NotNull(patient);
            Assert.Equal("Arun Kumar", patient.PatientName);
        }
        [Fact]
        public void RegisterPatient_GivenId_RegisterPatientWithSameID()
        {
            Patient p = new Patient
            {
                PatientId = 6,
                PatientName = "Mathew.Abraham",
                DateOfBirth = new DateTime(2006, 6, 12),
                Gender = Patient.Genders.Other,
                PhoneNumber = "9864108247",
                Email = "Mathew.Abraham@example.com",
                InsuranceId = "",
                RegisteredDate = DateTime.Now
            };
            var result = _repository.RegisterPatient(p);
            Assert.NotNull(result);
            Assert.Equal(p, result);
            var patients = _repository.GetAllPatients();
            Assert.Equal(6, patients.Count);
        }
        [Fact]
        public void GetById_GivenInvalidId_ShouldReturnNull()
        {
            var patient = _repository.GetPatientById(999);
            Assert.Null(patient);
        }
        [Fact]
        public void UpdatePatient_GivenValidPatient_ShouldUpdatePatient()
        {
            Patient updatedPatient = new Patient
            {
                PatientId = 1,
                PatientName = "Arun Kumar S",
                DateOfBirth = new DateTime(1992, 5, 14),
            };
            var result = _repository.UpdatePatient(updatedPatient);
            Assert.True(result);
        }
        [Theory]
        [InlineData(999, "Non Existent", "2000-01-01")]
        [InlineData(0, "", "1992-05-14")]
        public void UpdatePatient_GivenInvalidPatient_ShouldReturnFalse(int id, string name, string dob)
        {
            Patient updatedPatient = new Patient
            {
                PatientId = id,
                PatientName = name,
                DateOfBirth = DateTime.Parse(dob),
            };
            var result = _repository.UpdatePatient(updatedPatient);
            Assert.False(result);
        }
        }
} 
