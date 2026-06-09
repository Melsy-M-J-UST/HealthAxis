using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            return Ok(_service.GetByPatient(patientId));
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public IHttpActionResult GetByDoctor(int doctorId)
        {
            return Ok(_service.GetByDoctor(doctorId));
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/today")]
        public IHttpActionResult Today(int doctorId)
        {
            return Ok(_service.GetTodaySchedule(doctorId));
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/weekly")]
        public IHttpActionResult Weekly(int doctorId, DateTime? startDate = null)
        {
            DateTime weekStartDate = startDate ?? DateTime.Today;

            return Ok(_service.GetWeeklySchedule(doctorId, weekStartDate));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Book(AppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;

            bool result = _service.Book(dto, out errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Appointment booked.");
        }

        [HttpPut]
        [Route("{id:int}/status")]
        public IHttpActionResult UpdateStatus(int id, AppointmentStatusUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;

            bool result = _service.UpdateStatus(id, dto, out errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Status updated.");
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            bool result = _service.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Appointment deleted.");
        }
    }
}