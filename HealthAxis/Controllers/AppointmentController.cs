using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentMvcService _appointmentService;
        private readonly IDoctorMvcService _doctorService;

        public AppointmentsController(
            IAppointmentMvcService appointmentService,
            IDoctorMvcService doctorService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
        }

        private void LoadDropdowns()
        {
            ViewBag.DoctorId = new SelectList(
                _doctorService.GetActiveDoctors(),
                "DoctorId",
                "FullName");

            ViewBag.TimeSlots = new SelectList(new List<string>
            {
                "09:00 AM - 09:30 AM",
                "09:30 AM - 10:00 AM",
                "10:00 AM - 10:30 AM",
                "10:30 AM - 11:00 AM",
                "11:00 AM - 11:30 AM",
                "02:00 PM - 02:30 PM",
                "02:30 PM - 03:00 PM",
                "03:00 PM - 03:30 PM"
            });
        }

        public ActionResult Book()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Patient")
            {
                return RedirectToAction("Login", "Account", new { role = "Patient" });
            }

            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(AppointmentDto dto)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Patient")
            {
                return RedirectToAction("Login", "Account", new { role = "Patient" });
            }

            dto.PatientId = Convert.ToInt32(Session["ReferenceId"]);

            ModelState.Remove("PatientId");
            ModelState.Remove("Status");
            ModelState.Remove("CancellationReason");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(dto);
            }

            bool result = _appointmentService.Book(dto, out string errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                LoadDropdowns();
                return View(dto);
            }

            return RedirectToAction("MyAppointments");
        }

        public ActionResult MyAppointments()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Patient")
            {
                return RedirectToAction("Login", "Account", new { role = "Patient" });
            }

            int patientId = Convert.ToInt32(Session["ReferenceId"]);
            var appointments = _appointmentService.GetPatientAppointments(patientId);
            return View(appointments);
        }

        public ActionResult DoctorAppointments()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account", new { role = "Doctor" });
            }

            int doctorId = Convert.ToInt32(Session["ReferenceId"]);
            var appointments = _appointmentService.GetDoctorAppointments(doctorId);
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(AppointmentDto dto)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Doctor")
            {
                return RedirectToAction("Login", "Account", new { role = "Doctor" });
            }

            bool result = _appointmentService.UpdateStatus(dto, out string errorMessage);

            if (result)
            {
                TempData["Success"] = "Appointment updated successfully.";
            }
            else
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("DoctorAppointments");
        }
    }
}
