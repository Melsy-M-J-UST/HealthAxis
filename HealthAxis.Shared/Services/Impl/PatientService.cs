using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxisWebApp;
using HealthAxis.Shared.Services.Interfaces;

namespace HealthAxis.Shared.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository repository;

        private static readonly TimeSpan RegexTimeout =
            TimeSpan.FromMilliseconds(250);

        public PatientService(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public List<Patient> GetAllPatients()
        {
            return repository.GetAll();
        }

        public Patient GetPatientById(int id)
        {
            return repository.GetById(id);
        }

        public void AddPatient(Patient patient)
        {
            ValidatePatient(patient);

            patient.CreatedDate = DateTime.Now;

            repository.Add(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            ValidatePatient(patient);

            repository.Update(patient);
        }

        public void DeletePatient(int id)
        {
            Patient patient = repository.GetById(id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            repository.Delete(id);
        }

        private static void ValidatePatient(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            if (string.IsNullOrWhiteSpace(patient.FullName))
            {
                throw new ArgumentException(
                    "Patient name is required.",
                    nameof(patient));
            }

            if (!Regex.IsMatch(
                patient.FullName,
                @"^[a-zA-Z\s]+$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException(
                    "Patient name can contain only letters and spaces.",
                    nameof(patient));
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                throw new ArgumentException(
                    "Email is required.",
                    nameof(patient));
            }

            if (!Regex.IsMatch(
                patient.Email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException(
                    "Invalid email format.",
                    nameof(patient));
            }

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new ArgumentException(
                    "Phone number is required.",
                    nameof(patient));
            }

            if (!Regex.IsMatch(
                patient.PhoneNumber,
                @"^\d{10}$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException(
                    "Phone number must contain 10 digits.",
                    nameof(patient));
            }

            if (patient.DateOfBirth >= DateTime.Today)
            {
                throw new ArgumentException(
                    "Invalid date of birth.",
                    nameof(patient));
            }
        }
    }
}