using HealthAxis.Shared.DTOs;
using HealthAxisWebApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxisWebApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientApiClient _apiClient;
        private const string InactivePatientMessage =
            "Patient's account has been disabled, no operations allowed.";

        public PatientController()
        {
            _apiClient = new PatientApiClient();
        }

        public async Task<ActionResult> Index(
            string sortBy = "name",
            string filter = "all",
            string searchBy = "id",
            string searchValue = null)
        {
            ViewBag.SortBy = sortBy;
            ViewBag.Filter = filter;
            ViewBag.SearchBy = searchBy;
            ViewBag.SearchValue = searchValue;
            ViewBag.HasSearched = false;
            ViewBag.Message = null;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                ViewBag.HasSearched = true;

                if (searchBy == "id")
                {
                    if (!int.TryParse(searchValue, out int patientId))
                    {
                        ViewBag.Message = "Please enter a valid Patient ID.";
                        return View(new List<PatientDto>());
                    }

                    var patient = await _apiClient.GetPatientById(patientId);

                    if (patient == null)
                    {
                        ViewBag.Message = "Patient does not exists";
                        return View(new List<PatientDto>());
                    }

                    return View(new List<PatientDto> { patient });
                }
                else if (searchBy == "name")
                {
                    var patients = await _apiClient.SearchPatients(
                        "name",
                        searchValue,
                        sortBy,
                        filter);

                    if (patients == null || patients.Count == 0)
                    {
                        ViewBag.Message = "No patients found.";
                        return View(new List<PatientDto>());
                    }

                    return View(patients);
                }
            }

            var allPatients = await _apiClient.GetPatients(sortBy, filter);
            return View(allPatients);
        }


        public new async Task<ActionResult> Profile(int id)
        {
            var patientStatus = await _apiClient.GetPatientById(id);

            if (patientStatus == null)
            {
                return HttpNotFound();
            }

            if (!patientStatus.IsActive)
            {
                return HandleInactivePatientAccess();
            }

            var patient = await _apiClient.GetPatientProfile(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProfileModal", patient);
            }

            return View(patient);
        }

        public ActionResult Create()
        {
            LoadGenderDropdown();
            return View(new PatientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientDto patient)
        {
            patient.InsuranceID = string.IsNullOrWhiteSpace(patient.InsuranceID)
                ? null
                : patient.InsuranceID.Trim();

            if (!ModelState.IsValid)
            {
                LoadGenderDropdown(patient.Gender);
                return View(patient);
            }

            try
            {
                await _apiClient.CreatePatient(patient);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AddPatientFieldError(patient, ex.Message);
                LoadGenderDropdown(patient.Gender);
                return View(patient);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            if (!patient.IsActive)
            {
                return HandleInactivePatientAccess();
            }

            LoadGenderDropdown(patient.Gender);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditModal", patient);
            }

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PatientDto patient)
        {
            patient.InsuranceID = string.IsNullOrWhiteSpace(patient.InsuranceID)
                ? null
                : patient.InsuranceID.Trim();

            if (id != patient.PatientId)
            {
                return new HttpStatusCodeResult(400);
            }

            var existingPatient = await _apiClient.GetPatientById(id);

            if (existingPatient == null)
            {
                return HttpNotFound();
            }

            if (!existingPatient.IsActive)
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("<div class='p-4 text-danger fw-semibold'>Patient's account has been disabled, no operations allowed.</div>");
                }

                TempData["ErrorMessage"] = InactivePatientMessage;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                LoadGenderDropdown(patient.Gender);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", patient);
                }

                return View(patient);
            }

            try
            {
                await _apiClient.UpdatePatient(patient);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AddPatientFieldError(patient, ex.Message);
                LoadGenderDropdown(patient.Gender);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", patient);
                }

                return View(patient);
            }
        }

        public async Task<ActionResult> Deactivate(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            if (!patient.IsActive)
            {
                return HandleInactivePatientAccess();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeactivateModal", patient);
            }

            return View(patient);
        }

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            if (!patient.IsActive)
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("<div class='p-4 text-danger fw-semibold'>Patient's account has been disabled, no operations allowed.</div>");
                }

                TempData["ErrorMessage"] = InactivePatientMessage;
                return RedirectToAction("Index");
            }

            try
            {
                await _apiClient.DeactivatePatient(id);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var latestPatient = await _apiClient.GetPatientById(id);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_DeactivateModal", latestPatient);
                }

                return View(latestPatient);
            }
        }

        private void LoadGenderDropdown(int? selectedGender = null)
        {
            ViewBag.GenderList = new SelectList(
                new[]
                {
                    new { Value = 0, Text = "Male" },
                    new { Value = 1, Text = "Female" },
                    new { Value = 2, Text = "Other" }
                },
                "Value",
                "Text",
                selectedGender
            );
        }

        private void AddPatientFieldError(PatientDto patient, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return;
            }

            if (message.Contains("Phone number"))
            {
                ModelState.AddModelError(nameof(patient.PhoneNumber), message);
            }
            else if (message.Contains("Insurance"))
            {
                ModelState.AddModelError(nameof(patient.InsuranceID), message);
            }
            else if (message.Contains("Name") || message.Contains("name"))
            {
                ModelState.AddModelError(nameof(patient.FullName), message);
            }
            else if (message.Contains("Email") || message.Contains("email"))
            {
                ModelState.AddModelError(nameof(patient.Email), message);
            }
            else if (message.Contains("Date of Birth") || message.Contains("DOB"))
            {
                ModelState.AddModelError(nameof(patient.DateOfBirth), message);
            }
            else
            {
                ModelState.AddModelError("", message);
            }
        }

        private ActionResult HandleInactivePatientAccess()
        {
            if (Request.IsAjaxRequest())
            {
                return Content("<div class='p-4 text-danger fw-semibold'>Patient's account has been disabled, no operations allowed.</div>");
            }

            TempData["ErrorMessage"] = InactivePatientMessage;
            return RedirectToAction("Index");
        }
    }
}