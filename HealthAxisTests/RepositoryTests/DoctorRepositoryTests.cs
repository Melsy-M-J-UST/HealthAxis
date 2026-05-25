using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxisTests.RepositoryTests
{
    public class DoctorRepositoryTests
    {
        private readonly Database _db;
        private readonly DoctorRepository _repository;

        public DoctorRepositoryTests()
        {
            _db = new Database();
            _repository = new DoctorRepository(_db);
        }

        [Fact]
        public void AddDoctor_WhenDone_ShouldBeAdded()
        {
            Doctor d = new Doctor
            {
                DoctorId = _db.GetNextDoctorId(),
                DoctorName = "Dr. Chitresh Zope",
                Specialisation = Doctor.Specialisations.Cardiologist,
                Experience = 14,
                Fees = 900,
                IsPractising = true
            };
            var doctor = _repository.AddDoctor(d);
            Assert.NotNull(doctor);
            Assert.Equal(9, doctor.DoctorId);
            Assert.Equal(9, _db.Doctors.Count);
        }

        [Fact]
        public void GetAllDoctors_ShouldRetrieveAllDoctors()
        {
            var result = _repository.GetAllDoctors();

            Assert.NotNull(result);
            Assert.Equal(8, result.Count);
            Assert.Equal("Dr. Suresh Mathew", result[1].DoctorName);
        }

        [Fact]
        public void ShouldReturnAllDoctorsGivenSpecialization()
        {
            Enum.TryParse("Pediatrician", true, out Doctor.Specialisations spec);
            var result = _repository.SearchDoctorBySpecialisation(spec);
            Assert.NotNull(result);
            Assert.Equal("Dr. Neha Iyer", result[0].DoctorName);
        }
        [Fact]
        public void GetDoctorById_GivenValidId_ShouldReturnDoctor()
        {
            var doctor = _repository.GetDoctorById(1);
            Assert.NotNull(doctor);
            Assert.Equal("Dr. Priya Sharma", doctor.DoctorName);
        }
        [Fact]
        public void GetDoctorById_GivenInvalidId_ShouldReturnNull()
        {
            var doctor = _repository.GetDoctorById(999);
            Assert.Null(doctor);
        }
        [Fact]
        public void UpdateDoctor_GivenValidDoctor_ShouldUpdateDoctor()
        {
            Doctor updatedDoctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Dr. Priya Sharma Updated",
                Specialisation = Doctor.Specialisations.Cardiologist,
                Experience = 12,
                Fees = 800,
                IsPractising = true
            };
            var result = _repository.UpdateDoctor(updatedDoctor);
            Assert.True(result);
            var doctor = _repository.GetDoctorById(1);
            Assert.Equal("Dr. Priya Sharma Updated", doctor?.DoctorName);
        }
    }
}
