using HealthAxis.Repositories.Impl;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service.Impl
{
    public class PatientService : IPatientService
    {
        private IPatientRepository _repository;

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
            return _repository.GetPatientById(patientId);
        }
        public Patient RegisterPatient(Patient patient)
        {
            return _repository.RegisterPatient(patient);
        }
    }
}
