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
           return _repository.AddDoctor(doctor);
        }

        public Doctor? GetById(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new ArgumentException("Invalid doctor ID ");
            }
            var doctor = _repository.GetById(doctorId);
            if (doctor == null)
            {
                throw new HealthAxis.Exceptions.DoctorNotFoundException($"Doctor with id {doctorId} not found.");
            }
            return doctor;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            var doctors = _repository.SearchDoctorBySpecialisation(specialisation);
            if (doctors == null || doctors.Count == 0)
            {
                throw new HealthAxis.Exceptions.DoctorNotFoundException("No doctors found with the given specialization.");
            }
            return doctors;
        }

        public bool UpdateDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentException("Patient is required.");

            if (string.IsNullOrWhiteSpace(doctor.FullName))
                throw new ArgumentException("Patient name is required.");

            return _repository.UpdateDoctor(doctor);
        }
    }
}