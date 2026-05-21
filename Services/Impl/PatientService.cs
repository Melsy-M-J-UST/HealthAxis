using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repository;
using HAP_Pod4_ConsoleApp_au.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis1.Services.Impl
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
