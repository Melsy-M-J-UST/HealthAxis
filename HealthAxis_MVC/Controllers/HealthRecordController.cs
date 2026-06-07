using System.Linq;
using System.Web.Mvc;
using HealthAxis_MVC.Models;
using static HealthAxis_MVC.Database.AppContextDB;

namespace HealthAxis_MVC.Controllers
{
    public class HealthRecordController : Controller
    {
        public ActionResult Index()
        {
            return View(Records);
        }

        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(HealthRecord record)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(record);
            }

            record.HealthRecordId = Records.Count + 1;
            Records.Add(record);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var record = Records.First(x => x.HealthRecordId == id);

            ViewBag.Patients = new SelectList(Patients, "PatientId", "FullName", record.PatientId);
            ViewBag.Doctors = new SelectList(Doctors, "DoctorId", "FullName", record.DoctorId);

            return View(record);
        }

        [HttpPost]
        public ActionResult Edit(HealthRecord record)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(record);
            }

            var existing = Records.First(x => x.HealthRecordId == record.HealthRecordId);

            existing.PatientId = record.PatientId;
            existing.DoctorId = record.DoctorId;
            existing.VisitDate = record.VisitDate;
            existing.Diagnosis = record.Diagnosis;
            existing.Prescription = record.Prescription;
            existing.Notes = record.Notes;

            return RedirectToAction("Index");
        }

        private void LoadDropdowns()
        {
            ViewBag.Patients = new SelectList(Patients, "PatientId", "FullName");
            ViewBag.Doctors = new SelectList(Doctors, "DoctorId", "FullName");
        }
    }
}