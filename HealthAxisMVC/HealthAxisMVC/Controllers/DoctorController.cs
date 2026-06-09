using HealthAxisMVC.Models;
using HealthAxisMVC.Repositories;
using HealthAxisMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAxisMVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;
        public DoctorController(IDoctorService service)
        {
            _service = service;
        }
        // GET: Doctor
        public ActionResult Index()
        {
            var doctorList = _service.GetAllDoctors();
            return View(doctorList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _service.AddDoctor(doctor);
                return RedirectToAction("Index");
            }

            return View(doctor);
        }


        public ActionResult Details(int id)
        {
            try
            {
                var doctor = _service.GetById(id);
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var doctor = _service.GetById(id);
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateDoctor(id, doctor);
                return RedirectToAction("Index");
            }

            return View(doctor);
        }


        [HttpGet]
        public ActionResult SearchBySpecialisation(Doctor.SpecialisationOption? specialisation)
        {
            try
            {
                if (specialisation == null)
                {
                    var allDoctors = _service.GetAllDoctors();
                    return View("Index", allDoctors);
                }

                var doctors = _service.SearchDoctorBySpecialisation(specialisation.Value);
                return View("Index", doctors);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


    }
}