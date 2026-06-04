using HealthAxis_MVC.Models;
using HealthAxis_MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAxis_MVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _service;

        public DoctorController()
        {
            
        }
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

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

            _service.UpdateDoctor(doctor.DoctorId,doctor);
            return RedirectToAction("Index");

        }

    }
}