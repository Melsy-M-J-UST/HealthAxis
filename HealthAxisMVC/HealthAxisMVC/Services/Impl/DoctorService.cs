using HealthAxisMVC.Exceptions;
using HealthAxisMVC.Models;
using HealthAxisMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Services.Impl
{
    public class DoctorService : IDoctorService 
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }
        public void AddDoctor(Doctor doctor)
        {
            _repository.AddDoctor(doctor);
        }

        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

        public Doctor GetById(int doctorId)
        {
            try
            {
               return _repository.GetById(doctorId);
            }
            catch(Exception)
            {
                throw new HealthAppException("There are no doctors with this ID");
            }
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            try
            {
                return _repository.SearchDoctorBySpecialisation(specialisation);
            }
            catch(Exception)
            {
                throw new HealthAppException("Invalid specialisation");
            }
        }

        public void UpdateDoctor(int id, Doctor doctor)
        {
            try
            {
                _repository.UpdateDoctor(id, doctor);
            }
            catch (Exception)
            {
                throw new HealthAppException("Invalid id");
            }
        }
    }
}