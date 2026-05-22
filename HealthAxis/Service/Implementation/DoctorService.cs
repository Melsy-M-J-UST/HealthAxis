using HealthAxis.Models;
using HealthAxis.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service.Implementation
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

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation)
        {
            return _repository.SearchDoctorBySpecialisation(specialisation);
        }
        public Doctor? GetDoctorById(int doctorId)
        {
            return _repository.GetDoctorById(doctorId);
        }
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }
    }
}
