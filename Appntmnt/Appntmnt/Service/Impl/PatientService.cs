using Appntmnt.Models;
using Appntmnt.Repository;
using Appntmnt.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Service.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            this._repository = repository;
        }
        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }

        public Patient? GetPatientById(int patientId)
        {
            var patient = _repository.GetPatientById(patientId);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with id {patientId} not registered.");
            }
            return patient;
        }
        public Patient RegisterPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient is required.");

            // Validate InsuranceId format if provided
            if (!string.IsNullOrWhiteSpace(patient.InsuranceId))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(patient.InsuranceId,"^INS\\d{4}$",System.Text.RegularExpressions.RegexOptions.IgnoreCase,TimeSpan.FromMilliseconds(100)))
                {
                    throw new ArgumentException("Insurance ID must follow format INSXXXX where X are digits.");
                }

                // Normalize to uppercase
                patient.InsuranceId = patient.InsuranceId.ToUpperInvariant();
            }

            var result = _repository.RegisterPatient(patient);

            if (result == null)
            {
                throw new InvalidOperationException("Insurance ID already exists. It must be unique for each patient.");
            }

            return result;
        }
        public bool UpdatePatient(Patient patient)
        {

            if (patient == null)
                throw new ArgumentException("Patient is required.");

            if (string.IsNullOrWhiteSpace(patient.FullName))
                throw new ArgumentException("Patient name is required.");

            return _repository.UpdatePatient(patient);

        }
    }
}
