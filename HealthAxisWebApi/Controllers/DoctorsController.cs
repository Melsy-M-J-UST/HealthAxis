using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;

namespace HealthAxisWebApi.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorsController : ApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(
            string sortBy = "name",
            string specialisation = "all")
        {
            var doctors = _doctorService
                .GetAllDoctors()
                .Where(d => d.IsActive);

            if (!string.IsNullOrEmpty(specialisation) && specialisation != "all")
            {
                if (int.TryParse(specialisation, out int specValue))
                {
                    doctors = doctors.Where(d => d.Specialisation == specValue);
                }
            }

            doctors = sortBy == "name_desc"
                ? doctors.OrderByDescending(d => d.FullName)
                : doctors.OrderBy(d => d.FullName);

            var result = doctors
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Specialisation = d.Specialisation,
                    SpecialisationName = GetSpecialisationName(d.Specialisation),
                    YearsOfExperience = d.YearsOfExperience,
                    ConsultationFee = d.ConsultationFee,
                    IsActive = d.IsActive
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var dto = new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                SpecialisationName = GetSpecialisationName(doctor.Specialisation),
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("{id:int}/profile")]
        public IHttpActionResult GetProfile(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var dto = new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                SpecialisationName = GetSpecialisationName(doctor.Specialisation),
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive,
                UpcomingAppointmentCount = _doctorService.GetUpcomingAppointmentCount(id)
            };

            return Ok(dto);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(DoctorDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Doctor data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(DoctorSpecialisation), dto.Specialisation))
            {
                return BadRequest("Invalid Specialisation.");
            }

            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            try
            {
                _doctorService.AddDoctor(doctor);
                dto.DoctorId = doctor.DoctorId;
                return Created($"api/doctors/{doctor.DoctorId}", dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, DoctorDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Doctor data is required.");
            }

            if (id != dto.DoctorId)
            {
                return BadRequest("Doctor ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(DoctorSpecialisation), dto.Specialisation))
            {
                return BadRequest("Invalid Specialisation.");
            }

            var existingDoctor = _doctorService.GetDoctorById(id);

            if (existingDoctor == null)
            {
                return NotFound();
            }

            existingDoctor.FullName = dto.FullName;
            existingDoctor.Specialisation = dto.Specialisation;
            existingDoctor.YearsOfExperience = dto.YearsOfExperience;
            existingDoctor.ConsultationFee = dto.ConsultationFee;

            try
            {
                _doctorService.UpdateDoctor(existingDoctor);
                return Ok("Doctor updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/toggle")]
        public IHttpActionResult ToggleStatus(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            doctor.IsActive = !doctor.IsActive;

            _doctorService.UpdateDoctor(doctor);

            return Ok("Doctor status updated successfully.");
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            try
            {
                _doctorService.DeleteDoctor(id);
                return Ok("Doctor deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetSpecialisationName(int specialisation)
        {
            if (Enum.IsDefined(typeof(DoctorSpecialisation), specialisation))
            {
                return ((DoctorSpecialisation)specialisation).ToString();
            }

            return "Unknown";
        }
    }
}