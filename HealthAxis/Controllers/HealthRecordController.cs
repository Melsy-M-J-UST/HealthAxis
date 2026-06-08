using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    [Authorize]
    public class HealthRecordsController : Controller
    {
        private readonly IHealthRecordMvcService _healthRecordService;

        public HealthRecordsController(IHealthRecordMvcService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        public ActionResult PatientHistory(int? patientId)
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "Patient")
            {
                patientId = Convert.ToInt32(Session["ReferenceId"]);
            }

            if (patientId == null) return RedirectToAction("Index", "Home");

            var records = _healthRecordService.GetPatientHistory(patientId.Value);
            return View(records);
        }

        public ActionResult Create(int patientId, int doctorId, int? appointmentId)
        {
            var dto = new HealthRecordDto
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = appointmentId
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            bool result = _healthRecordService.Create(dto, out string errorMessage);
            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                return View(dto);
            }

            return RedirectToAction("DoctorAppointments", "Appointments");
        }
    }
}
