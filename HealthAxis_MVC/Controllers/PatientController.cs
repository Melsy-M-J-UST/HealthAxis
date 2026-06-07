using HealthAxis_MVC.Database;
using HealthAxis_MVC.Models;
using HealthAxis_MVC.Repositories;
using System.Linq;
using System.Web.Mvc;
using static HealthAxis_MVC.Database.AppContextDB;

namespace HealthAxis_MVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _service;
        public PatientController(IPatientRepository service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var patients = _service.GetAllPatients();
            return View(patients);
        }
        public ActionResult Create()
        {
            int nextId = Patients.Any() ? Patients.Max(d => d.PatientId) + 1 : 1;

            var patient = new Patient
            {
                PatientId = nextId
            };

            return View(patient);
        }

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            patient.PatientId = Patients.Any() ? Patients.Max(d => d.PatientId) + 1 : 1;

            _service.AddPatient(patient);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var patient = _service.GetById(id);
            return View(patient);
        }

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            _service.UpdatePatient(patient.PatientId, patient);
            return RedirectToAction("Index");
        }
    }
}