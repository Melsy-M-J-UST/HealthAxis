using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services;
using HealthAxis.Exceptions;

namespace HealthAxisTest.ServiceTests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_repoMock.Object);
        }

        private static Patient CreatePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Test Patient"
            };
        }

        private static Doctor CreateDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Test Doctor",
                IsActive = true
            };
        }

        [Fact]
        public void BookAppointment_ValidSlot_ShouldCreateAppointment()
        {
           
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today.AddDays(1);

            _repoMock.Setup(r => r.GetByPatientId(1))
                .Returns(new List<Appointment>());

            _repoMock.Setup(r => r.GetNextAvailableSlotAvoidingPatientConflicts(1, date, 1))
                .Returns("09:00 AM");

            _repoMock.Setup(r => r.PatientHasAppointmentAt(1, date, "09:00 AM"))
                .Returns(false);

            _repoMock.Setup(r => r.Add(It.IsAny<Appointment>()))
                .Returns((Appointment a) => a);

            
            var result = _service.BookAppointment(patient, doctor, date);

           
            Assert.NotNull(result);
            Assert.Equal("09:00 AM", result.TimeSlot);
            Assert.Equal(Appointment.StatusOption.Pending, result.Status);

            _repoMock.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Once);
        }

        
        [Fact]
        public void BookAppointment_PastDate_ShouldThrowPastDateException()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var pastDate = DateTime.Today.AddDays(-1);

            Assert.Throws<PastDateException>(() =>
                _service.BookAppointment(patient, doctor, pastDate)
            );
        }

        [Fact]
        public void BookAppointment_SlotAlreadyTaken_ShouldThrowConflictException()
        {
           
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today.AddDays(1);

            _repoMock.Setup(r => r.GetByPatientId(1))
                .Returns(new List<Appointment>());

            _repoMock.Setup(r => r.GetNextAvailableSlotAvoidingPatientConflicts(1, date, 1))
                .Returns("09:00 AM");

            _repoMock.Setup(r => r.PatientHasAppointmentAt(1, date, "09:00 AM"))
                .Returns(true);

            
            Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );
        }

        
        [Fact]
        public void CancelAppointment_ExistingId_ShouldUpdateStatusToCancelled()
        {
          
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = Appointment.StatusOption.Confirmed
            };

            _repoMock.Setup(r => r.GetById(1))
                .Returns(appointment);

            
            var result = _service.CancelAppointment(1, "Not needed");

           
            Assert.True(result);
            Assert.Equal(Appointment.StatusOption.Cancelled, appointment.Status);

            _repoMock.Verify(r => r.Remove(appointment), Times.Once);
        }
        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnOnlyPatientAppointments()
        {
            
            var patient = CreatePatient(1);

            var appointments = new List<Appointment>
            {
                new Appointment { Patient = patient },
                new Appointment { Patient = patient }
            };

            _repoMock.Setup(r => r.GetByPatientId(1))
                .Returns(appointments);

            
            var result = _service.GetAppointmentsByPatient(1);

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(1, a.Patient.PatientId));
        }
        [Fact]
        public void BookAppointment_DoctorInactive_ShouldThrow()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            doctor.IsActive = false;

            Assert.Throws<DoctorUnavailableException>(() =>
                _service.BookAppointment(patient, doctor, DateTime.Today.AddDays(1)));
        }

        [Fact]
        public void BookAppointment_OnSunday_ShouldThrow()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);

            var sunday = DateTime.Today.AddDays((7 - (int)DateTime.Today.DayOfWeek) % 7);

            Assert.Throws<DoctorUnavailableException>(() =>
                _service.BookAppointment(patient, doctor, sunday));
        }

        [Fact]
        public void BookAppointment_NoSlots_ShouldThrow()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today.AddDays(1);

            _repoMock.Setup(r => r.GetByPatientId(1)).Returns(new List<Appointment>());
            _repoMock.Setup(r => r.GetNextAvailableSlotAvoidingPatientConflicts(1, date, 1)).Returns((string)null!);
            _repoMock.Setup(r => r.GetNextAvailableSlot(1, date)).Returns((string)null!);

            Assert.Throws<DoctorUnavailableException>(() =>
                _service.BookAppointment(patient, doctor, date));
        }
        [Fact]
        public void BookAppointment_NullPatient_ShouldThrow()
        {
            var doctor = CreateDoctor(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                _service.BookAppointment(null!, doctor, DateTime.Today.AddDays(1))
            );

            Assert.Contains("Patient is required", ex.Message);
        }
        [Fact]
        public void BookAppointment_NullDoctor_ShouldThrow()
        {
            var patient = CreatePatient(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                _service.BookAppointment(patient, null!, DateTime.Today.AddDays(1))
            );

            Assert.Contains("Doctor is required", ex.Message);
        }
        [Fact]
        public void BookAppointment_ExistingDoctorConflict_ShouldThrow()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today.AddDays(1);

            var existingAppointments = new List<Appointment>
    {
        new Appointment
        {
            Doctor = doctor
        }
    };

            _repoMock.Setup(r => r.GetByPatientId(1))
                .Returns(existingAppointments);

            var ex = Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );

            Assert.Contains("already has an appointment", ex.Message);
        }
        [Fact]
        public void BookAppointment_FallbackSlot_ShouldUseAlternateSlot()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = DateTime.Today.AddDays(1);

            _repoMock.Setup(r => r.GetByPatientId(1))
                .Returns(new List<Appointment>());

            _repoMock.Setup(r => r.GetNextAvailableSlotAvoidingPatientConflicts(1, date, 1))
                .Returns((string)null!);

            _repoMock.Setup(r => r.GetNextAvailableSlot(1, date))
                .Returns("10:00 AM");

            _repoMock.Setup(r => r.PatientHasAppointmentAt(1, date, "10:00 AM"))
                .Returns(false);

            _repoMock.Setup(r => r.Add(It.IsAny<Appointment>()))
                .Returns((Appointment a) => a);

            var result = _service.BookAppointment(patient, doctor, date);

            Assert.NotNull(result);
            Assert.Equal("10:00 AM", result.TimeSlot);
        }
        [Fact]
        public void CancelAppointment_NotFound_ShouldReturnFalse()
        {
            _repoMock.Setup(r => r.GetById(1))
                .Returns((Appointment)null!);

            var result = _service.CancelAppointment(1, "reason");

            Assert.False(result);
        }
        [Fact]
        public void CancelAppointment_Completed_ShouldReturnFalse()
        {
            var appointment = new Appointment
            {
                Status = Appointment.StatusOption.Completed
            };

            _repoMock.Setup(r => r.GetById(1))
                .Returns(appointment);

            var result = _service.CancelAppointment(1, "reason");

            Assert.False(result);
        }
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOnlyConfirmedAndSorted()
        {
            var doctorA = CreateDoctor(1);
            var doctorB = CreateDoctor(2);
            doctorA.FullName = "A";
            doctorB.FullName = "B";

            var data = new List<Appointment>
    {
        new Appointment
        {
            ScheduledDate = DateTime.Today.AddDays(2),
            Status = Appointment.StatusOption.Confirmed,
            Doctor = doctorB
        },
        new Appointment
        {
            ScheduledDate = DateTime.Today.AddDays(1),
            Status = Appointment.StatusOption.Confirmed,
            Doctor = doctorA
        },
        new Appointment
        {
            ScheduledDate = DateTime.Today.AddDays(1),
            Status = Appointment.StatusOption.Pending,
            Doctor = doctorA
        }
    };

            _repoMock.Setup(r => r.GetAll())
                .Returns(data);

            var result = _service.GetUpcomingAppointments();

            Assert.Equal(2, result.Count);
            Assert.True(result[0].ScheduledDate <= result[1].ScheduledDate);
        }
    }
}