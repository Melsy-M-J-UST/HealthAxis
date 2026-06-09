using HealthAxisMVC.Services;
using HealthAxisMVC.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAxisMVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _service;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public AppointmentController(
            IAppointmentService service,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _service = service;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public ActionResult Index()
        {
            var appointments = _service.GetAllAppointments();
            return View(appointments);
        }

        public ActionResult Create()
        {
            ViewBag.Patients = _patientService.GetAllPatients();
            ViewBag.Doctors = _doctorService.GetAllDoctors();

            return View();
        }
        [HttpPost]
        public ActionResult Create(int patientId, int doctorId, DateTime scheduledDate)
        {
            try
            {
                var patient = _patientService.GetPatientById(patientId);
                var doctor = _doctorService.GetById(doctorId);

                var appointment = _service.BookAppointment(patient, doctor, scheduledDate);

                TempData["Success"] = $"Appointment booked at {appointment.TimeSlot}";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // reload dropdowns just in case of errors
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllDoctors();

                return View();
            }
        }
        public ActionResult Cancel(int id)
        {
            var appointment = _service.GetAppointmentById(id);

            if (appointment == null)
                return HttpNotFound();

            return View(appointment);
        }

        [HttpPost]
        public ActionResult Cancel(int id, string reason)
        {
            try
            {
                _service.CancelAppointment(id, reason);
                TempData["Success"] = "Appointment cancelled successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
        public ActionResult Confirm(int id)
        {
            try
            {
                _service.ConfirmAppointment(id);
                TempData["Success"] = "Appointment confirmed";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
        public ActionResult Complete(int id)
        {
            try
            {
                _service.CompleteAppointment(id);
                TempData["Success"] = "Appointment completed";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

    }

}