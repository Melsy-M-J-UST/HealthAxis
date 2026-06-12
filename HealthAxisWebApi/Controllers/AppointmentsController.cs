using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

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

                    ScheduledDate = a.ScheduledDate,

                    TimeSlot = a.TimeSlot,

                    TimeSlotName =
                        GetTimeSlotName(a.TimeSlot),

                    Status = a.Status,

                    StatusName =
                        GetStatusName(a.Status),

                    CancellationReason = a.CancellationReason
                })
                .ToList();

            return Ok(appointments);
        }

        // ==========================================
        // GET APPOINTMENT BY ID
        // ==========================================
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
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
                    AppointmentId = appointment.AppointmentId,

                    PatientId = appointment.PatientId,

                    PatientName =
                        appointment.Patient != null
                        ? appointment.Patient.FullName
                        : string.Empty,

                    DoctorId = appointment.DoctorId,

                    DoctorName =
                        appointment.Doctor != null
                        ? appointment.Doctor.FullName
                        : string.Empty,

                    ScheduledDate = appointment.ScheduledDate,

                    TimeSlot = appointment.TimeSlot,

                    TimeSlotName =
                        GetTimeSlotName(appointment.TimeSlot),

                    Status = appointment.Status,

                    StatusName =
                        GetStatusName(appointment.Status),

                    CancellationReason = appointment.CancellationReason
                };

            return Ok(dto);
        }

        // ==========================================
        // CREATE APPOINTMENT
        // ==========================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(AppointmentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Appointment data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment =
                new Appointment
                {
                    PatientId = dto.PatientId,
                    DoctorId = dto.DoctorId,
                    ScheduledDate = dto.ScheduledDate,
                    TimeSlot = dto.TimeSlot,
                    Status = dto.Status,
                    CancellationReason = dto.CancellationReason
                };

            try
            {
                _appointmentService.AddAppointment(appointment);

                return Ok("Appointment created successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error: " + GetInnermostMessage(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // UPDATE APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, AppointmentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Appointment data is required.");
            }

            if (id != dto.AppointmentId)
            {
                return BadRequest("Appointment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAppointment =
                _appointmentService
                .GetAppointmentById(id);

            if (existingAppointment == null)
            {
                return NotFound();
            }

            existingAppointment.PatientId = dto.PatientId;
            existingAppointment.DoctorId = dto.DoctorId;
            existingAppointment.ScheduledDate = dto.ScheduledDate;
            existingAppointment.TimeSlot = dto.TimeSlot;
            existingAppointment.Status = dto.Status;
            existingAppointment.CancellationReason = dto.CancellationReason;

            try
            {
                _appointmentService.UpdateAppointment(existingAppointment);

                return Ok("Appointment updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error: " + GetInnermostMessage(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // CANCEL APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}/cancel")]
        public IHttpActionResult Cancel(int id, CancelAppointmentRequest request)
        {
            var appointment =
                _appointmentService
                .GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (request == null ||
                string.IsNullOrWhiteSpace(request.CancellationReason))
            {
                return BadRequest("Cancellation reason is required.");
            }

            try
            {
                _appointmentService.CancelAppointment(
                    id,
                    request.CancellationReason);

                return Ok("Appointment cancelled successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error: " + GetInnermostMessage(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // CONFIRM APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}/confirm")]
        public IHttpActionResult Confirm(int id)
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
                _appointmentService.ConfirmAppointment(id);

                return Ok("Appointment confirmed successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error: " + GetInnermostMessage(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // COMPLETE APPOINTMENT
        // ==========================================
        [HttpPut]
        [Route("{id:int}/complete")]
        public IHttpActionResult Complete(int id)
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
                _appointmentService.CompleteAppointment(id);

                return Ok("Appointment completed successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error: " + GetInnermostMessage(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // GET APPOINTMENTS BY PATIENT
        // ==========================================
        [HttpGet]
        [Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var appointments =
                _appointmentService
                .GetAllAppointments()
                .Where(a => a.PatientId == patientId)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient != null ? a.Patient.FullName : string.Empty,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    TimeSlotName = GetTimeSlotName(a.TimeSlot),
                    Status = a.Status,
                    StatusName = GetStatusName(a.Status),
                    CancellationReason = a.CancellationReason
                })
                .ToList();

            return Ok(appointments);
        }

        // ==========================================
        // GET TODAY APPOINTMENTS FOR DOCTOR
        // ==========================================
        [HttpGet]
        [Route("doctor/{doctorId:int}/today")]
        public IHttpActionResult GetTodayAppointments(int doctorId)
        {
            DateTime today = DateTime.Today;

            var appointments =
                _appointmentService
                .GetAllAppointments()
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == today)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient != null ? a.Patient.FullName : string.Empty,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    TimeSlotName = GetTimeSlotName(a.TimeSlot),
                    Status = a.Status,
                    StatusName = GetStatusName(a.Status),
                    CancellationReason = a.CancellationReason
                })
                .ToList();

            return Ok(appointments);
        }

        // ==========================================
        // GET WEEKLY APPOINTMENTS FOR DOCTOR
        // ==========================================
        [HttpGet]
        [Route("doctor/{doctorId:int}/week")]
        public IHttpActionResult GetWeeklyAppointments(int doctorId)
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(7);

            var appointments =
                _appointmentService
                .GetAllAppointments()
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date >= start &&
                    a.ScheduledDate.Date <= end)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient != null ? a.Patient.FullName : string.Empty,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    TimeSlotName = GetTimeSlotName(a.TimeSlot),
                    Status = a.Status,
                    StatusName = GetStatusName(a.Status),
                    CancellationReason = a.CancellationReason
                })
                .ToList();

            return Ok(appointments);
        }

        // ==========================================
        // DELETE APPOINTMENT
        // ==========================================
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
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
                _appointmentService.DeleteAppointment(id);

                return Ok("Appointment deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database error: " + GetInnermostMessage(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // HELPER METHODS
        // ==========================================
        private string GetStatusName(int status)
        {
            if (Enum.IsDefined(typeof(AppointmentStatus), status))
            {
                return ((AppointmentStatus)status).ToString();
            }

            return "Unknown";
        }

        private string GetTimeSlotName(int slot)
        {
            switch (slot)
            {
                case 1: return "10:00 a.m. - 10:30 a.m.";
                case 2: return "10:30 a.m. - 11:00 a.m.";
                case 3: return "11:00 a.m. - 11:30 a.m.";
                case 4: return "11:30 a.m. - 12:00 p.m.";
                case 5: return "12:00 p.m. - 12:30 p.m.";
                case 6: return "12:30 p.m. - 01:00 p.m.";
                case 7: return "01:00 p.m. - 01:30 p.m.";
                case 8: return "01:30 p.m. - 02:00 p.m.";
                case 9: return "02:00 p.m. - 02:30 p.m.";
                case 10: return "02:30 p.m. - 03:00 p.m.";
                case 11: return "03:00 p.m. - 03:30 p.m.";
                case 12: return "03:30 p.m. - 04:00 p.m.";
                default: return "Unknown Slot";
            }
        }

        private string GetInnermostMessage(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }

    public class CancelAppointmentRequest
    {
        public string CancellationReason { get; set; }
    }
}
