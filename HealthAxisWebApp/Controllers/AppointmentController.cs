using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxisWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxisWebApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentApiClient _appointmentApiClient;
        private readonly PatientApiClient _patientApiClient;
        private readonly DoctorApiClient _doctorApiClient;

        public AppointmentController()
        {
            _appointmentApiClient = new AppointmentApiClient();
            _patientApiClient = new PatientApiClient();
            _doctorApiClient = new DoctorApiClient();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public async Task<ActionResult> MyAppointments(int? patientId)
        {
            ViewBag.HasSearched = false;
            ViewBag.Message = null;

            if (!patientId.HasValue)
            {
                return View(new List<AppointmentDto>());
            }

            ViewBag.HasSearched = true;

            var patient = await _patientApiClient.GetPatientById(patientId.Value);

            if (patient == null)
            {
                ViewBag.Message = "Patient does not exists";
                return View(new List<AppointmentDto>());
            }

            var list = await _appointmentApiClient.GetAppointmentsByPatient(patientId.Value);

            if (list == null || !list.Any())
            {
                ViewBag.Message = "No appointments found for this patient.";
                return View(new List<AppointmentDto>());
            }

            return View(list);
        }

        public async Task<ActionResult> SelectDoctor(int? doctorId, string mode)
        {
            var doctors = await _doctorApiClient.GetAllDoctors();

            ViewBag.DoctorList = new SelectList(
                doctors,
                "DoctorId",
                "FullName",
                doctorId
            );

            ViewBag.HasSearched = false;
            ViewBag.Mode = mode;
            ViewBag.Message = null;

            if (!doctorId.HasValue || string.IsNullOrWhiteSpace(mode))
            {
                return View(new List<AppointmentDto>());
            }

            ViewBag.HasSearched = true;

            List<AppointmentDto> appointments = new List<AppointmentDto>();

            if (mode == "today")
            {
                appointments = await _appointmentApiClient.GetTodayAppointments(doctorId.Value);
            }
            else if (mode == "weekly")
            {
                appointments = await _appointmentApiClient.GetWeeklyAppointments(doctorId.Value);
            }

            if (appointments == null || !appointments.Any())
            {
                ViewBag.Message = mode == "today"
                    ? "No appointments found for today."
                    : "No appointments found for this week.";

                return View(new List<AppointmentDto>());
            }

            return View(appointments);
        }

        public async Task<ActionResult> Today(int doctorId)
        {
            var list = await _appointmentApiClient.GetTodayAppointments(doctorId);
            return View(list);
        }

        public async Task<ActionResult> Weekly(int doctorId)
        {
            var list = await _appointmentApiClient.GetWeeklyAppointments(doctorId);
            return View(list);
        }

        public async Task<ActionResult> Details(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DetailsModal", appointment);
            }

            return View(appointment);
        }

        public async Task<ActionResult> Create()
        {
            var model = new AppointmentDto
            {
                ScheduledDate = DateTime.Today.AddDays(1),
                Status = (int)AppointmentStatus.Pending
            };

            await LoadDropdowns(
                model.PatientId,
                model.DoctorId,
                model.TimeSlot,
                model.Status,
                null);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppointmentDto appointment)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status,
                    null);

                return View(appointment);
            }

            try
            {
                await _appointmentApiClient.CreateAppointment(appointment);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status,
                    null);

                return View(appointment);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (appointment.Status == 2 || appointment.Status == 3)
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("<div class='p-4 text-danger fw-semibold'>Cancelled or completed appointments cannot be edited.</div>");
                }

                TempData["ErrorMessage"] = "Cancelled or completed appointments cannot be edited.";
                return RedirectToAction("List");
            }

            await LoadDropdowns(
                appointment.PatientId,
                appointment.DoctorId,
                appointment.TimeSlot,
                appointment.Status,
                null);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditModal", appointment);
            }

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AppointmentDto appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return new HttpStatusCodeResult(400, "Appointment ID mismatch.");
            }

            var existing = await _appointmentApiClient.GetAppointmentById(appointment.AppointmentId);

            if (existing == null)
            {
                return HttpNotFound();
            }

            if (existing.Status == 2 || existing.Status == 3)
            {
                ModelState.AddModelError("", "Cancelled or completed appointments cannot be edited.");

                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status,
                    null);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", appointment);
                }

                return View(appointment);
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status,
                    null);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", appointment);
                }

                return View(appointment);
            }

            try
            {
                appointment.Status = existing.Status;

                await _appointmentApiClient.UpdateAppointment(appointment);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status,
                    null);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditModal", appointment);
                }

                return View(appointment);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _appointmentApiClient.DeleteAppointment(id);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var appointment = await _appointmentApiClient.GetAppointmentById(id);
                return View(appointment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentApiClient.ConfirmAppointment(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                var appointment = await _appointmentApiClient.GetAppointmentById(id);

                if (appointment == null)
                {
                    return HttpNotFound();
                }

                await _appointmentApiClient.CompleteAppointment(id);

                return RedirectToAction(
                    "Create",
                    "HealthRecord",
                    new
                    {
                        appointmentId = appointment.AppointmentId,
                        patientId = appointment.PatientId,
                        doctorId = appointment.DoctorId
                    });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        public async Task<ActionResult> List(string status)
        {
            var appointments = await _appointmentApiClient.GetAllAppointments();

            if (!string.IsNullOrEmpty(status))
            {
                int statusValue = int.Parse(status);
                appointments = appointments
                    .Where(a => a.Status == statusValue)
                    .ToList();
            }

            ViewBag.CurrentStatus = status;

            return View("Index", appointments);
        }

        public async Task<ActionResult> Cancel(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CancelModal", appointment);
            }

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, string cancellationReason)
        {
            if (string.IsNullOrWhiteSpace(cancellationReason))
            {
                ModelState.AddModelError("", "Cancellation reason is required.");

                var appointment = await _appointmentApiClient.GetAppointmentById(id);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_CancelModal", appointment);
                }

                return View(appointment);
            }

            try
            {
                await _appointmentApiClient.CancelAppointment(id, cancellationReason);

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var appointment = await _appointmentApiClient.GetAppointmentById(id);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_CancelModal", appointment);
                }

                return View(appointment);
            }
        }

        // =========================================================
        // NEW: Search patient by ID for Create Appointment
        // =========================================================
        [HttpGet]
        public async Task<JsonResult> SearchPatientById(int patientId)
        {
            var patient = await _patientApiClient.GetPatientById(patientId);

            if (patient == null)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "Patient not found."
                    },
                    JsonRequestBehavior.AllowGet);
            }

            if (!patient.IsActive)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "Patient's account has been disabled, no operations allowed."
                    },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    success = true,
                    patient = new
                    {
                        patient.PatientId,
                        patient.FullName,
                        patient.DateOfBirth,
                        patient.Gender,
                        patient.GenderName,
                        patient.PhoneNumber,
                        patient.Email,
                        patient.InsuranceID
                    }
                },
                JsonRequestBehavior.AllowGet);
        }

        // =========================================================
        // NEW: Load doctors by specialisation for Create Appointment
        // =========================================================
        [HttpGet]
        public async Task<JsonResult> GetDoctorsBySpecialisation(int specialisation)
        {
            var doctors = await _doctorApiClient.GetAllDoctors();

            var filteredDoctors = doctors
                .Where(d => d.IsActive && d.Specialisation == specialisation)
                .Select(d => new
                {
                    d.DoctorId,
                    d.FullName
                })
                .ToList();

            return Json(filteredDoctors, JsonRequestBehavior.AllowGet);
        }

        private async Task LoadDropdowns(
            int? selectedPatientId = null,
            int? selectedDoctorId = null,
            int? selectedTimeSlot = null,
            int? selectedStatus = null,
            int? selectedSpecialisation = null)
        {
            var patients = await _patientApiClient.GetPatients("name", "all");
            var doctors = await _doctorApiClient.GetAllDoctors();

            ViewBag.PatientList = new SelectList(
                patients,
                "PatientId",
                "FullName",
                selectedPatientId);

            // If specialisation not passed but doctor selected, infer it
            if (!selectedSpecialisation.HasValue && selectedDoctorId.HasValue)
            {
                var selectedDoctor = doctors.FirstOrDefault(d => d.DoctorId == selectedDoctorId.Value);
                if (selectedDoctor != null)
                {
                    selectedSpecialisation = selectedDoctor.Specialisation;
                }
            }

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
                selectedSpecialisation);

            var filteredDoctors = selectedSpecialisation.HasValue
                ? doctors.Where(d => d.IsActive && d.Specialisation == selectedSpecialisation.Value).ToList()
                : new List<DoctorDto>();

            ViewBag.DoctorList = new SelectList(
                filteredDoctors,
                "DoctorId",
                "FullName",
                selectedDoctorId);

            var timeSlots = Enum.GetValues(typeof(AppointmentTimeSlot))
                .Cast<AppointmentTimeSlot>()
                .Select(t => new
                {
                    Value = (int)t,
                    Text = GetTimeSlotDisplayName((int)t)
                })
                .ToList();

            ViewBag.TimeSlotList = new SelectList(timeSlots, "Value", "Text", selectedTimeSlot);

            var statuses = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(s => new
                {
                    Value = (int)s,
                    Text = s.ToString()
                })
                .ToList();

            ViewBag.StatusList = new SelectList(statuses, "Value", "Text", selectedStatus);
        }

        private string GetTimeSlotDisplayName(int slot)
        {
            switch (slot)
            {
                case 1: return "10:00 a.m. - 10:30 a.m.";
                case 2: return "10:30 a.m. - 11:00 a.m.";
                case 3: return "11:00 a.m. - 11:30 a.m.";
                case 4: return "11:30 a.m. - 12:00 p.m.";
                case 5: return "12:00 p.m. - 12:30 p.m.";
                case 6: return "12:30 p.m. - 01:00 p.m.";
                case 7: return "01:00 p.m. - 01:30 p.m.";
                case 8: return "01:30 p.m. - 02:00 p.m.";
                case 9: return "02:00 p.m. - 02:30 p.m.";
                case 10: return "02:30 p.m. - 03:00 p.m.";
                case 11: return "03:00 p.m. - 03:30 p.m.";
                case 12: return "03:30 p.m. - 04:00 p.m.";
                default: return "Unknown Slot";
            }
        }
    }
}
