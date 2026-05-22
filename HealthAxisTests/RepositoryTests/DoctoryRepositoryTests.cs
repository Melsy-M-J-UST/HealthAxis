using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentPortal.Tests.RepositoryTests
{
    public class DoctorRepositoryTests
    {
        private readonly Database _dbcontext;
        private readonly DoctorRepository _repo;

        public DoctorRepositoryTests()
        {
            _dbcontext = new Database();
            _repo = new DoctorRepository(_dbcontext);
        }

        [Fact]
        public void AddDoctor_WhenDone_ShouldBeAdded()
        {
            Doctor d = new Doctor
            {
                DoctorId = _dbcontext.GetNextDoctorId(),
                FullName = "Dr. Chitresh Zope",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 14,
                ConsultationFee = 900,
                IsActive = true
            };
            var doctor = _repo.AddDoctor(d);
            Assert.NotNull(doctor);
            Assert.Equal(9, doctor.DoctorId);
            Assert.Equal(9, _dbcontext.Doctors.Count);
        }

        [Fact]
        public void GetAllDoctors_ShouldRetrieveAllDoctors()
        {
            var result = _repo.GetAllDoctors();

            Assert.NotNull(result);
            Assert.Equal(8, result.Count);
            Assert.Equal("Dr. Suresh Mathew", result[1].FullName);
        }

        [Fact]
        public void ShouldReturnAllDoctorsGivenSpecialization()
        {
            Enum.TryParse("Pediatrician", true, out Doctor.SpecialisationOption spec);
            var result = _repo.SearchDoctorBySpecialisation(spec);
            Assert.NotNull(result);
            Assert.Equal("Dr. Neha Iyer", result[0].FullName);
        }
    }
}
