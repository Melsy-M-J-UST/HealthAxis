using HealthCareWebApp.Models;
using HealthCareWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;
using HealthCareWebApp.Exceptions;

namespace HealthCareWebApp.Service.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        public PatientService(IPatientRepository _repository)
        {
            this._repository = _repository;
        }

        public Patient GetPatientById(int patientId)
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

            var result = _repository.RegisterPatient(patient);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to register patient.");
            }
            return result;
        }

        public bool UpdatePatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient is required.");

            if (string.IsNullOrWhiteSpace(patient.PatientName))
                throw new ArgumentException("Patient name is required.");

            return _repository.UpdatePatient(patient);
        }

        List<Patient> IPatientService.GetAllPatients()
        {
            return _repository.GetAllPatients();
        }
    }
}