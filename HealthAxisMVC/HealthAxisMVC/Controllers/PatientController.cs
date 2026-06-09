using HealthAxisMVC.Models;
using HealthAxisMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAxisMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _service;
        public PatientController(IPatientService service)
        {
            _service = service;
        }
        // GET: Patient
        public ActionResult Index()
        {
            var patientList = _service.GetAllPatients();
            return View(patientList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.CreatedDate = DateTime.Now;

                _service.RegisterPatient(patient);

                return RedirectToAction("Index");
            }

            return View(patient);
        }


        public ActionResult Details(int id)
        {
            var patient = _service.GetPatientById(id);

            if (patient == null)
                return HttpNotFound();

            return View(patient);
        }


        public ActionResult Edit(int id)
        {
            var patient = _service.GetPatientById(id);

            if (patient == null)
                return HttpNotFound();

            return View(patient);
        }

        [HttpPost]
        public ActionResult Edit(int id, Patient patient)
        {
            if (ModelState.IsValid)
            {
                _service.UpdatePatient(id, patient);
                return RedirectToAction("Index");
            }

            return View(patient);
        }
    }
}