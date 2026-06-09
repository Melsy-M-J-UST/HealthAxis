using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HealthAxisWebApp.Repositories.Interfaces;
using HealthAxis.Shared.Services;
using HealthAxisWebApp;
using HealthAxis.Shared.Services.Interfaces;

namespace HealthAxis.Shared.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository repository;

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
            repository.Delete(id);
        }

        private void ValidatePatient(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.FullName))
            {
                throw new Exception(
                    "Patient name is required.");
            }

            if (!Regex.IsMatch(
                patient.FullName,
                @"^[a-zA-Z\s]+$"))
            {
                throw new Exception(
                    "Patient name can contain only letters.");
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                throw new Exception(
                    "Email is required.");
            }

            if (!Regex.IsMatch(
                patient.Email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new Exception(
                    "Invalid email format.");
            }

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new Exception(
                    "Phone number is required.");
            }

            if (!Regex.IsMatch(
                patient.PhoneNumber,
                @"^\d{10}$"))
            {
                throw new Exception(
                    "Phone number must contain 10 digits.");
            }

            if (patient.DateOfBirth >= DateTime.Today)
            {
                throw new Exception(
                    "Invalid date of birth.");
            }
        }
    }
}
