using System;
using System.Linq;
using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories.Impl;
using Xunit;

namespace HealthAxisTest.RepositoryTests
{
    public class DoctorRepositoryTests
    {
        private readonly AppDbContext _db;
        private readonly DoctorRepository _repo;

        public DoctorRepositoryTests()
        {
            _db = new AppDbContext();
            _repo = new DoctorRepository(_db);

           
            _db.Doctors.Clear();
        }

        [Fact]
        public void GetById_GivenId_ReturnsDoctor()
        {
            
            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Test",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            _repo.AddDoctor(doctor);

            
            var result = _repo.GetById(1);

            
            Assert.NotNull(result);
            Assert.Equal("Dr. Test", result.FullName);
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            
            var result = _repo.GetById(999);

            
            Assert.Null(result);
        }

        [Fact]
        public void AddDoctor_ShouldAddDoctorSuccessfully()
        {
            
            Doctor doctor = new Doctor
            {
                DoctorId = 2,
                FullName = "Dr. New Doctor",
                Specialisation = Doctor.SpecialisationOption.Neurologist,
                YearsOfExperience = 5,
                ConsultationFee = 400,
                IsActive = true
            };

            
            var result = _repo.AddDoctor(doctor);

            
            Assert.NotNull(result);
            Assert.Equal(doctor.DoctorId, result.DoctorId);

            var doctors = _repo.GetAllDoctors();
            Assert.Contains(doctors, d => d.DoctorId == 2);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_ShouldReturnMatchingDoctors()
        {
            
            Doctor doc1 = new Doctor
            {
                DoctorId = 3,
                FullName = "Dr. A",
                Specialisation = Doctor.SpecialisationOption.Dermatologist,
                YearsOfExperience = 8,
                ConsultationFee = 600,
                IsActive = true
            };

            Doctor doc2 = new Doctor
            {
                DoctorId = 4,
                FullName = "Dr. B",
                Specialisation = Doctor.SpecialisationOption.Dermatologist,
                YearsOfExperience = 6,
                ConsultationFee = 550,
                IsActive = true
            };

            _repo.AddDoctor(doc1);
            _repo.AddDoctor(doc2);

            
            var result = _repo.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Dermatologist);

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, d => Assert.Equal(Doctor.SpecialisationOption.Dermatologist, d.Specialisation));
        }
        [Fact]
        public void GetAllDoctors_ShouldReturnInsertedDoctors()
        {
            
            Doctor doc1 = new Doctor
            {
                DoctorId = 6,
                FullName = "Dr. Y",
                Specialisation = Doctor.SpecialisationOption.GeneralPractitioner,
                YearsOfExperience = 4,
                ConsultationFee = 300,
                IsActive = true
            };

            Doctor doc2 = new Doctor
            {
                DoctorId = 7,
                FullName = "Dr. Z",
                Specialisation = Doctor.SpecialisationOption.Oncologist,
                YearsOfExperience = 12,
                ConsultationFee = 900,
                IsActive = true
            };

            _repo.AddDoctor(doc1);
            _repo.AddDoctor(doc2);

            
            var result = _repo.GetAllDoctors();

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void UpdateDoctor_Existing_ShouldUpdate()
        {
            var doctor = new Doctor { DoctorId = 1, FullName = "Old" };
            _repo.AddDoctor(doctor);

            var updated = new Doctor { DoctorId = 1, FullName = "New" };

            var result = _repo.UpdateDoctor(updated);

            Assert.True(result);
            Assert.Equal("New", _repo.GetById(1).FullName);
        }

        [Fact]
        public void UpdateDoctor_NotFound_ShouldReturnFalse()
        {
            var result = _repo.UpdateDoctor(new Doctor { DoctorId = 999 });
            Assert.False(result);
        }
    }
}