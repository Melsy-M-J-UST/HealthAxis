using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientMvcService _patients;
        private readonly IDoctorMvcService _doctors;

        public PatientsController(
            IPatientMvcService patients,
            IDoctorMvcService doctors)
        {
            _patients = patients;
            _doctors = doctors;
        }

        public ActionResult Index(string insuranceStatus)
        {
            ViewBag.InsuranceStatus = insuranceStatus;

            var patients = _patients.GetAll(insuranceStatus);

            return View(patients);
        }
        public new ActionResult Profile()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var patient = _patients.GetById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        public ActionResult Create()
        {
            LoadGender();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadGender();
                return View(dto);
            }

            string errorMessage;

            bool result = _patients.Create(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);

                LoadGender();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var patient = _patients.GetById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            LoadGender();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadGender();
                return View(dto);
            }

            string errorMessage;

            bool result = _patients.Update(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);

                LoadGender();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var patient = _patients.GetById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string errorMessage;

            bool result = _patients.Delete(id, out errorMessage);

            if (!result)
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("Index");
        }

        public ActionResult SearchDoctors(SpecialisationEnum? specialisation)
        {
            LoadSpecialisation();

            string selectedSpecialisation = specialisation.HasValue
                ? specialisation.Value.ToString()
                : null;

            var doctors = _doctors.GetAll(
                selectedSpecialisation,
                true);

            return View(doctors);
        }

        private void LoadGender()
        {
            ViewBag.GenderList = new SelectList(
                Enum.GetValues(typeof(GenderEnum)));
        }

        private void LoadSpecialisation()
        {
            ViewBag.SpecialisationList = new SelectList(
                Enum.GetValues(typeof(SpecialisationEnum)));
        }
    }
}