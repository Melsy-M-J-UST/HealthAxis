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

        // ENTRY POINT → REDIRECT TO DASHBOARD
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        // NEW DASHBOARD (PATIENT + DOCTOR CARDS)
        public ActionResult Dashboard()
        {
            return View();
        }

        // PATIENT VIEW

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


        // TODAY SCHEDULE
        public async Task<ActionResult> Today(int doctorId)
        {
            var list = await _appointmentApiClient.GetTodayAppointments(doctorId);
            return View(list);
        }

        // WEEKLY SCHEDULE
        public async Task<ActionResult> Weekly(int doctorId)
        {
            var list = await _appointmentApiClient.GetWeeklyAppointments(doctorId);
            return View(list);
        }

        // DETAILS
        public async Task<ActionResult> Details(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }

        // CREATE
        public async Task<ActionResult> Create()
        {
            var model = new AppointmentDto
            {
                ScheduledDate = DateTime.Today.AddDays(1),
                Status = (int)AppointmentStatus.Pending
            };

            await LoadDropdowns(model.PatientId, model.DoctorId, model.TimeSlot, model.Status);
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
                    appointment.Status);

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
                    appointment.Status);

                return View(appointment);
            }
        }

        // EDIT
        public async Task<ActionResult> Edit(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            await LoadDropdowns(
                appointment.PatientId,
                appointment.DoctorId,
                appointment.TimeSlot,
                appointment.Status);

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

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status);

                return View(appointment);
            }

            try
            {
                var existing = await _appointmentApiClient.GetAppointmentById(appointment.AppointmentId);
                appointment.Status = existing.Status;

                await _appointmentApiClient.UpdateAppointment(appointment);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status);

                return View(appointment);
            }
        }

        // DELETE
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

        // CONFIRM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentApiClient.ConfirmAppointment(id);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // COMPLETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                var appointment =
                    await _appointmentApiClient
                        .GetAppointmentById(id);

                if (appointment == null)
                {
                    return HttpNotFound();
                }

                await _appointmentApiClient
                    .CompleteAppointment(id);

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

                return RedirectToAction(
                    "Details",
                    new { id });
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


        // CANCEL
        public async Task<ActionResult> Cancel(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
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
                return View(appointment);
            }

            try
            {
                await _appointmentApiClient.CancelAppointment(id, cancellationReason);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var appointment = await _appointmentApiClient.GetAppointmentById(id);
                return View(appointment);
            }
        }

        private async Task LoadDropdowns(
            int? selectedPatientId = null,
            int? selectedDoctorId = null,
            int? selectedTimeSlot = null,
            int? selectedStatus = null)
        {
            var patients = await _patientApiClient.GetPatients("name", "all");
            var doctors = await _doctorApiClient.GetAllDoctors();

            ViewBag.PatientList = new SelectList(patients, "PatientId", "FullName", selectedPatientId);
            ViewBag.DoctorList = new SelectList(doctors, "DoctorId", "FullName", selectedDoctorId);

            var timeSlots = Enum.GetValues(typeof(AppointmentTimeSlot))
                .Cast<AppointmentTimeSlot>()
                .Select(t => new
                {
                    Value = (int)t,
                    Text = t.ToString()
                }).ToList();

            ViewBag.TimeSlotList = new SelectList(timeSlots, "Value", "Text", selectedTimeSlot);

            var statuses = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(s => new
                {
                    Value = (int)s,
                    Text = s.ToString()
                }).ToList();

            ViewBag.StatusList = new SelectList(statuses, "Value", "Text", selectedStatus);
        }
    }
}