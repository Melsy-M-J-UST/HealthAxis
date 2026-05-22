using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Repositories.Impl;
using HealthAxis.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository _repository)
        {
            this._repository = _repository;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            return _repository.AddDoctor(doctor);
        }


        public Doctor? GetById(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new ArgumentException("Invalid doctor ID.");
            }

            return _repository.GetById(doctorId);
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            {
                return _repository.SearchDoctorBySpecialisation(specialisation);
            }
        }
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

    }
}
