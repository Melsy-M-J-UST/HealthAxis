using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Interfaces;
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

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(string sortBy = "name", string insuranceFilter = "all")
        {
            var patients = _patientService
                .GetPatients(sortBy, insuranceFilter)
                .Where(p => p.IsActive)
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
                    CreatedDate = p.CreatedDate,
                    IsActive = p.IsActive
                })
                .ToList();

            return Ok(patients);
        }

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
                CreatedDate = patient.CreatedDate,
                IsActive = patient.IsActive
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("{id:int}/profile")]
        public IHttpActionResult GetProfile(int id)
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
                GenderName = ((GenderType)patient.Gender).ToString(),
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceID = patient.InsuranceID,
                CreatedDate = patient.CreatedDate,
                AppointmentCount = _patientService.GetAppointmentCount(id)
            };

            return Ok(dto);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(PatientDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Patient data is required.");
            }

            dto.InsuranceID = string.IsNullOrWhiteSpace(dto.InsuranceID)
                ? null
                : dto.InsuranceID.Trim();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var patient = new Patient
                {
                    FullName = dto.FullName,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    InsuranceID = dto.InsuranceID,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _patientService.AddPatient(patient);

                return Ok("Patient created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, PatientDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Patient data is required.");
            }

            dto.InsuranceID = string.IsNullOrWhiteSpace(dto.InsuranceID)
                ? null
                : dto.InsuranceID.Trim();

            if (id != dto.PatientId)
            {
                return BadRequest("Patient ID mismatch.");
            }

            var existingPatient = _patientService.GetPatientById(id);

            if (existingPatient == null)
            {
                return NotFound();
            }

            try
            {
                existingPatient.FullName = dto.FullName;
                existingPatient.DateOfBirth = dto.DateOfBirth;
                existingPatient.Gender = dto.Gender;
                existingPatient.PhoneNumber = dto.PhoneNumber;
                existingPatient.Email = dto.Email;
                existingPatient.InsuranceID = dto.InsuranceID;

                _patientService.UpdatePatient(existingPatient);

                return Ok("Patient updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/deactivate")]
        public IHttpActionResult Deactivate(int id)
        {
            var patient = _patientService.GetPatientById(id);

            if (patient == null)
            {
                return NotFound();
            }

            _patientService.DeactivatePatient(id);

            return Ok("Patient deactivated successfully.");
        }
    }
}