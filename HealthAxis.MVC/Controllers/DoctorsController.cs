using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorMvcService _doctors;
        private readonly IPatientMvcService _patients;

        public DoctorsController(IDoctorMvcService doctors, IPatientMvcService patients)
        {
            _doctors = doctors;
            _patients = patients;
        }

        public ActionResult Index(SpecialisationEnum? specialisation)
        {
            LoadSpecialisation();

            string selectedSpecialisation = specialisation.HasValue
                ? specialisation.Value.ToString()
                : null;

            var doctors = _doctors.GetAll(selectedSpecialisation);

            return View(doctors);
        }
        public new ActionResult Profile()
        {
            return View();
        }

        public ActionResult PatientList(string insuranceStatus, string sortOrder)
        {
            ViewBag.InsuranceStatus = insuranceStatus;
            ViewBag.SortOrder = sortOrder;

            var patients = _patients.GetAll(insuranceStatus);

            if (sortOrder == "name_desc")
            {
                patients = patients.OrderByDescending(p => p.FullName).ToList();
            }
            else
            {
                patients = patients.OrderBy(p => p.FullName).ToList();
            }

            return View(patients);
        }

        public ActionResult Details(int id)
        {
            var doctor = _doctors.GetById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        public ActionResult Create()
        {
            LoadSpecialisation();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadSpecialisation();
                return View(dto);
            }

            string errorMessage;

            bool result = _doctors.Create(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);

                LoadSpecialisation();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var doctor = _doctors.GetById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            LoadSpecialisation();

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadSpecialisation();
                return View(dto);
            }

            string errorMessage;

            bool result = _doctors.Update(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);

                LoadSpecialisation();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ToggleStatus(int id)
        {
            string errorMessage;

            bool result = _doctors.ToggleStatus(id, out errorMessage);

            if (!result)
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("Index");
        }

        private void LoadSpecialisation()
        {
            ViewBag.SpecialisationList = new SelectList(
                Enum.GetValues(typeof(SpecialisationEnum)));
        }
    }
}