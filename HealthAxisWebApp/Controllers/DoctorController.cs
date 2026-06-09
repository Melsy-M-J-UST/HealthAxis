using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxisWebApp.ApiClients;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HealthAxisWebApp.ApiClients;

namespace HealthAxisWebApp.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorApiClient _apiClient;

        public DoctorController()
        {
            _apiClient = new DoctorApiClient();
        }

        public ActionResult Index()
        {
            return RedirectToAction("ApiIndex");
        }

        public async Task<ActionResult> ApiIndex()
        {
            var doctors = await _apiClient.GetAllDoctors();
            return View(doctors);
        }

        public async Task<ActionResult> Details(int id)
        {
            var doctor = await _apiClient.GetDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
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
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
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
                return View(doctor);
            }

            try
            {
                await _apiClient.UpdateDoctor(doctor);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                LoadSpecialisationDropdown(doctor.Specialisation);
                return View(doctor);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var doctor = await _apiClient.GetDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiClient.DeleteDoctor(id);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var doctor = await _apiClient.GetDoctorById(id);
                return View(doctor);
            }
        }

        private void LoadSpecialisationDropdown(int? selectedSpecialisation = null)
        {
            var specialisations = Enum.GetValues(typeof(DoctorSpecialisation))
                .Cast<DoctorSpecialisation>()
                .Select(s => new
                {
                    Value = (int)s,
                    Text = s.ToString()
                })
                .ToList();

            ViewBag.SpecialisationList = new SelectList(
                specialisations,
                "Value",
                "Text",
                selectedSpecialisation
            );
        }
    }
}