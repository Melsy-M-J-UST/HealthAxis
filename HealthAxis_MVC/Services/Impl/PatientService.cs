using HealthAxis_MVC.Models;
using HealthAxis_MVC.Repositories;
using System.Collections.Generic;

namespace HealthAxis_MVC.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;

        public PatientService()
        {
            _repo = new Repositories.Impl.PatientRepository();
        }

        public void AddPatient(Patient patient)
        {
            _repo.AddPatient(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return _repo.GetAllPatients();
        }

        public Patient GetById(int id)
        {
            return _repo.GetById(id);
        }

        public void UpdatePatient(int id, Patient patient)
        {
            _repo.UpdatePatient(id, patient);
        }
    }
}