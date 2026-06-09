using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxisWebApp;
using System;
using System.Linq;
using System.Web.Http;

namespace HealthAxisWebApi.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentsController(
            IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // ==========================================
        // GET ALL APPOINTMENTS
        // ==========================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var appointments =
                _appointmentService
                .GetAllAppointments()
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,

                    PatientId = a.PatientId,

                    PatientName =
                        a.Patient != null
                        ? a.Patient.FullName
                        : string.Empty,

                    DoctorId = a.DoctorId,

                    DoctorName =
                        a.Doctor != null
                        ? a.Doctor.FullName
                        : string.Empty,

                    ScheduledDate =
                        a.ScheduledDate,

                    TimeSlot =
                        a.TimeSlot,

                    TimeSlotName =
                        GetTimeSlotName(
                            a.TimeSlot),

                    Status =
                        a.Status,

                    StatusName =
                        GetStatusName(
                            a.Status),

                    CancellationReason =
                        a.CancellationReason
                })
                .ToList();

            return Ok(appointments);
        }

        // ==========================================
        // GET APPOINTMENT BY ID
        // ==========================================
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(
            int id)
        {
            var appointment =
                _appointmentService
                .GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var dto =
                new AppointmentDto
                {
                    AppointmentId =
                        appointment.AppointmentId,

                    PatientId =
                        appointment.PatientId,

                    PatientName =
                        appointment.Patient != null
                        ? appointment.Patient.FullName
                        : string.Empty,

                    DoctorId =
                        appointment.DoctorId,

                    DoctorName =
                        appointment.Doctor != null
                        ? appointment.Doctor.FullName
                        : string.Empty,

                    ScheduledDate =
                        appointment.ScheduledDate,

                    TimeSlot =
                        appointment.TimeSlot,

                    TimeSlotName =
                        GetTimeSlotName(
                            appointment.TimeSlot),

                    Status =
                        appointment.Status,

                    StatusName =
                        GetStatusName(
                            appointment.Status),

                    CancellationReason =
                        appointment.CancellationReason
                };

            return Ok(dto);
        }

        // ==========================================
        // CREATE APPOINTMENT
        // ==========================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(
            AppointmentDto dto)
        {
            if (dto == null)
            {
                return BadRequest(
                    "Appointment data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            var appointment =
                new Appointment
                {
                    PatientId =
                        dto.PatientId,

                    DoctorId =
                        dto.DoctorId,

                    ScheduledDate =
                        dto.ScheduledDate,

                    TimeSlot =
                        dto.TimeSlot,

                    Status =
                        dto.Status,

                    CancellationReason =
                        dto.CancellationReason
                };

            try
            {
                _appointmentService
                    .AddAppointment(
                        appointment);

                return Ok(
                    "Appointment created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // ==========================================
        // UPDATE APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(
            int id,
            AppointmentDto dto)
        {
            if (dto == null)
            {
                return BadRequest(
                    "Appointment data is required.");
            }

            if (id != dto.AppointmentId)
            {
                return BadRequest(
                    "Appointment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            var existingAppointment =
                _appointmentService
                .GetAppointmentById(id);

            if (existingAppointment == null)
            {
                return NotFound();
            }

            existingAppointment.PatientId =
                dto.PatientId;

            existingAppointment.DoctorId =
                dto.DoctorId;

            existingAppointment.ScheduledDate =
                dto.ScheduledDate;

            existingAppointment.TimeSlot =
                dto.TimeSlot;

            existingAppointment.Status =
                dto.Status;

            existingAppointment.CancellationReason =
                dto.CancellationReason;

            try
            {
                _appointmentService
                    .UpdateAppointment(
                        existingAppointment);

                return Ok(
                    "Appointment updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // ==========================================
        // CANCEL APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}/cancel")]
        public IHttpActionResult Cancel(
            int id,
            CancelAppointmentRequest request)
        {
            var appointment =
                _appointmentService
                .GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (request == null ||
                string.IsNullOrWhiteSpace(
                    request.CancellationReason))
            {
                return BadRequest(
                    "Cancellation reason is required.");
            }

            try
            {
                _appointmentService
                    .CancelAppointment(
                        id,
                        request.CancellationReason);

                return Ok(
                    "Appointment cancelled successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // ==========================================
        // CONFIRM APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}/confirm")]
        public IHttpActionResult Confirm(
            int id)
        {
            var appointment =
                _appointmentService
                .GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                _appointmentService
                    .ConfirmAppointment(id);

                return Ok(
                    "Appointment confirmed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // ==========================================
        // COMPLETE APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}/complete")]
        public IHttpActionResult Complete(
            int id)
        {
            var appointment =
                _appointmentService
                .GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                _appointmentService
                    .CompleteAppointment(id);

                return Ok(
                    "Appointment completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // ==========================================
        // DELETE APPOINTMENT
        // ==========================================
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(
            int id)
        {
            var appointment =
                _appointmentService
                .GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                _appointmentService
                    .DeleteAppointment(id);

                return Ok(
                    "Appointment deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // ==========================================
        // HELPER METHODS
        // ==========================================
        private string GetStatusName(
            int status)
        {
            if (Enum.IsDefined(
                typeof(AppointmentStatus),
                status))
            {
                return ((AppointmentStatus)
                    status).ToString();
            }

            return "Unknown";
        }

        private string GetTimeSlotName(
            int slot)
        {
            if (Enum.IsDefined(
                typeof(AppointmentTimeSlot),
                slot))
            {
                return ((AppointmentTimeSlot)
                    slot).ToString();
            }

            return "Unknown";
        }
    }

    public class CancelAppointmentRequest
    {
        public string CancellationReason
        {
            get;
            set;
        }
    }
}
