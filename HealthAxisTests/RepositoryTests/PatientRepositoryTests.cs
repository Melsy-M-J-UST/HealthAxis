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
    }
}
