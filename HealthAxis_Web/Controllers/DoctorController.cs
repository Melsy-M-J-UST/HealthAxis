using HealthAxis_MVC.Services;
using HealthAxis_Web.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthAxis_Web.Controllers
{
    public class DoctorController : ApiController
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }
        [HttpGet]
        public IHttpActionResult GetAllDoctors()
        {
            var doctors = _service.GetAllDoctors();
            return Ok(doctors);
        }
        [HttpGet]
        public IHttpActionResult GetDoctorById(int id)
        {
            var doctor = _service.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }
        [HttpPost]
        public IHttpActionResult AddDoctor(DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _service.AddDoctor(doctorDto);
            return CreatedAtRoute("DefaultApi", new { id = result.DoctorId }, result);
        }
        [HttpPut]

        public IHttpActionResult UpdateDoctor(int id, [FromBody] DoctorDto docDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _service.UpdateDoctor(id, docDto);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
