using System;
using System.Linq;
using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories.Impl;
using Xunit;

namespace HealthAxisTest.RepositoryTests
{
    public class PatientRepositoryTests
    {
        private readonly AppDbContext _db;
        private readonly PatientRepository _repo;

        public PatientRepositoryTests()
        {
            _db = new AppDbContext();
            _repo = new PatientRepository(_db);
        }

        [Fact]
        public void GetById_GivenId_GetPatientWithSameID()
        {
            
            Patient p = new Patient
            {
                PatientId = 1,
                FullName = "Arun Kumar",
                DateOfBirth = new DateTime(1995, 5, 10),
                Gender = Patient.GenderOptions.Male,
                PhoneNumber = "9999999999",
                Email = "arun@example.com",
                CreatedDate = DateTime.Now
            };

            _repo.RegisterPatient(p);

            
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
                FullName = "Mathew Abraham",
                DateOfBirth = new DateTime(2006, 6, 12),
                Gender = Patient.GenderOptions.Other,
                PhoneNumber = "9864108247",
                Email = "mathew.abraham@example.com",
                InsuranceId = "",
                CreatedDate = DateTime.Now
            };

            
            var result = _repo.RegisterPatient(p);

            
            Assert.NotNull(result);
            Assert.Equal(p.PatientId, result.PatientId);

            var patients = _repo.GetAllPatients();
            Assert.Contains(patients, x => x.PatientId == 6);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnInsertedPatients()
        {
            
            Patient p1 = new Patient
            {
                PatientId = 2,
                FullName = "Test User 1",
                DateOfBirth = DateTime.Now,
                Gender = Patient.GenderOptions.Other,
                PhoneNumber = "1111111111",
                Email = "test1@example.com",
                CreatedDate = DateTime.Now
            };

            Patient p2 = new Patient
            {
                PatientId = 3,
                FullName = "Test User 2",
                DateOfBirth = DateTime.Now,
                Gender = Patient.GenderOptions.Other,
                PhoneNumber = "2222222222",
                Email = "test2@example.com",
                CreatedDate = DateTime.Now
            };

            _repo.RegisterPatient(p1);
            _repo.RegisterPatient(p2);

            
            var patients = _repo.GetAllPatients();

            
            Assert.NotNull(patients);
            Assert.Contains(patients, x => x.PatientId == 2);
            Assert.Contains(patients, x => x.PatientId == 3);
        }
    }
}