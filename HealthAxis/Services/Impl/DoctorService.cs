using HealthAxis.Models;
using HealthAxis.Repositories;
using System.Collections.Generic;

namespace HealthAxis.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public Doctor AddDoctor(Doctor doctor)
        {
            _repository.AddDoctor(doctor);
            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            return _repository.SearchDoctorBySpecialisation(specialisation);
        }
    }
}