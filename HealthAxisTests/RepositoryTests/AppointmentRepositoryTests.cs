using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxisTests.RepositoryTests
{
    public class AppointmentRepositoryTests
    {
        private readonly Database _db;
        private readonly AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _db = new Database();
            _repository = new AppointmentRepository(_db);
        }
        private static Patient CreatePatient(int id)

        {

            return new Patient
            {

                PatientId = id,

                PatientName = $"Patient{id}",
                DateOfBirth = DateTime.Now,
                Gender = Patient.Genders.Male,
                PhoneNumber = "9999999999",
                Email = $"p{id}@test.com",
                RegisteredDate = DateTime.Now
            };
        }

        private static Doctor CreateDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                DoctorName = $"Dr{id}",
                Specialisation = Doctor.Specialisations.Cardiologist,
                Experience = 10,
                Fees = 500,
                IsPractising = true
            };
        }
        [Fact]
        public void GetAllAppointments_ShouldReturnAllAppointments()
        {
            var appointments = _repository.GetAllAppointments();
            Assert.NotNull(appointments);
            Assert.Empty(appointments);
        }
        [Fact]
        public void AddAppointment_ShouldReturnAllAppointments()
        {
            var appointment = new Appointment
            {
                Patient = new Patient { PatientId = 6, PatientName = "John Doe" },
                Doctor = new Doctor { DoctorId = 9, DoctorName = "Dr. Smith" },
                ScheduledDate = DateTime.Now.AddDays(1),
                Slot = "10:00 AM - 10:30 AM"
            };
            Assert.NotNull(appointment);
            Assert.Equal(6, appointment.Patient.PatientId);
            Assert.Equal("John Doe", appointment.Patient.PatientName);
            Assert.Equal(9, appointment.Doctor.DoctorId);
            Assert.Equal("Dr. Smith", appointment.Doctor.DoctorName);
            Assert.Equal("10:00 AM - 10:30 AM", appointment.Slot);

        }
        [Fact]
        public void CancelAppointment_ShouldCancelAppointment()
        {
            var appointment = new Appointment
            {
                Patient = new Patient { PatientId = 6, PatientName = "John Doe" },
                Doctor = new Doctor { DoctorId = 9, DoctorName = "Dr. Smith" },
                ScheduledDate = DateTime.Now.AddDays(1),
                Slot = "10:00 AM - 10:30 AM"
            };
            var addedAppointment = _repository.AddAppointment(appointment);
            var result = _repository.CancelAppointment(addedAppointment.AppointmentId, "Patient is sick");
            Assert.True(result);
            var cancelledAppointment = _repository.GetAppointmentById(addedAppointment.AppointmentId);
            Assert.NotNull(cancelledAppointment);
            Assert.Equal(Appointment.AppointmentStatus.Cancelled, cancelledAppointment.Status);
            Assert.Equal("Patient is sick", cancelledAppointment.CancellationReason);
        }
        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnAppointmentsForPatient()
        {
            var appointment1 = new Appointment
            {
                Patient = new Patient { PatientId = 6, PatientName = "John Doe" },
                Doctor = new Doctor { DoctorId = 9, DoctorName = "Dr. Smith" },
                ScheduledDate = DateTime.Now.AddDays(1),
                Slot = "10:00 AM - 10:30 AM"
            };
            var appointment2 = new Appointment
            {
                Patient = new Patient { PatientId = 6, PatientName = "John Doe" },
                Doctor = new Doctor { DoctorId = 10, DoctorName = "Dr. Jones" },
                ScheduledDate = DateTime.Now.AddDays(2),
                Slot = "11:00 AM - 11:30 AM"
            };
            _repository.AddAppointment(appointment1);
            _repository.AddAppointment(appointment2);
            var appointmentsForPatient = _repository.GetAppointmentsByPatient(6);
            Assert.NotNull(appointmentsForPatient);
            Assert.Equal(2, appointmentsForPatient.Count);
        }
        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnAppointmentsForDoctor()
        {
            var appointment1 = new Appointment
            {
                Patient = new Patient { PatientId = 6, PatientName = "John Doe" },
                Doctor = new Doctor { DoctorId = 9, DoctorName = "Dr. Smith" },
                ScheduledDate = DateTime.Now.AddDays(1),
                Slot = "10:00 AM - 10:30 AM"
            };
            var appointment2 = new Appointment
            {
                Patient = new Patient { PatientId = 7, PatientName = "Jane Doe" },
                Doctor = new Doctor { DoctorId = 9, DoctorName = "Dr. Smith" },
                ScheduledDate = DateTime.Now.AddDays(2),
                Slot = "11:00 AM - 11:30 AM"
            };
            _repository.AddAppointment(appointment1);
            _repository.AddAppointment(appointment2);
            var appointmentsForDoctor = _repository.GetAppointmentsByDoctor(9);
            Assert.NotNull(appointmentsForDoctor);
            Assert.Equal(2, appointmentsForDoctor.Count);
        }
        [Fact]
        public void GetAll_ShouldReturnOrderedAppointments()
        {

            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            _repository.AddAppointment(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                Slot = "11:00 AM"
            });

            _repository.AddAppointment(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                Slot = "09:00 AM"
            });

            var result = _repository.GetAllAppointments();

            Assert.Equal(2, result.Count);
            Assert.Equal("09:00 AM", result.First().Slot);
        }
        [Fact]
        public void GetById_ShouldReturnAppointment()
        {

            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var appointment = _repository.AddAppointment(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                Slot = "09:00 AM"
            });

            var result = _repository.GetAppointmentById(appointment.AppointmentId);

            Assert.NotNull(result);
            Assert.Equal("09:00 AM", result.Slot);
        }
        [Fact]
        public void GetById_ShouldReturnNullForInvalidId()
        {
            var result = _repository.GetAppointmentById(999);
            Assert.Null(result);
        }
        [Fact]
        public void GetNextAvailableSlot_ShouldReturnNextAvailableSlot()
        {
            var doctor = CreateDoctor(1);
            var date = DateTime.Today.AddDays(1);
            _repository.AddAppointment(new Appointment
            {
                Patient = CreatePatient(1),
                Doctor = doctor,
                ScheduledDate = date,
                Slot = "09:00 AM"
            });
            var nextSlot = _repository.GetNextAvailableSlot(1, date);
            Assert.Equal("10:00 AM", nextSlot);
        }
        [Fact]
        public void GetNextAvailableSlotAvoidingPatientConflict()
        {
            int doctor1Id = 1;
            int patientId = 1;
            int doctor2Id = 1;
            var appointment1 = new Appointment
            {
                Patient = CreatePatient(patientId),
                Doctor = CreateDoctor(doctor1Id),
                ScheduledDate = DateTime.Today,
                Slot = "09:00 AM"
            };
            var book1=_repository.AddAppointment(appointment1);
            var nextSlot = _repository.GetNextAvailableSlotAvoidingPatientConflicts(doctor2Id, DateTime.Today, patientId);
            Assert.NotEqual("09:00 AM", nextSlot);
        }
        [Fact]
        public void GetBookedSlotCount_returnCount()
        {
            var doctor1Id = 1;
            var result = _repository.GetBookedSlotCount(doctor1Id, DateTime.Today);
            Assert.Equal(0, result);
        }
        [Fact]
        public void PatientHasAppointmentAt_ReturnTrue()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            _repository.AddAppointment(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                Slot = "11:00 AM"
            });
            var result= _repository.PatientHasAppointmentAt(1, DateTime.Today,"11:00 AM");
            Assert.True(result);
        }
        [Fact]
        public void RemoveAppointment_AddAndRemove()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var appointment=_repository.AddAppointment(new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Today,
                Slot = "11:00 AM"
            });
            Assert.NotNull(appointment);
            _repository.Remove(appointment);
            var result=_repository.GetAllAppointments();
            Assert.Empty(result);
        }
    }
}
