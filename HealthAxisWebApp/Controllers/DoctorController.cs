using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxisWebApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxisWebApp.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorApiClient _apiClient;

        public DoctorController()
        {
            _apiClient = new DoctorApiClient();
        }

        public async Task<ActionResult> Index(
            string sortBy = "name",
            string specialisation = "all")
        {
            var doctors = await _apiClient.GetDoctors(sortBy, specialisation);

            ViewBag.SortBy = sortBy;
            ViewBag.Specialisation = specialisation;

            LoadSpecialisationDropdown(null, includeAll: false);

            return View(doctors);
        }

        public async Task<ActionResult> DoctorProfile(int id)
        {
            var doctor = await _apiClient.GetDoctorProfile(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DoctorProfileModal", doctor);
            }

            return View(doctor);
        }

        public ActionResult Create()
        {
            LoadSpecialisationDropdown();
            return View(new DoctorDto
            {
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                LoadSpecialisationDropdown(doctor.Specialisation);
                return View(doctor);
            }

            try
            {
                await _apiClient.CreateDoctor(doctor);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AddDoctorFieldError(doctor, ex.Message);
                LoadSpecialisationDropdown(doctor.Specialisation);
                return View(doctor);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var doctor = await _apiClient.GetDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            LoadSpecialisationDropdown(doctor.Specialisation);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditModal", doctor);
            }

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, DoctorDto doctor)
        {
            if (id != doctor.DoctorId)
            {
                return new HttpStatusCodeResult(400, "Doctor ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                LoadSpecialisationDropdown(doctor.Specialisation);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", doctor);
                }

                return View(doctor);
            }

            try
            {
                var existingDoctor = await _apiClient.GetDoctorById(id);

                if (existingDoctor == null)
                {
                    return HttpNotFound();
                }

                doctor.IsActive = existingDoctor.IsActive;

                await _apiClient.UpdateDoctor(doctor);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AddDoctorFieldError(doctor, ex.Message);
                LoadSpecialisationDropdown(doctor.Specialisation);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", doctor);
                }

                return View(doctor);
            }
        }

        public async Task<ActionResult> Toggle(int id)
        {
            var doctor = await _apiClient.GetDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ToggleModal", doctor);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Toggle")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleConfirmed(int id)
        {
            try
            {
                await _apiClient.ToggleDoctorStatus(id);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var doctor = await _apiClient.GetDoctorById(id);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ToggleModal", doctor);
                }

                return RedirectToAction("Index");
            }
        }

        private void LoadSpecialisationDropdown(
            int? selectedSpecialisation = null,
            bool includeAll = false)
        {
            var specialisations = Enum.GetValues(typeof(DoctorSpecialisation))
                .Cast<DoctorSpecialisation>()
                .Select(s => new
                {
                    Value = (int)s,
                    Text = s.ToString()
                })
                .ToList();

            if (includeAll)
            {
                specialisations.Insert(0, new
                {
                    Value = -1,
                    Text = "All"
                });
            }

            ViewBag.SpecialisationList = new SelectList(
                specialisations,
                "Value",
                "Text",
                selectedSpecialisation
            );
        }

        private void AddDoctorFieldError(DoctorDto doctor, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return;
            }

            if (message.Contains("Doctor name") || message.Contains("Name") || message.Contains("name"))
            {
                ModelState.AddModelError(nameof(doctor.FullName), message);
            }
            else if (message.Contains("experience") || message.Contains("Experience"))
            {
                ModelState.AddModelError(nameof(doctor.YearsOfExperience), message);
            }
            else if (message.Contains("fee") || message.Contains("Fee") || message.Contains("Consultation"))
            {
                ModelState.AddModelError(nameof(doctor.ConsultationFee), message);
            }
            else if (message.Contains("Specialisation") || message.Contains("specialisation"))
            {
                ModelState.AddModelError(nameof(doctor.Specialisation), message);
            }
            else
            {
                ModelState.AddModelError("", message);
            }
        }
    }
}