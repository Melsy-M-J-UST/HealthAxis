using HealthAxis_MVC.Database;
using HealthAxis_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class AppointmentController : Controller
{
    AppointmentRepository repo = new AppointmentRepository();

    public ActionResult Index()
    {
        return View(repo.GetAll());
    }

    public ActionResult Create()
    {
        int nextId = AppContextDB.Appointments.Any()
            ? AppContextDB.Appointments.Max(a => a.AppointmentId) + 1
            : 1;

        ViewBag.Specialisations = new SelectList(
            Enum.GetValues(typeof(Doctor.SpecialisationType))
        );

        ViewBag.Patients = new SelectList(AppContextDB.Patients, "PatientId", "FullName");

        ViewBag.Doctors = new SelectList(Enumerable.Empty<SelectListItem>());

        ViewBag.Slots = GetSlotList(null, 0);

        return View(new Appointment { AppointmentId = nextId });
    }

    [HttpPost]
    public ActionResult Create(Appointment appt)
    {
        if (!appt.ScheduledDate.HasValue)
        {
            ModelState.AddModelError("ScheduledDate", "Date is required");
        }
        else
        {
            var date = appt.ScheduledDate.Value.Date;

            if (date <= DateTime.Today)
            {
                ModelState.AddModelError("ScheduledDate", "Appointment must be from tomorrow");
            }

            if (date > DateTime.Today.AddMonths(6))
            {
                ModelState.AddModelError("ScheduledDate", "Cannot book beyond 6 months");
            }
        }

        bool alreadyBookedSameDay = AppContextDB.Appointments.Any(a =>
            a.PatientId == appt.PatientId &&
            a.DoctorId == appt.DoctorId &&
            a.ScheduledDate == appt.ScheduledDate);

        if (alreadyBookedSameDay)
        {
            ModelState.AddModelError("ScheduledDate",
                "This patient already has an appointment with this doctor on this date");
        }

        bool slotBooked = AppContextDB.Appointments.Any(a =>
            a.DoctorId == appt.DoctorId &&
            a.ScheduledDate == appt.ScheduledDate &&
            a.Slot == appt.Slot);

        if (slotBooked)
        {
            ModelState.AddModelError("Slot", "This slot is already booked");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Specialisations = new SelectList(
                Enum.GetValues(typeof(Doctor.SpecialisationType))
            );

            ViewBag.Patients = new SelectList(
                AppContextDB.Patients, "PatientId", "FullName"
            );

            if (appt.Specialisation.HasValue)
            {
                var doctors = AppContextDB.Doctors
                    .Where(d => d.Specialisation == appt.Specialisation.Value)
                    .ToList();

                ViewBag.Doctors = new SelectList(doctors, "DoctorId", "FullName");
            }
            else
            {
                ViewBag.Doctors = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            ViewBag.Slots = GetSlotList(appt.ScheduledDate, appt.DoctorId);

            return View(appt);
        }

        repo.Add(appt);

        return RedirectToAction("Index");
    }



    public ActionResult Confirm(int id)
    {
        repo.UpdateStatus(id, Appointment.AppointmentStatus.Confirmed);
        return RedirectToAction("Index");
    }

    private List<SelectListItem> GetSlotList(DateTime? date, int doctorId)
    {
        var booked = AppContextDB.Appointments
            .Where(a => a.DoctorId == doctorId && a.ScheduledDate == date)
            .Select(a => a.Slot)
            .ToList();

        return Enum.GetValues(typeof(Appointment.SlotType))
            .Cast<Appointment.SlotType>()
            .Select(s => new SelectListItem
            {
                Value = s.ToString(),
                Text = GetSlotText(s),
                Disabled = booked.Contains(s)
            }).ToList();
    }

    private string GetSlotText(Appointment.SlotType slot)
    {
        switch (slot)
        {
            case Appointment.SlotType.Slot1: return "09:00 - 10:00";
            case Appointment.SlotType.Slot2: return "10:00 - 11:00";
            case Appointment.SlotType.Slot3: return "11:00 - 12:00";
            case Appointment.SlotType.Slot4: return "02:00 - 03:00";
            case Appointment.SlotType.Slot5: return "03:00 - 04:00";
            default: return slot.ToString();
        }
    }
    public JsonResult GetDoctorsBySpecialisation(int specialisation)
    {
        var doctors = AppContextDB.Doctors
            .Where(d => (int)d.Specialisation == specialisation)
            .Select(d => new
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName
            })
            .ToList();

        return Json(doctors, JsonRequestBehavior.AllowGet);
    }
}