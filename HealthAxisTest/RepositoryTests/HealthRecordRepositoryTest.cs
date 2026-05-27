using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repositories.Impl;
using HealthAxis.Services.Impl;
using System;
using System.Linq;
using Xunit;

namespace HealthAxisTest.RepositoryTests
{
    public class HealthRepositoryTests
    {
        private readonly AppDbContext _db;
        private readonly HealthRepository _repo;

        public HealthRepositoryTests()
        {
            _db = new AppDbContext();
            _repo = new HealthRepository(_db);

            _db.HealthRecords.Clear();
            _db.Doctors.Clear();
            _db.Patients.Clear();
        }

        private static Patient CreatePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = $"Patient{id}",
                DateOfBirth = DateTime.Now,
                Gender = Patient.GenderOptions.Male,
                PhoneNumber = "9999999999",
                Email = $"p{id}@test.com",
                CreatedDate = DateTime.Now
            };
        }

        private static Doctor CreateDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = $"Dr{id}",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        [Fact]
        public void AddRecord_ShouldCreateHealthRecord()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var record = new HealthRecord
            {
                Patient = patient,
                Doctor = doctor,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Rest"
            };

            
            var result = _repo.AddRecord(record);

            
            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
            Assert.Contains(_db.HealthRecords, r => r.RecordId == 1);
        }

        [Fact]
        public void GetRecordsByPatient_ShouldReturnMatchingRecords()
        {
            
            var patient1 = CreatePatient(1);
            var patient2 = CreatePatient(2);
            var doctor = CreateDoctor(1);

            _repo.AddRecord(new HealthRecord
            {
                Patient = patient1,
                Doctor = doctor,
                VisitDate = DateTime.Today,
                Diagnosis = "Cold",
                Prescription = "Medicine",
            });

            _repo.AddRecord(new HealthRecord
            {
                Patient = patient2,
                Doctor = doctor,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Tablet",
            });

            
            var result = _repo.GetRecordsByPatient(1);

            
            Assert.Single(result);
            Assert.Equal(1, result[0].Patient.PatientId);
        }

        [Fact]
        public void GetRecordsByDoctor_ShouldReturnMatchingRecords()
        {
            
            var patient = CreatePatient(1);
            var doctor1 = CreateDoctor(1);
            var doctor2 = CreateDoctor(2);

            _repo.AddRecord(new HealthRecord
            {
                Patient = patient,
                Doctor = doctor1,
                VisitDate = DateTime.Today,
                Diagnosis = "Cold",
                Prescription = "Medicine",
            });

            _repo.AddRecord(new HealthRecord
            {
                Patient = patient,
                Doctor = doctor2,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Tablet",
            });

            
            var result = _repo.GetRecordsByDoctor(1);

            
            Assert.Single(result);
            Assert.Equal(1, result[0].Doctor.DoctorId);
        }
    }
}