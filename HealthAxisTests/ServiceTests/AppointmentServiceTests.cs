using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Service.Implementation;
using HealthAxis.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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
        private static DateTime GetNextWeekday()
        {
            var date = DateTime.Today.AddDays(1);
            while (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);
            return date;
        }
        private static Patient CreatePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                PatientName = "Test Patient"
            };
        }

        private static Doctor CreateDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                DoctorName = "Test Doctor",
                IsPractising = true
            };
        }

        [Fact]
        public void BookAppointment_ValidSlot_ShouldCreateAppointment()
        {

            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = GetNextWeekday();

            _repoMock.Setup(r => r.GetAppointmentsByPatient(1))
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
            var date = GetNextWeekday();

            _repoMock.Setup(r => r.GetAppointmentsByPatient(1))
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
        public void GetAppointmentsByPatient_ShouldReturnOnlyPatientAppointments()
        {

            var patient = CreatePatient(1);

            var appointments = new List<Appointment>
            {
                new Appointment { Patient = patient },
                new Appointment { Patient = patient }
            };

            _repoMock.Setup(r => r.GetAppointmentsByPatient(1))
                .Returns(appointments);


            var result = _service.GetAppointmentsByPatient(1);


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(1, a.Patient.PatientId));
        }
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnOnlyFutureConfirmedAppointments()
        {
            var doctor = CreateDoctor(1);
            var appointments = new List<Appointment>
            {
                new Appointment { ScheduledDate = DateTime.Today.AddDays(1), Status = Appointment.AppointmentStatus.Confirmed, Doctor = doctor },
                new Appointment { ScheduledDate = DateTime.Today.AddDays(-1), Status = Appointment.AppointmentStatus.Confirmed, Doctor = doctor },
                new Appointment { ScheduledDate = DateTime.Today.AddDays(2), Status = Appointment.AppointmentStatus.Cancelled, Doctor = doctor },
                new Appointment { ScheduledDate = DateTime.Today.AddDays(3), Status = Appointment.AppointmentStatus.Confirmed, Doctor = doctor }
            };
            _repoMock.Setup(r => r.GetAllAppointments())
                .Returns(appointments);
            Assert.Equal(2, _service.GetUpcomingAppointments().Count);
            Assert.NotNull(appointments);
        }
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnSortedAppointments()
        {
            var doctor1 = CreateDoctor(1);
            var doctor2 = CreateDoctor(2);
            var appointments = new List<Appointment>
            {
                new Appointment { ScheduledDate = DateTime.Today.AddDays(3), Status = Appointment.AppointmentStatus.Confirmed, Doctor = doctor2 },
                new Appointment { ScheduledDate = DateTime.Today.AddDays(1), Status = Appointment.AppointmentStatus.Confirmed, Doctor = doctor1 },
                new Appointment { ScheduledDate = DateTime.Today.AddDays(2), Status = Appointment.AppointmentStatus.Confirmed, Doctor = doctor1 }
            };
            _repoMock.Setup(r => r.GetAllAppointments())
                .Returns(appointments);
            var result = _service.GetUpcomingAppointments();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(DateTime.Today.AddDays(1), result[0].ScheduledDate);
            Assert.Equal(DateTime.Today.AddDays(2), result[1].ScheduledDate);
            Assert.Equal(DateTime.Today.AddDays(3), result[2].ScheduledDate);
        }
        [Fact]
        public void BookAppointment_DoctorNotPractising_ShouldThrowDoctorUnavailableException()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            doctor.IsPractising = false;
            var date = GetNextWeekday();
            Assert.Throws<DoctorUnavailableException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );
        }
        [Theory]
        [InlineData(1)]
        public void BookAppointment_PatientHasConflict_ShouldThrowAppointmentConflictException(int existingDoctorId)
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = GetNextWeekday();
            _repoMock.Setup(r => r.GetAppointmentsByPatient(1))
                .Returns(new List<Appointment>
                {
                    new Appointment { Doctor = new Doctor { DoctorId = existingDoctorId } }
                });
            Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );
        }
        [Fact]
        public void BookAppointment_PatientHasConflictWithSameDoctor_ShouldThrowAppointmentConflictException()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = GetNextWeekday();
            _repoMock.Setup(r => r.GetAppointmentsByPatient(1))
                .Returns(new List<Appointment>
                {
                    new Appointment { Doctor = doctor }
                });
            Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );
        }
        [Fact]
        public void GetAppointment_PatientHasConflict_ShouldThrowAppointmentConflictException()
        {
            var patient = CreatePatient(1);
            var doctor = CreateDoctor(1);
            var date = GetNextWeekday();
            _repoMock.Setup(r => r.GetAppointmentsByPatient(1))
                .Returns(new List<Appointment>
                {
                    new Appointment { Doctor = doctor }
                });
            Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, date)
            );
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(99)]
        public void GetAppointmentById_returnsNull_ShouldReturnNull( int id)
        {
            var result = _service.GetAppointmentById(id);
            Assert.Null(result);
        }
    }
}