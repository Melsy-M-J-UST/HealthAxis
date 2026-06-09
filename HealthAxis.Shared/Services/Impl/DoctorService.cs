using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxisWebApp;

namespace HealthAxis.Shared.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository repository;

        public DoctorService(IDoctorRepository repository)
        {
            this.repository = repository;
        }

        public List<Doctor> GetAllDoctors()
        {
            return repository.GetAll();
        }

        public Doctor GetDoctorById(int id)
        {
            return repository.GetById(id);
        }

        public void AddDoctor(Doctor doctor)
        {
            ValidateDoctor(doctor);

            repository.Add(doctor);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            ValidateDoctor(doctor);

            repository.Update(doctor);
        }

        public void DeleteDoctor(int id)
        {
            repository.Delete(id);
        }

        private void ValidateDoctor(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.FullName))
            {
                throw new Exception(
                    "Doctor name is required.");
            }

            if (!Regex.IsMatch(
                doctor.FullName,
                @"^[a-zA-Z\s]+$"))
            {
                throw new Exception(
                    "Doctor name can contain only letters.");
            }

            if (doctor.YearsOfExperience < 0)
            {
                throw new Exception(
                    "Years of experience cannot be negative.");
            }

            if (doctor.ConsultationFee <= 0)
            {
                throw new Exception(
                    "Consultation fee must be greater than zero.");
            }

            if (doctor.Specialisation <= 0)
            {
                throw new Exception(
                    "Please select a valid specialisation.");
            }
        }
    }

}
