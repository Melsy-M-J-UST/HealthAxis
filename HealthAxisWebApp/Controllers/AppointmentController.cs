using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using HealthAxisWebApp.ApiClients;
using System;
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
            return RedirectToAction("ApiIndex");
        }

        public async Task<ActionResult> ApiIndex()
        {
            var appointments = await _appointmentApiClient.GetAllAppointments();
            return View(appointments);
        }

        public async Task<ActionResult> Details(int id)
        {
            var appointment = await _appointmentApiClient.GetAppointmentById(id);

            if (appointment == null)
            {
                return HttpNotFound();
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
                    appointment.Status
                );

                return View(appointment);
            }

            try
            {
                await _appointmentApiClient.CreateAppointment(appointment);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status
                );

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

            await LoadDropdowns(
                appointment.PatientId,
                appointment.DoctorId,
                appointment.TimeSlot,
                appointment.Status
            );

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
                    appointment.Status
                );

                return View(appointment);
            }

            try
            {
                await _appointmentApiClient.UpdateAppointment(appointment);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                await LoadDropdowns(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.TimeSlot,
                    appointment.Status
                );

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
                return RedirectToAction("ApiIndex");
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
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                await _appointmentApiClient.CompleteAppointment(id);
                return RedirectToAction("ApiIndex");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }

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
                return RedirectToAction("ApiIndex");
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
            var patients = await _patientApiClient.GetAllPatients();
            var doctors = await _doctorApiClient.GetAllDoctors();

            ViewBag.PatientList = new SelectList(
                patients,
                "PatientId",
                "FullName",
                selectedPatientId
            );

            ViewBag.DoctorList = new SelectList(
                doctors,
                "DoctorId",
                "FullName",
                selectedDoctorId
            );

            var timeSlots = Enum.GetValues(typeof(AppointmentTimeSlot))
                .Cast<AppointmentTimeSlot>()
                .Select(t => new
                {
                    Value = (int)t,
                    Text = t.ToString()
                })
                .ToList();

            ViewBag.TimeSlotList = new SelectList(
                timeSlots,
                "Value",
                "Text",
                selectedTimeSlot
            );

            var statuses = Enum.GetValues(typeof(AppointmentStatus))
                .Cast<AppointmentStatus>()
                .Select(s => new
                {
                    Value = (int)s,
                    Text = s.ToString()
                })
                .ToList();

            ViewBag.StatusList = new SelectList(
                statuses,
                "Value",
                "Text",
                selectedStatus
            );
        }
    }
}