using System;
using System.Collections.Generic;
using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Impl;
using HealthAxisWebApp;
using HealthAxisWebApp.Repositories.Interfaces;
using Moq;
using Xunit;

namespace HealthAxis.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> appointmentRepositoryMock;
        private readonly Mock<IDoctorRepository> doctorRepositoryMock;
        private readonly AppointmentService appointmentService;

        public AppointmentServiceTests()
        {
            appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            doctorRepositoryMock = new Mock<IDoctorRepository>();

            appointmentService = new AppointmentService(
                appointmentRepositoryMock.Object,
                doctorRepositoryMock.Object);
        }

        [Fact]
        public void GetAllAppointments_ReturnsAppointments()
        {
            var appointments = new List<Appointment>
            {
                CreateValidAppointment(1)
            };

            appointmentRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(appointments);

            var result = appointmentService.GetAllAppointments();

            Assert.Single(result);
            Assert.Equal(1, result[0].AppointmentId);
        }

        [Fact]
        public void GetAppointmentById_WhenExists_ReturnsAppointment()
        {
            var appointment = CreateValidAppointment(1);

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            var result = appointmentService.GetAppointmentById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public void GetAppointmentById_WhenNotExists_ReturnsNull()
        {
            appointmentRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Appointment)null);

            var result = appointmentService.GetAppointmentById(99);

            Assert.Null(result);
        }

        [Fact]
        public void AddAppointment_WhenAppointmentIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                appointmentService.AddAppointment(null));
        }

        [Fact]
        public void AddAppointment_WhenPatientIdInvalid_ThrowsArgumentException()
        {
            var appointment = CreateValidAppointment();
            appointment.PatientId = 0;

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            Assert.Throws<ArgumentException>(() =>
                appointmentService.AddAppointment(appointment));
        }

        [Fact]
        public void AddAppointment_WhenDoctorIdInvalid_ThrowsArgumentException()
        {
            var appointment = CreateValidAppointment();
            appointment.DoctorId = 0;

            SetupNoExistingAppointments();

            Assert.Throws<ArgumentException>(() =>
                appointmentService.AddAppointment(appointment));
        }

        [Fact]
        public void AddAppointment_WhenScheduledDateIsPast_ThrowsArgumentException()
        {
            var appointment = CreateValidAppointment();
            appointment.ScheduledDate = DateTime.Today.AddDays(-1);

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            Assert.Throws<ArgumentException>(() =>
                appointmentService.AddAppointment(appointment));
        }

        [Fact]
        public void AddAppointment_WhenDoctorNotFound_ThrowsKeyNotFoundException()
        {
            var appointment = CreateValidAppointment();

            doctorRepositoryMock
                .Setup(r => r.GetById(appointment.DoctorId))
                .Returns((Doctor)null);

            SetupNoExistingAppointments();

            Assert.Throws<KeyNotFoundException>(() =>
                appointmentService.AddAppointment(appointment));
        }

        [Fact]
        public void AddAppointment_WhenDoctorInactive_ThrowsInvalidOperationException()
        {
            var appointment = CreateValidAppointment();

            doctorRepositoryMock
                .Setup(r => r.GetById(appointment.DoctorId))
                .Returns(new Doctor
                {
                    DoctorId = appointment.DoctorId,
                    FullName = "Amit Verma",
                    Specialisation = 1,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = false
                });

            SetupNoExistingAppointments();

            Assert.Throws<InvalidOperationException>(() =>
                appointmentService.AddAppointment(appointment));
        }

        [Fact]
        public void AddAppointment_WhenDoctorAlreadyBooked_ThrowsInvalidOperationException()
        {
            var appointment = CreateValidAppointment();

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 99,
                    PatientId = 7,
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot,
                    Status = 0
                }
            };

            SetupActiveDoctor();

            appointmentRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(existingAppointments);

            Assert.Throws<InvalidOperationException>(() =>
                appointmentService.AddAppointment(appointment));
        }

        [Fact]
        public void AddAppointment_WhenExistingAppointmentIsCancelled_AllowsBooking()
        {
            var appointment = CreateValidAppointment();

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 99,
                    PatientId = 7,
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot,
                    Status = 2
                }
            };

            SetupActiveDoctor();

            appointmentRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(existingAppointments);

            appointmentService.AddAppointment(appointment);

            appointmentRepositoryMock.Verify(
                r => r.Add(appointment),
                Times.Once);
        }

        [Fact]
        public void AddAppointment_WhenValid_SetsStatusPending()
        {
            var appointment = CreateValidAppointment();
            appointment.Status = 3;

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            appointmentService.AddAppointment(appointment);

            Assert.Equal(0, appointment.Status);
        }

        [Fact]
        public void AddAppointment_WhenCancellationReasonNull_SetsEmptyString()
        {
            var appointment = CreateValidAppointment();
            appointment.CancellationReason = null;

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            appointmentService.AddAppointment(appointment);

            Assert.Equal(string.Empty, appointment.CancellationReason);
        }

        [Fact]
        public void AddAppointment_WhenValid_CallsRepositoryAdd()
        {
            var appointment = CreateValidAppointment();

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            appointmentService.AddAppointment(appointment);

            appointmentRepositoryMock.Verify(
                r => r.Add(appointment),
                Times.Once);
        }

        [Fact]
        public void UpdateAppointment_WhenAppointmentIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                appointmentService.UpdateAppointment(null));
        }

        [Fact]
        public void UpdateAppointment_WhenInvalid_ThrowsArgumentException()
        {
            var appointment = CreateValidAppointment();
            appointment.PatientId = 0;

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            Assert.Throws<ArgumentException>(() =>
                appointmentService.UpdateAppointment(appointment));
        }

        [Fact]
        public void UpdateAppointment_WhenSameAppointmentId_DoesNotTreatAsDoubleBooking()
        {
            var appointment = CreateValidAppointment(10);

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 10,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot,
                    Status = 0
                }
            };

            SetupActiveDoctor();

            appointmentRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(existingAppointments);

            appointmentService.UpdateAppointment(appointment);

            appointmentRepositoryMock.Verify(
                r => r.Update(appointment),
                Times.Once);
        }

        [Fact]
        public void UpdateAppointment_WhenValid_CallsRepositoryUpdate()
        {
            var appointment = CreateValidAppointment(1);

            SetupActiveDoctor();
            SetupNoExistingAppointments();

            appointmentService.UpdateAppointment(appointment);

            appointmentRepositoryMock.Verify(
                r => r.Update(appointment),
                Times.Once);
        }

        [Fact]
        public void ConfirmAppointment_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            appointmentRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Appointment)null);

            Assert.Throws<KeyNotFoundException>(() =>
                appointmentService.ConfirmAppointment(99));
        }

        [Fact]
        public void ConfirmAppointment_WhenStatusNotPending_ThrowsInvalidOperationException()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 1;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            Assert.Throws<InvalidOperationException>(() =>
                appointmentService.ConfirmAppointment(1));
        }

        [Fact]
        public void ConfirmAppointment_WhenPending_SetsStatusConfirmed()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.ConfirmAppointment(1);

            Assert.Equal(1, appointment.Status);
        }

        [Fact]
        public void ConfirmAppointment_WhenPending_CallsRepositoryUpdate()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.ConfirmAppointment(1);

            appointmentRepositoryMock.Verify(
                r => r.Update(appointment),
                Times.Once);
        }

        [Fact]
        public void CompleteAppointment_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            appointmentRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Appointment)null);

            Assert.Throws<KeyNotFoundException>(() =>
                appointmentService.CompleteAppointment(99));
        }

        [Fact]
        public void CompleteAppointment_WhenStatusNotConfirmed_ThrowsInvalidOperationException()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            Assert.Throws<InvalidOperationException>(() =>
                appointmentService.CompleteAppointment(1));
        }

        [Fact]
        public void CompleteAppointment_WhenConfirmed_SetsStatusCompleted()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 1;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.CompleteAppointment(1);

            Assert.Equal(3, appointment.Status);
        }

        [Fact]
        public void CompleteAppointment_WhenConfirmed_CallsRepositoryUpdate()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 1;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.CompleteAppointment(1);

            appointmentRepositoryMock.Verify(
                r => r.Update(appointment),
                Times.Once);
        }

        [Fact]
        public void CancelAppointment_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            appointmentRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Appointment)null);

            Assert.Throws<KeyNotFoundException>(() =>
                appointmentService.CancelAppointment(99, "Reason"));
        }

        [Fact]
        public void CancelAppointment_WhenReasonEmpty_ThrowsArgumentException()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            Assert.Throws<ArgumentException>(() =>
                appointmentService.CancelAppointment(1, string.Empty));
        }

        [Fact]
        public void CancelAppointment_WhenCompleted_ThrowsInvalidOperationException()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 3;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            Assert.Throws<InvalidOperationException>(() =>
                appointmentService.CancelAppointment(1, "Reason"));
        }

        [Fact]
        public void CancelAppointment_WhenAlreadyCancelled_ThrowsInvalidOperationException()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 2;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            Assert.Throws<InvalidOperationException>(() =>
                appointmentService.CancelAppointment(1, "Reason"));
        }

        [Fact]
        public void CancelAppointment_WhenValid_SetsStatusCancelled()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.CancelAppointment(1, "Patient request");

            Assert.Equal(2, appointment.Status);
        }

        [Fact]
        public void CancelAppointment_WhenValid_SetsCancellationReason()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.CancelAppointment(1, "Patient request");

            Assert.Equal("Patient request", appointment.CancellationReason);
        }

        [Fact]
        public void CancelAppointment_WhenValid_CallsRepositoryUpdate()
        {
            var appointment = CreateValidAppointment(1);
            appointment.Status = 0;

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.CancelAppointment(1, "Patient request");

            appointmentRepositoryMock.Verify(
                r => r.Update(appointment),
                Times.Once);
        }

        [Fact]
        public void DeleteAppointment_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            appointmentRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Appointment)null);

            Assert.Throws<KeyNotFoundException>(() =>
                appointmentService.DeleteAppointment(99));
        }

        [Fact]
        public void DeleteAppointment_WhenAppointmentExists_CallsRepositoryDelete()
        {
            var appointment = CreateValidAppointment(1);

            appointmentRepositoryMock
                .Setup(r => r.GetById(1))
                .Returns(appointment);

            appointmentService.DeleteAppointment(1);

            appointmentRepositoryMock.Verify(
                r => r.Delete(1),
                Times.Once);
        }

        private static Appointment CreateValidAppointment(int id = 0)
        {
            return new Appointment
            {
                AppointmentId = id,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = 1,
                Status = 0,
                CancellationReason = string.Empty
            };
        }

        private void SetupActiveDoctor()
        {
            doctorRepositoryMock
                .Setup(r => r.GetById(2))
                .Returns(new Doctor
                {
                    DoctorId = 2,
                    FullName = "Amit Verma",
                    Specialisation = 1,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                });
        }

        private void SetupNoExistingAppointments()
        {
            appointmentRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(new List<Appointment>());
        }
    }
}