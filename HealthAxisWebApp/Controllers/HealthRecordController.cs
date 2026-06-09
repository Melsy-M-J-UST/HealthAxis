using HealthAxis.Shared.DTOs;
using HealthAxisWebApp.ApiClients;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxisWebApp.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly HealthRecordApiClient _healthRecordApiClient;
        private readonly AppointmentApiClient _appointmentApiClient;

        public HealthRecordController()
        {
            _healthRecordApiClient = new HealthRecordApiClient();
            _appointmentApiClient = new AppointmentApiClient();
        }

        public ActionResult Index()
        {
            return RedirectToAction("ApiIndex");
        }

        public async Task<ActionResult> ApiIndex()
        {
            var healthRecords = await _healthRecordApiClient.GetAllHealthRecords();

            return View(healthRecords);
        }

        public async Task<ActionResult> Details(int id)
        {
            var healthRecord = await _healthRecordApiClient.GetHealthRecordById(id);

            if (healthRecord == null)
            {
                return HttpNotFound();
            }

            return View(healthRecord);
        }

        public async Task<ActionResult> Create()
        {
            await LoadAppointmentDropdown();

            return View(new HealthRecordDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HealthRecordDto healthRecord)
        {
            if (healthRecord.AppointmentId == null || healthRecord.AppointmentId <= 0)
            {
                ModelState.AddModelError("AppointmentId", "Appointment is required.");
            }

            if (string.IsNullOrWhiteSpace(healthRecord.Diagnosis))
            {
                ModelState.AddModelError("Diagnosis", "Diagnosis is required.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAppointmentDropdown(healthRecord.AppointmentId);

                return View(healthRecord);
            }

            try
            {
                await _healthRecordApiClient.CreateHealthRecord(healthRecord);

                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadAppointmentDropdown(healthRecord.AppointmentId);

                return View(healthRecord);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var healthRecord = await _healthRecordApiClient.GetHealthRecordById(id);

            if (healthRecord == null)
            {
                return HttpNotFound();
            }

            return View(healthRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, HealthRecordDto healthRecord)
        {
            if (id != healthRecord.RecordId)
            {
                return new HttpStatusCodeResult(400, "Health record ID mismatch.");
            }

            if (string.IsNullOrWhiteSpace(healthRecord.Diagnosis))
            {
                ModelState.AddModelError("Diagnosis", "Diagnosis is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(healthRecord);
            }

            try
            {
                await _healthRecordApiClient.UpdateHealthRecord(healthRecord);

                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(healthRecord);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var healthRecord = await _healthRecordApiClient.GetHealthRecordById(id);

            if (healthRecord == null)
            {
                return HttpNotFound();
            }

            return View(healthRecord);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _healthRecordApiClient.DeleteHealthRecord(id);

                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var healthRecord = await _healthRecordApiClient.GetHealthRecordById(id);

                return View(healthRecord);
            }
        }

        private async Task LoadAppointmentDropdown(int? selectedAppointmentId = null)
        {
            var appointments = await _appointmentApiClient.GetAllAppointments();

            var appointmentItems = appointments
                .Select(a => new
                {
                    Value = a.AppointmentId,
                    Text = "Appointment #" + a.AppointmentId
                           + " | " + a.PatientName
                           + " | " + a.DoctorName
                           + " | " + a.ScheduledDate.ToString("yyyy-MM-dd")
                           + " | " + a.TimeSlotName
                           + " | " + a.StatusName
                })
                .ToList();

            ViewBag.AppointmentList = new SelectList(
                appointmentItems,
                "Value",
                "Text",
                selectedAppointmentId
            );
        }
    }
}