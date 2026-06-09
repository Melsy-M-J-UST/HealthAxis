using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxisWebApp;
using System;
using System.Linq;
using System.Web.Http;

namespace HealthAxisWebApi.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patients
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var patients = _patientService.GetAllPatients()
                .Select(p => new PatientDto
                {
                    PatientId = p.PatientId,
                    FullName = p.FullName,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    GenderName = ((GenderType)p.Gender).ToString(),
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    InsuranceID = p.InsuranceID,
                    CreatedDate = p.CreatedDate
                })
                .ToList();

            return Ok(patients);
        }

        // GET: api/patients/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var patient = _patientService.GetPatientById(id);

            if (patient == null)
            {
                return NotFound();
            }

            var dto = new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                GenderName = ((GenderType)patient.Gender).ToString(),
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceID = patient.InsuranceID,
                CreatedDate = patient.CreatedDate
            };

            return Ok(dto);
        }

        // POST: api/patients
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(PatientDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Patient data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = new Patient
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceID = dto.InsuranceID,
                CreatedDate = DateTime.Now
            };

            _patientService.AddPatient(patient);

            return Ok("Patient created successfully.");
        }

        // PUT: api/patients/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, PatientDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Patient data is required.");
            }

            if (id != dto.PatientId)
            {
                return BadRequest("Patient ID mismatch.");
            }

            var existingPatient = _patientService.GetPatientById(id);

            if (existingPatient == null)
            {
                return NotFound();
            }

            existingPatient.FullName = dto.FullName;
            existingPatient.DateOfBirth = dto.DateOfBirth;
            existingPatient.Gender = dto.Gender;
            existingPatient.PhoneNumber = dto.PhoneNumber;
            existingPatient.Email = dto.Email;
            existingPatient.InsuranceID = dto.InsuranceID;

            _patientService.UpdatePatient(existingPatient);

            return Ok("Patient updated successfully.");
        }

        // DELETE: api/patients/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var existingPatient = _patientService.GetPatientById(id);

            if (existingPatient == null)
            {
                return NotFound();
            }

            _patientService.DeletePatient(id);

            return Ok("Patient deleted successfully.");
        }
    }
}