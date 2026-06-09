using HealthAxisMVC.Exceptions;
using HealthAxisMVC.Models;
using HealthAxisMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }
        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }

        public Patient GetPatientById(int id)
        {
            try
            {
                return _repository.GetPatientById(id);
            }
            catch (Exception)
            {
                throw new HealthAppException("There are no patients with this ID");
            }
        }

        public void RegisterPatient(Patient patient)
        {
            _repository.RegisterPatient(patient);
        }

        public void UpdatePatient(int id, Patient patient)
        {
            try
            {
                _repository.UpdatePatient(id, patient);
            }
            catch (Exception)
            {
                throw new HealthAppException("There are no patients with this ID");
            }
        }
    }
}