using HealthAxis.Models;
using HealthAxis.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service.Implementation
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
                throw new HealthAxis.Exceptions.PatientNotFoundException($"Patient with id {patientId} not registered.");
            }
            return patient;
        }
        public Patient RegisterPatient(Patient patient)
        {
            return _repository.RegisterPatient(patient);
        }
        public bool UpdatePatient(Patient patient)
        {

            if (patient == null)
                throw new ArgumentException("Patient is required.");

            if (string.IsNullOrWhiteSpace(patient.PatientName))
                throw new ArgumentException("Patient name is required.");

            return _repository.UpdatePatient(patient);

        }
    }
}
