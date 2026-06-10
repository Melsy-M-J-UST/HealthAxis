using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxisWebApp;
using System;
using System.Linq;
using System.Web.Http;

namespace HealthAxisWebApi.Controllers
{
    [RoutePrefix("api/healthrecords")]
    public class HealthRecordsController : ApiController
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IAppointmentService _appointmentService;
        public HealthRecordsController(
            IHealthRecordService healthRecordService,
            IAppointmentService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        // GET: api/healthrecords
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var records = _healthRecordService
                .GetAllRecords()
                .Select(r => new HealthRecordDto
                {
                    RecordId = r.RecordId,
                    AppointmentId = r.AppointmentId,
                    PatientId = r.PatientId,
                    DoctorId = r.DoctorId,
                    VisitDate = r.VisitDate,
                    Diagnosis = r.Diagnosis,
                    Prescription = r.Prescription,
                    Notes = r.Notes
                })
                .ToList();

            return Ok(records);
        }

        // GET: api/healthrecords/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var record =
                _healthRecordService.GetRecordById(id);

            if (record == null)
            {
                return NotFound();
            }

            var dto = new HealthRecordDto
            {
                RecordId = record.RecordId,
                AppointmentId = record.AppointmentId,
                PatientId = record.PatientId,
                DoctorId = record.DoctorId,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };

            return Ok(dto);
        }

        // POST: api/healthrecords
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(
            HealthRecordDto dto)
        {
            if (dto == null)
            {
                return BadRequest(
                    "Health record data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment =
                _appointmentService
                .GetAppointmentById(
                    dto.AppointmentId);

            if (appointment == null)
            {
                return BadRequest(
                    "Invalid Appointment ID.");
            }

            var record = new HealthRecord
            {
                AppointmentId =
                    appointment.AppointmentId,

                PatientId =
                    appointment.PatientId,

                DoctorId =
                    appointment.DoctorId,

                VisitDate =
                    appointment.ScheduledDate,

                Diagnosis =
                    dto.Diagnosis,

                Prescription =
                    dto.Prescription,

                Notes =
                    dto.Notes
            };

            try
            {
                _healthRecordService
                    .AddRecord(record);

                return Ok(
                    "Health record created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // PUT: api/healthrecords/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(
            int id,
            HealthRecordDto dto)
        {
            if (dto == null)
            {
                return BadRequest(
                    "Health record data is required.");
            }

            if (id != dto.RecordId)
            {
                return BadRequest(
                    "Record ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            var existingRecord =
                _healthRecordService
                .GetRecordById(id);

            if (existingRecord == null)
            {
                return NotFound();
            }

            existingRecord.Diagnosis =
                dto.Diagnosis;

            existingRecord.Prescription =
                dto.Prescription;

            existingRecord.Notes =
                dto.Notes;

            try
            {
                _healthRecordService
                    .UpdateRecord(
                        existingRecord);

                return Ok(
                    "Health record updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        // DELETE: api/healthrecords/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(
            int id)
        {
            var existingRecord =
                _healthRecordService
                .GetRecordById(id);

            if (existingRecord == null)
            {
                return NotFound();
            }

            try
            {
                _healthRecordService
                    .DeleteRecord(id);

                return Ok(
                    "Health record deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}
