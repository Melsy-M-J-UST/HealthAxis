using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/health-records")]
    public class HealthRecordsController : ApiController
    {
        private readonly IHealthRecordService _service;

        public HealthRecordsController(IHealthRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var records = _service.GetByPatient(patientId);

            return Ok(records);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;

            bool result = _service.Create(dto, out errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Health record created.");
        }
    }
}