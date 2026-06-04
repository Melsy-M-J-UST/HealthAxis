using HealthCareWebApp.Models;
using HealthCareWebApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCareWebApp.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        private readonly IPatientService _service;

        public PatientController()
        {

        }
        public PatientController(IPatientService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            var Patients = _service.GetAllPatients();
            return View(Patients);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Patient Patient)
        {
            _service.RegisterPatient(Patient);
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var Patient = _service.GetPatientById(id);
            return View(Patient);
        }
        [HttpPost]
        public ActionResult Update(Patient patient)
        {
            _service.UpdatePatient(patient);
            return RedirectToAction("Index");
        }
    }
}