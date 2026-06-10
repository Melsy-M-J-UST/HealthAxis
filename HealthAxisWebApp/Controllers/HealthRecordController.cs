using HealthAxis.Shared.DTOs;
using HealthAxisWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxisWebApp.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly HealthRecordApiClient _healthRecordApiClient;
        private readonly DoctorApiClient _doctorApiClient;
        private readonly PatientApiClient _patientApiClient;

        public HealthRecordController()
        {
            _healthRecordApiClient = new HealthRecordApiClient();
            _doctorApiClient = new DoctorApiClient();
            _patientApiClient = new PatientApiClient();
        }

        public async Task<ActionResult> Index(int? patientId)
        {
            return await PatientHistory(patientId);
        }

        public ActionResult List()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Create(int? appointmentId, int? patientId, int? doctorId)
        {
            if (!appointmentId.HasValue || !patientId.HasValue || !doctorId.HasValue)
            {
                return RedirectToAction("Index");
            }

            var model = new HealthRecordDto
            {
                AppointmentId = appointmentId.Value,
                PatientId = patientId.Value,
                DoctorId = doctorId.Value,
                VisitDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HealthRecordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _healthRecordApiClient.CreateHealthRecord(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            var record = await _healthRecordApiClient.GetHealthRecordById(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            await LoadRecordNames(record);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DetailsModal", record);
            }

            return View(record);
        }

        public async Task<ActionResult> PatientHistory(int? patientId)
        {
            ViewBag.HasSearched = false;

            if (!patientId.HasValue)
            {
                return View("Index", new List<HealthRecordDto>());
            }

            var records = await _healthRecordApiClient.GetAllHealthRecords();

            var result = records
                .Where(r => r.PatientId == patientId.Value)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            await LoadDoctorNames(result);

            ViewBag.HasSearched = true;
            ViewBag.PatientId = patientId.Value;

            return View("Index", result);
        }

        public ActionResult DoctorView(int? patientId)
        {
            return RedirectToAction("Index");
        }

        private async Task LoadDoctorNames(IEnumerable<HealthRecordDto> records)
        {
            var doctorIds = records
                .Select(r => r.DoctorId)
                .Distinct()
                .ToList();

            var doctorNames = new Dictionary<int, string>();

            foreach (var doctorId in doctorIds)
            {
                var doctor = await _doctorApiClient.GetDoctorById(doctorId);
                doctorNames[doctorId] = doctor != null ? doctor.FullName : $"Dr. #{doctorId}";
            }

            ViewBag.DoctorNames = doctorNames;
        }

        private async Task LoadRecordNames(HealthRecordDto record)
        {
            var patient = await _patientApiClient.GetPatientById(record.PatientId);
            var doctor = await _doctorApiClient.GetDoctorById(record.DoctorId);

            ViewBag.PatientName = patient != null ? patient.FullName : $"Patient #{record.PatientId}";
            ViewBag.DoctorName = doctor != null ? doctor.FullName : $"Dr. #{record.DoctorId}";
        }
    }
}