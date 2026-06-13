using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxis.Shared.Models;

namespace HealthAxis.Shared.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository repository;

        private static readonly TimeSpan RegexTimeout =
            TimeSpan.FromMilliseconds(250);

        public DoctorService(IDoctorRepository repository)
        {
            this.repository = repository;
        }

        public List<Doctor> GetAllDoctors()
        {
            return repository.GetAll();
        }

        public List<Doctor> GetAllDoctors(string sortBy, string specialisation)
        {
            return repository.GetAll(sortBy, specialisation);
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
            Doctor doctor = repository.GetById(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            repository.Delete(id);
        }

        public int GetUpcomingAppointmentCount(int doctorId)
        {
            return repository.GetUpcomingAppointmentCount(doctorId);
        }

        private static void ValidateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor));
            }

            if (string.IsNullOrWhiteSpace(doctor.FullName))
            {
                throw new ArgumentException(
                    "Doctor name is required.",
                    nameof(doctor));
            }

            if (!Regex.IsMatch(
                doctor.FullName,
                @"^[a-zA-Z\s]+$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException(
                    "Doctor name can contain only letters and spaces.",
                    nameof(doctor));
            }

            if (doctor.YearsOfExperience < 0)
            {
                throw new ArgumentException(
                    "Years of experience cannot be negative.",
                    nameof(doctor));
            }

            if (doctor.ConsultationFee <= 0)
            {
                throw new ArgumentException(
                    "Consultation fee must be greater than zero.",
                    nameof(doctor));
            }

            if (doctor.Specialisation <= 0)
            {
                throw new ArgumentException(
                    "Please select a valid specialisation.",
                    nameof(doctor));
            }
        }
    }
}