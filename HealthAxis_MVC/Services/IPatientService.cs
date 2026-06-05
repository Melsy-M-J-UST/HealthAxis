using HealthAxis_MVC.Models;
using System.Collections.Generic;

namespace HealthAxis_MVC.Services
{
    public interface IPatientService
    {
        void AddPatient(Patient patient);

        List<Patient> GetAllPatients();

        Patient GetById(int id);

        void UpdatePatient(int id, Patient patient);
    }
}