using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
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
        public IHttpActionResult GetAll(string specialisation = null, bool activeOnly = false)
        {
            var doctors = _doctorService.GetAll(specialisation, activeOnly);
            return Ok(doctors);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var doctor = _doctorService.GetById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdDoctor = _doctorService.Create(dto);
            return Ok(createdDoctor);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = _doctorService.Update(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut]
        [Route("{id:int}/toggle-status")]
        public IHttpActionResult ToggleStatus(int id)
        {
            var toggled = _doctorService.ToggleStatus(id);

            if (!toggled)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}