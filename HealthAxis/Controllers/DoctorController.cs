using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly IDoctorMvcService _doctorService;

        public DoctorsController(IDoctorMvcService doctorService)
        {
            _doctorService = doctorService;
        }

        public ActionResult Index(string specialisation)
        {
            LoadSpecialisationDropdown();
            var doctors = string.IsNullOrWhiteSpace(specialisation)
                ? _doctorService.GetAllDoctors()
                : _doctorService.GetDoctorsBySpecialisation(specialisation);
            return View(doctors);
        }

        public ActionResult Details(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return HttpNotFound();
            return View(doctor);
        }

        public ActionResult Create()
        {
            LoadSpecialisationDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadSpecialisationDropdown();
                return View(dto);
            }

            bool result = _doctorService.CreateDoctor(dto, out string errorMessage);
            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                LoadSpecialisationDropdown();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return HttpNotFound();
            LoadSpecialisationDropdown();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadSpecialisationDropdown();
                return View(dto);
            }

            bool result = _doctorService.UpdateDoctor(dto, out string errorMessage);
            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                LoadSpecialisationDropdown();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ToggleStatus(int id)
        {
            bool result = _doctorService.ToggleStatus(id, out string errorMessage);
            if (!result) TempData["Error"] = errorMessage;
            return RedirectToAction("Index");
        }

        private void LoadSpecialisationDropdown()
        {
            ViewBag.SpecialisationList = new SelectList(new[]
            {
                "Endocrinologist",
                "Oncologist",
                "Gynecologist",
                "OrthopedicSurgeon",
                "Psychiatrist",
                "Pediatrician",
                "Neurologist",
                "Dermatologist",
                "Cardiologist",
                "GeneralPractitioner"
            });
        }
    }
}
