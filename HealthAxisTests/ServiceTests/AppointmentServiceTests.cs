using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Services;
using HealthAxis.Exceptions;

namespace HealthAxisTests.ServiceTests
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

        [Fact]
        public void BookAppointment_ShouldThrowException_WhenPatientIsNull()
        {
            Patient patient = null!;
            Doctor doctor = new();
            DateTime date = DateTime.Now;

            var ex = Assert.Throws<ArgumentException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );

            Assert.Equal("Patient is required.", ex.Message);
        }

        [Fact]
        public void BookAppointment_ShouldThrowException_WhenDoctorIsNull()
        {
            Patient patient = new Patient();
            Doctor doctor = null!;
            DateTime date = DateTime.Now;

            var ex = Assert.Throws<ArgumentException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );

            Assert.Equal("Doctor is required.", ex.Message);
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

            _repoMock.Setup(r => r.AddAppointment(It.IsAny<Appointment>()))
                .Returns((Appointment a) => a);


            var result = _service.BookAppointment(patient, doctor, date);


            Assert.NotNull(result);
            Assert.Equal("09:00 AM", result.Slot);
            Assert.Equal(Appointment.AppointmentStatus.Confirmed, result.Status);

            _repoMock.Verify(r => r.AddAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnAppointments()
        {
            int doctorId = 1;

            var expectedList = new List<Appointment>
            {
            new Appointment
            {
            Doctor = new Doctor { DoctorId = doctorId }
            }
            };

            _repoMock.Setup(r => r.GetByDoctorId(doctorId))
                     .Returns(expectedList);

            var result = _service.GetAppointmentsByDoctor(doctorId);

            Assert.NotNull(result);
            Assert.Equal(expectedList, result);
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
                Status = Appointment.AppointmentStatus.Confirmed
            };

            _repoMock.Setup(r => r.GetAppointmentById(1))
                .Returns(appointment);


            var result = _service.CancelAppointment(1, "Not needed");


            Assert.True(result);
            Assert.Equal(Appointment.AppointmentStatus.Cancelled, appointment.Status);

            _repoMock.Verify(r => r.Remove(appointment), Times.Once);
        }

        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOnlyFutureConfirmedAppointments_Sorted()
        {
            var today = DateTime.Today;

            var appointments = new List<Appointment>
    {
        new Appointment
        {
            ScheduledDate = today.AddDays(1),
            Status = Appointment.AppointmentStatus.Confirmed,
            Doctor = new Doctor { FullName = "B Doctor" }
        },
        new Appointment
        {
            ScheduledDate = today.AddDays(2),
            Status = Appointment.AppointmentStatus.Confirmed,
            Doctor = new Doctor { FullName = "A Doctor" }
        },
        new Appointment
        {
            ScheduledDate = today.AddDays(-1),
            Status = Appointment.AppointmentStatus.Confirmed,
            Doctor = new Doctor { FullName = "C Doctor" }
        },
        new Appointment
        {
            ScheduledDate = today.AddDays(1),
            Status = Appointment.AppointmentStatus.Cancelled
        }
    };

            _repoMock.Setup(r => r.GetAllAppointments())
                     .Returns(appointments);

            var result = _service.GetUpcomingAppointments();

            Assert.Equal(2, result.Count);

            Assert.Equal("B Doctor", result[0].Doctor.FullName);
            Assert.Equal("A Doctor", result[1].Doctor.FullName);
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

        private static DateTime GetNextWeekday()

        {

            var date = DateTime.Today.AddDays(1);

            while (date.DayOfWeek == DayOfWeek.Sunday)

                date = date.AddDays(1);

            return date;

        }
        private static DateTime GetNextSunday()

        {

            var date = GetNextWeekday();

            while (date.DayOfWeek != DayOfWeek.Sunday)

                date = date.AddDays(1);

            return date;

        }

        [Fact]
        public void BookAppointment_ShouldThrowConflictException_WhenSameDoctorAlreadyExists()
        {
            var patient = new Patient { PatientId = 1 };
            var doctor = new Doctor { DoctorId = 1, IsActive = true };
            var date = DateTime.Today.AddDays(1);

            var existingAppointments = new List<Appointment>
    {
        new Appointment
        {
            Patient = patient,
            Doctor = doctor,
            ScheduledDate = date
        }
    };

            _repoMock.Setup(r => r.GetByPatientId(patient.PatientId))
                     .Returns(existingAppointments);

            var ex = Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );

            Assert.Equal(
                "Patient already has an appointment with this doctor on the selected date.",
                ex.Message
            );
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
        public void CancelAppointment_InvalidId_ShouldReturnFalse()
        {
            _repoMock.Setup(r => r.GetAppointmentById(99))
                     .Returns((Appointment)null!);

            var result = _service.CancelAppointment(99, "Reason");

            Assert.False(result);
        }
        [Fact]
        public void GetById_ShouldReturnAppointment()
        {
            var appointment = new Appointment { AppointmentId = 1 };

            _repoMock.Setup(r => r.GetAppointmentById(1))
                     .Returns(appointment);

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public void GetAll_ShouldReturnAllAppointments()
        {
            var list = new List<Appointment>
        {
        new Appointment(),
        new Appointment()
        };

            _repoMock.Setup(r => r.GetAllAppointments())
                     .Returns(list);

            var result = _service.GetAll();

            Assert.Equal(2, result.Count);
        }

    }
}