using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repositories.Impl;

namespace AppointmentPortal.Tests.RepositoryTests
{
    public class PatientRepositoryTests
    {
        private readonly Database _db;
        private readonly PatientRepository _repo;
        public PatientRepositoryTests()
        {
            _db = new Database();
            _repo = new PatientRepository(_db);
        }
        [Fact]
        public void GetAllPatients()
        {
            var patients = _repo.GetAllPatients();
            Assert.NotNull(patients);
            Assert.Equal(5, patients.Count);
            Assert.Equal("Arun Kumar", patients[0].FullName);
        }

        [Fact]
        public void GetById_GivenId_GetPatientWithSameID()
        {
            var patient = _repo.GetPatientById(1);
            Assert.NotNull(patient);
            Assert.Equal("Arun Kumar", patient.FullName);
        }
        [Fact]
        public void RegisterPatient_GivenId_RegisterPatientWithSameID()
        {
            Patient p = new Patient
            {
                PatientId = 6,
                FullName = "Mathew.Abraham",
                DateOfBirth = new DateTime(2006, 6, 12),
                Gender = Patient.GenderOptions.Other,
                PhoneNumber = "9864108247",
                Email = "Mathew.Abraham@example.com",
                InsuranceID = "",
                CreatedDate = DateTime.Now
            };
            var result = _repo.RegisterPatient(p);
            Assert.NotNull(result);
            Assert.Equal(p, result);
            var patients = _repo.GetAllPatients();
            Assert.Equal(6, patients.Count);
        }
    }
}
