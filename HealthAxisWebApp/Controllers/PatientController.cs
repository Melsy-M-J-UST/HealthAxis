using HealthAxis.Shared.DTOs;
using HealthAxisWebApp.ApiClients;
using System;
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

        public ActionResult Index()
        {
            return RedirectToAction("ApiIndex");
        }

        public async Task<ActionResult> ApiIndex()
        {
            var patients = await _apiClient.GetAllPatients();
            return View(patients);
        }

        public async Task<ActionResult> Details(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
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
            if (!ModelState.IsValid)
            {
                LoadGenderDropdown(patient.Gender);
                return View(patient);
            }

            try
            {
                await _apiClient.CreatePatient(patient);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
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

            LoadGenderDropdown(patient.Gender);
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PatientDto patient)
        {
            if (id != patient.PatientId)
            {
                return new HttpStatusCodeResult(400, "Patient ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                LoadGenderDropdown(patient.Gender);
                return View(patient);
            }

            try
            {
                await _apiClient.UpdatePatient(patient);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                LoadGenderDropdown(patient.Gender);
                return View(patient);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var patient = await _apiClient.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiClient.DeletePatient(id);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var patient = await _apiClient.GetPatientById(id);
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