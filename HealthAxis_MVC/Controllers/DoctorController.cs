using HealthAxis_MVC.Models;
using HealthAxis_MVC.Repositories;
using System.Linq;
using System.Web.Mvc;
using static HealthAxis_MVC.Database.AppContextDB;

namespace HealthAxis_MVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _service;

        public DoctorController(IDoctorRepository service)
        {
            _service = service;
        }

        // GET: Doctor
        public ActionResult Index()
        {
            var doctors = _service.GetAllDoctors();
            return View(doctors);
        }
        public ActionResult Create()
        {
            int nextId = Doctors.Any() ? Doctors.Max(d => d.DoctorId) + 1 : 1;

            var doctor = new Doctor
            {
                DoctorId = nextId
            };

            return View(doctor);
        }

        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            doctor.DoctorId = Doctors.Any() ? Doctors.Max(d => d.DoctorId) + 1 : 1;

            _service.AddDoctor(doctor);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var doctor = _service.GetById(id);
            return View(doctor);
        }

        [HttpPost]
        public ActionResult Edit(Doctor doctor)
        {
            _service.UpdateDoctor(doctor.DoctorId, doctor);
            return RedirectToAction("Index");
        }
    }
}