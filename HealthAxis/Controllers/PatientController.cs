using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly IPatientMvcService _patientService;
        private readonly IDoctorMvcService _doctorService;

        public PatientsController(
            IPatientMvcService patientService,
            IDoctorMvcService doctorService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public ActionResult Index(string insuranceStatus)
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "Patient")
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Login", "Account", new { role = "Patient" });
                }

                int patientId = Convert.ToInt32(Session["ReferenceId"]);

                var patient = _patientService.GetPatientById(patientId);

                if (patient == null)
                {
                    return HttpNotFound();
                }

                return View("PatientDashboard", patient);
            }

            ViewBag.InsuranceStatus = insuranceStatus;

            var patients = _patientService.GetAllPatients(insuranceStatus);

            return View(patients);
        }

        public ActionResult Details(int id)
        {
            var patient = _patientService.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        public ActionResult MyProfile()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Patient")
            {
                return RedirectToAction("Login", "Account", new { role = "Patient" });
            }

            int patientId = Convert.ToInt32(Session["ReferenceId"]);

            var patient = _patientService.GetPatientById(patientId);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View("Details", patient);
        }

        public ActionResult Edit(int id)
        {
            var patient = _patientService.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            LoadGenderDropdown();

            return View(patient);
        }

        public ActionResult EditMyProfile()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Patient")
            {
                return RedirectToAction("Login", "Account", new { role = "Patient" });
            }

            int patientId = Convert.ToInt32(Session["ReferenceId"]);

            return RedirectToAction("Edit", new { id = patientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadGenderDropdown();
                return View(dto);
            }

            bool result = _patientService.UpdatePatient(dto, out string errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                LoadGenderDropdown();
                return View(dto);
            }

            if (Session["Role"] != null && Session["Role"].ToString() == "Patient")
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult SearchDoctors(string specialisation)
        {
            LoadSpecialisationDropdown();

            if (string.IsNullOrWhiteSpace(specialisation))
            {
                return View("SearchDoctors", _doctorService.GetActiveDoctors());
            }

            return View("SearchDoctors", _doctorService.GetDoctorsBySpecialisation(specialisation));
        }

        private void LoadGenderDropdown()
        {
            ViewBag.GenderList = new SelectList(new[]
            {
                "Male",
                "Female",
                "Transgender",
                "Other"
            });
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