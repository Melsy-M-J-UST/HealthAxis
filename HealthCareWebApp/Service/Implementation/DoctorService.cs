using HealthCareWebApp.Models;
using HealthCareWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HealthCareWebApp.Exceptions;

namespace HealthCareWebApp.Service.Implementation
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
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }
        public Doctor GetDoctorById(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new DoctorNotFoundException("Invalid doctor ID.");
            }
            var doctor = _repository.GetDoctorById(doctorId);
            return doctor ?? throw new DoctorNotFoundException($"Doctor with id {doctorId} not found.");
        }
        public List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation)
        {
            var doctor = _repository.SearchDoctorBySpecialisation(specialisation);
            if (doctor == null || doctor.Count == 0)
            {
                throw new DoctorNotFoundException("No doctors found with the given specialization.");
            }
            return doctor;
        }
        public bool UpdateDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentException("Patient is required.");

            if (string.IsNullOrWhiteSpace(doctor.DoctorName))
                throw new ArgumentException("Patient name is required.");

            return _repository.UpdateDoctor(doctor);
        }
    }
}