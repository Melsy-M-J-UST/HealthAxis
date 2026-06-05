using HealthAxis_MVC.Models;
using System.Collections.Generic;

namespace HealthAxis_MVC.Repositories
{
    public interface IPatientRepository
    {
        void AddPatient(Patient patient);

        List<Patient> GetAllPatients();

        void UpdatePatient(int id, Patient patient);

        Patient GetById(int id);
    }
}