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

        public PatientController()
        {
            _apiClient = new PatientApiClient();
        }

        public async Task<ActionResult> Index(
            string sortBy = "name",
            string filter = "all",
            int? patientId = null)
        {
            ViewBag.SortBy = sortBy;
            ViewBag.Filter = filter;
            ViewBag.HasSearched = false;
            ViewBag.Message = null;
            ViewBag.PatientId = patientId;

            if (patientId.HasValue)
            {
                ViewBag.HasSearched = true;

                var patient = await _apiClient.GetPatientById(patientId.Value);

                if (patient == null)
                {
                    ViewBag.Message = "Patient does not exists";
                    return View(new List<PatientDto>());
                }

                return View(new List<PatientDto> { patient });
            }

            var patients = await _apiClient.GetPatients(sortBy, filter);
            return View(patients);
        }

        public new async Task<ActionResult> Profile(int id)
        {
            var patient = await _apiClient.GetPatientProfile(id);

            if (patient == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView("_ProfileModal", patient);

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
                if (ex.Message.Contains("Phone number"))
                {
                    ModelState.AddModelError(nameof(patient.PhoneNumber), ex.Message);
                }
                else if (ex.Message.Contains("Name"))
                {
                    ModelState.AddModelError(nameof(patient.FullName), ex.Message);
                }
                else if (ex.Message.Contains("Email"))
                {
                    ModelState.AddModelError(nameof(patient.Email), ex.Message);
                }
                else if (ex.Message.Contains("Date of Birth"))
                {
                    ModelState.AddModelError(nameof(patient.DateOfBirth), ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                }

                LoadGenderDropdown(patient.Gender);
                return View(patient);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
                return HttpNotFound();

            LoadGenderDropdown(patient.Gender);

            if (Request.IsAjaxRequest())
                return PartialView("_EditModal", patient);

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
                return new HttpStatusCodeResult(400);

            if (!ModelState.IsValid)
            {
                LoadGenderDropdown(patient.Gender);

                if (Request.IsAjaxRequest())
                    return PartialView("_EditModal", patient);

                return View(patient);
            }

            try
            {
                await _apiClient.UpdatePatient(patient);

                if (Request.IsAjaxRequest())
                    return Json(new { success = true });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Phone number"))
                {
                    ModelState.AddModelError(nameof(patient.PhoneNumber), ex.Message);
                }
                else if (ex.Message.Contains("Name"))
                {
                    ModelState.AddModelError(nameof(patient.FullName), ex.Message);
                }
                else if (ex.Message.Contains("Email"))
                {
                    ModelState.AddModelError(nameof(patient.Email), ex.Message);
                }
                else if (ex.Message.Contains("Date of Birth"))
                {
                    ModelState.AddModelError(nameof(patient.DateOfBirth), ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                }

                LoadGenderDropdown(patient.Gender);

                if (Request.IsAjaxRequest())
                    return PartialView("_EditModal", patient);

                return View(patient);
            }
        }

        public async Task<ActionResult> Deactivate(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView("_DeactivateModal", patient);

            return View(patient);
        }

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            try
            {
                await _apiClient.DeactivatePatient(id);

                if (Request.IsAjaxRequest())
                    return Json(new { success = true });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var patient = await _apiClient.GetPatientById(id);

                if (Request.IsAjaxRequest())
                    return PartialView("_DeactivateModal", patient);

                return View(patient);
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
    }
}