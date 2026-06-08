using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_appointmentService.GetAll());
        }
        [HttpGet, Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            return Ok(_appointmentService.GetByPatient(patientId));
        }
        [HttpGet, Route("doctor/{doctorId:int}")]
        public IHttpActionResult GetByDoctor(int doctorId)
        {
            return Ok(_appointmentService.GetByDoctor(doctorId));
        }
        [HttpPost, Route("")]
        public IHttpActionResult Book(AppointmentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool result = _appointmentService.Book(dto, out string errorMessage);
            if (!result) return BadRequest(errorMessage);
            return Ok("Appointment booked successfully.");
        }
        [HttpPut, Route("{id:int}/status")]
        public IHttpActionResult UpdateStatus(int id, AppointmentDto dto)
        {
            dto.AppointmentId = id;
            bool result = _appointmentService.UpdateStatus(dto, out string errorMessage);
            if (!result) return BadRequest(errorMessage);
            return Ok("Appointment status updated successfully.");
        }
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            bool result = _appointmentService.Delete(id);
            if (!result) return NotFound();
            return Ok("Appointment deleted successfully.");
        }
    }
}
