using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(string insuranceStatus = null)
        {
            var patients = _patientService.GetAll(insuranceStatus);

            return Ok(patients);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var patient = _patientService.GetById(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = _patientService.Create(dto, out string errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Patient created successfully.");
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = _patientService.Update(id, dto, out string errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Patient updated successfully.");
        }

        [HttpPut]
        [Route("{id:int}/deactivate")]
        public IHttpActionResult Deactivate(int id)
        {
            bool result = _patientService.Deactivate(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Patient deactivated successfully.");
        }
    }
}