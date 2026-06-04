using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthCareWebApp.Models;
using HealthCareWebApp.Service;

namespace HealthCareWebApp.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        private readonly IDoctorService _service;

        public DoctorController()
        {

        }
        public DoctorController(IDoctorService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            var doctors = _service.GetAllDoctors();
            return View(doctors);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Doctor Doctor)
        {
            _service.AddDoctor(Doctor);
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var doctor = _service.GetDoctorById(id);
            return View(doctor);
        }
        [HttpPost]
        public ActionResult Update(Doctor Doctor)
        {
            _service.UpdateDoctor(Doctor);
            return RedirectToAction("Index");
        }
    }
}