using HealthAxis.Shared.Models;
using HealthAxis.Shared.Services.Interfaces;
using HealthAxisWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public List<Patient> GetPatients(string sortBy, string insuranceFilter)
        {
            return repository.GetAllActive(sortBy, insuranceFilter);
        }

        public Patient GetPatientById(int id)
        {
            return repository.GetById(id);
        }

        public void AddPatient(Patient patient)
        {
            ValidatePatient(patient);

            patient.InsuranceID = string.IsNullOrWhiteSpace(patient.InsuranceID)
                ? null
                : patient.InsuranceID.Trim();

            if (repository.EmailExists(patient.Email))
            {
                throw new ArgumentException("Email already exists.");
            }

            patient.CreatedDate = DateTime.Now;
            patient.IsActive = true;

            repository.Add(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            ValidatePatient(patient);

            patient.InsuranceID = string.IsNullOrWhiteSpace(patient.InsuranceID)
                ? null
                : patient.InsuranceID.Trim();

            if (repository.EmailExists(patient.Email, patient.PatientId))
            {
                throw new ArgumentException("Email already exists.");
            }

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

        public void DeactivatePatient(int id)
        {
            var patient = repository.GetById(id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            repository.Deactivate(id);
        }

        public int GetAppointmentCount(int patientId)
        {
            return repository.GetAppointmentCount(patientId);
        }

        private static void ValidatePatient(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            if (string.IsNullOrWhiteSpace(patient.FullName))
            {
                throw new ArgumentException("Patient name is required.");
            }

            if (!Regex.IsMatch(
                patient.FullName,
                @"^[a-zA-Z\s]+$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException("Patient name can contain only letters and spaces.");
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (!Regex.IsMatch(
                patient.Email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException("Invalid email format.");
            }

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new ArgumentException("Phone number is required.");
            }

            if (!Regex.IsMatch(
                patient.PhoneNumber,
                @"^\d{10}$",
                RegexOptions.None,
                RegexTimeout))
            {
                throw new ArgumentException("Phone number must contain 10 digits.");
            }

            if (patient.DateOfBirth < new DateTime(1900, 1, 1))
            {
                throw new ArgumentException("Date of Birth year must be 1900 or later.");
            }

            if (patient.DateOfBirth >= DateTime.Today)
            {
                throw new ArgumentException("Date of Birth must be before today.");
            }
        }
    }
}