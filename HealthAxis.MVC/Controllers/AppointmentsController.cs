using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentMvcService _appointments;
        private readonly IDoctorMvcService _doctors;

        public AppointmentsController(
            IAppointmentMvcService appointments,
            IDoctorMvcService doctors)
        {
            _appointments = appointments;
            _doctors = doctors;
        }

        private void LoadDropdowns()
        {
            ViewBag.DoctorId = new SelectList(
                _doctors.GetAll(null, true),
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
            LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(AppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(dto);
            }

            string errorMessage;

            bool result = _appointments.Book(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                LoadDropdowns();
                return View(dto);
            }

            return RedirectToAction(
                "MyAppointments",
                new { patientId = dto.PatientId });
        }

        public ActionResult MyAppointments(int? patientId)
        {
            if (patientId == null)
            {
                return View("PatientIdRequired");
            }

            var appointments = _appointments.GetByPatient(patientId.Value);

            return View(appointments);
        }

        public ActionResult DoctorAppointments(int? doctorId)
        {
            if (doctorId == null)
            {
                return View("DoctorIdRequired");
            }

            var appointments = _appointments.GetByDoctor(doctorId.Value);

            return View(appointments);
        }

        public ActionResult TodaySchedule(int? doctorId)
        {
            if (doctorId == null)
            {
                return View("DoctorIdRequired");
            }

            var appointments = _appointments.Today(doctorId.Value);

            return View("DoctorAppointments", appointments);
        }

        public ActionResult WeeklySchedule(int? doctorId, DateTime? startDate)
        {
            if (doctorId == null)
            {
                return View("DoctorIdRequired");
            }

            var weekStartDate = startDate ?? DateTime.Today;

            var appointments = _appointments.Weekly(
                doctorId.Value,
                weekStartDate);

            return View("DoctorAppointments", appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(AppointmentDto dto)
        {
            string errorMessage;

            var statusUpdateDto = new AppointmentStatusUpdateDto
            {
                AppointmentId = dto.AppointmentId,
                Status = dto.Status,
                CancellationReason = dto.CancellationReason
            };

            bool result = _appointments.UpdateStatus(
                dto.AppointmentId,
                statusUpdateDto,
                out errorMessage);

            TempData[result ? "Success" : "Error"] =
                result ? "Status updated." : errorMessage;

            if (result && dto.Status == AppointmentStatusEnum.Completed)
            {
                return RedirectToAction(
                    "Create",
                    "HealthRecords",
                    new
                    {
                        patientId = dto.PatientId,
                        doctorId = dto.DoctorId
                    });
            }

            return RedirectToAction(
                "DoctorAppointments",
                new { doctorId = dto.DoctorId });
        }
    }
}