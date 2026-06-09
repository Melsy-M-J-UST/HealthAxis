using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class HealthRecordsController : Controller
    {
        private readonly IHealthRecordMvcService _records;

        public HealthRecordsController(IHealthRecordMvcService records)
        {
            _records = records;
        }

        public ActionResult PatientHistory(int? patientId)
        {
            if (patientId == null)
            {
                return View("PatientIdRequired");
            }

            var records = _records.GetByPatient(patientId.Value);

            return View(records);
        }

        public ActionResult Create(int patientId, int doctorId)
        {
            var dto = new HealthRecordDto
            {
                PatientId = patientId,
                DoctorId = doctorId
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            string errorMessage;

            bool result = _records.Create(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                return View(dto);
            }

            return RedirectToAction(
                "PatientHistory",
                new { patientId = dto.PatientId });
        }
    }
}