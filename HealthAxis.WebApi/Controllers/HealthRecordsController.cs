using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/health-records")]
    public class HealthRecordsController : ApiController
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordsController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var healthRecords = _healthRecordService.GetByPatient(patientId);
            return Ok(healthRecords);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdHealthRecord = _healthRecordService.Create(dto);
            return Ok(createdHealthRecord);
        }
    }
}