using HealthAxis_MVC.Models;
using HealthAxis_MVC.Repositories;
using System.Collections.Generic;
using HealthAxis_MVC.Exceptions;
using System;

namespace HealthAxis_MVC.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }
        void IDoctorService.AddDoctor(Doctor doctor)
        {
            _repository.AddDoctor(doctor);
        }

        List<Doctor> IDoctorService.GetAllDoctors()
        {
           return _repository.GetAllDoctors();
        }

        Doctor IDoctorService.GetById(int doctorId)
        {
            try
            {
                return _repository.GetById(doctorId);
            }
            catch (Exception ex)
            {
                throw new DoctorNotFoundException(ex.Message);
            }
        }

        void IDoctorService.UpdateDoctor(int id,Doctor doctor)
        {
            try
            {
                _repository.UpdateDoctor(id,doctor);
            }
            catch (Exception ex)
            {
                throw new DoctorNotFoundException(ex.Message);
            }

        }
    }
}