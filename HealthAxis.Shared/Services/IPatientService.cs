using HealthAxisWebApp;
using System.Collections.Generic;

namespace HealthAxis.Shared.Services.Interfaces
{
    public interface IPatientService
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int id);

        void AddPatient(Patient patient);

        void UpdatePatient(Patient patient);

        void DeletePatient(int id);
    }
}
