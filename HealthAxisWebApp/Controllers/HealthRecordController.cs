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

        public HealthRecordController()
        {
            _healthRecordApiClient = new HealthRecordApiClient();
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> List()
        {
            var records = await _healthRecordApiClient.GetAllHealthRecords();

            var orderedRecords = records
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            return View(orderedRecords);
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
                return RedirectToAction("List");
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

            return View(record);
        }

        public async Task<ActionResult> PatientHistory(int? patientId)
        {
            ViewBag.HasSearched = false;

            if (!patientId.HasValue)
            {
                return View(new List<HealthRecordDto>());
            }

            var records = await _healthRecordApiClient.GetAllHealthRecords();

            var result = records
                .Where(r => r.PatientId == patientId.Value)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            ViewBag.HasSearched = true;

            return View(result);
        }

        //public async Task<ActionResult> DoctorView(int? patientId)
        //{
        //    ViewBag.HasSearched = false;

        //    if (!patientId.HasValue)
        //    {
        //        return View(new List<HealthRecordDto>());
        //    }

        //    var records = await _healthRecordApiClient.GetAllHealthRecords();

        //    var result = records
        //        .Where(r => r.PatientId == patientId.Value)
        //        .OrderByDescending(r => r.VisitDate)
        //        .ToList();

        //    ViewBag.HasSearched = true;

        //    return View(result);
        //}
    }
}