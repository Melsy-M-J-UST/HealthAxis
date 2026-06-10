using HealthAxis.Shared.Models;
using System.Collections.Generic;

namespace HealthAxis.Shared.Services.Interfaces
{
    public interface IPatientService
    {
        List<Patient> GetAllPatients();

        List<Patient> GetPatients(string sortBy, string insuranceFilter);

        Patient GetPatientById(int id);

        void AddPatient(Patient patient);

        void UpdatePatient(Patient patient);

        void DeletePatient(int id);

        void DeactivatePatient(int id);

        int GetAppointmentCount(int patientId);
    }
}