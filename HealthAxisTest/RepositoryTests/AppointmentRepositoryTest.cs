using System;
using System.Linq;
using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repositories;
using Xunit;

namespace HealthAxisTest.RepositoryTests
{
    public class AppointmentRepositoryTests
    {
        private readonly AppDbContext _db;
        private readonly AppointmentRepository _repo;

        public AppointmentRepositoryTests()
        {
            _db = new AppDbContext();
            _repo = new AppointmentRepository(_db);

    
            _db.Appointments.Clear();
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
        public void Add_ShouldCreateAppointment()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var appointment = new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM"
            };

            
            var result = _repo.Add(appointment);

            
            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
            Assert.Contains(_db.Appointments, a => a.AppointmentId == 1);
            Assert.Contains(doctor.Appointments, a => a.AppointmentId == 1);
        }

        [Fact]
        public void GetById_ShouldReturnAppointment()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var appointment = _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM"
            });

            
            var result = _repo.GetById(appointment.AppointmentId);

            
            Assert.NotNull(result);
            Assert.Equal("09:00 AM", result.TimeSlot);
        }

        [Fact]
        public void GetAll_ShouldReturnOrderedAppointments()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "11:00 AM"
            });

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM"
            });

            
            var result = _repo.GetAll();

            
            Assert.Equal(2, result.Count);
            Assert.Equal("09:00 AM", result.First().TimeSlot);
        }

        [Fact]
        public void GetByPatientId_ShouldReturnAppointments()
        {
            
            var patient1 = CreatePatient(1);
            var patient2 = CreatePatient(2);
            var doctor = CreateDoctor(1);

            _repo.Add(new Appointment
            {
                Patient = patient1,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM"
            });

            _repo.Add(new Appointment
            {
                Patient = patient2,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "10:00 AM"
            });

            
            var result = _repo.GetByPatientId(1);

            
            Assert.Single(result);
            Assert.Equal(1, result[0].Patient.PatientId);
        }

        [Fact]
        public void GetByDoctorId_ShouldReturnAppointments()
        {
            
            var patient = CreatePatient(1);
            var doctor1 = CreateDoctor(1);
            var doctor2 = CreateDoctor(2);

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor1,
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM"
            });

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor2,
                ScheduledDate = DateTime.Today,
                TimeSlot = "10:00 AM"
            });

            
            var result = _repo.GetByDoctorId(1);

            
            Assert.Single(result);
            Assert.Equal(1, result[0].Doctor.DoctorId);
        }

        [Fact]
        public void GetNextAvailableSlot_ShouldReturnFirstFreeSlot()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today;

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = "09:00 AM"
            });

            
            var slot = _repo.GetNextAvailableSlot(1, date);

            
            Assert.NotNull(slot);
            Assert.NotEqual("09:00 AM", slot);
        }

        [Fact]
        public void GetBookedSlotCount_ShouldReturnCount()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today;

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = "09:00 AM"
            });

            
            var count = _repo.GetBookedSlotCount(1, date);

            
            Assert.Equal(1, count);
        }

        [Fact]
        public void Remove_ShouldDeleteAppointment()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var appointment = _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM"
            });

            
            _repo.Remove(appointment);

            
            Assert.Empty(_db.Appointments);
            Assert.Empty(doctor.Appointments);
        }

        [Fact]
        public void PatientHasAppointmentAt_ShouldReturnTrue()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today;

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = "09:00 AM"
            });

            
            var result = _repo.PatientHasAppointmentAt(1, date, "09:00 AM");

            
            Assert.True(result);
        }

        [Fact]
        public void GetNextAvailableSlotAvoidingPatientConflicts_ShouldSkipPatientSlot()
        {
            
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today;

            _repo.Add(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = "09:00 AM"
            });

            
            var slot = _repo.GetNextAvailableSlotAvoidingPatientConflicts(1, date, 1);

            
            Assert.NotNull(slot);
            Assert.NotEqual("09:00 AM", slot);
        }
    }
}