using System.Collections.Generic;
using HealthAxis.Shared.Models;

namespace HealthAxisWebApp.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();
        List<Patient> GetAllActive(string sortBy, string filter);
        List<Patient> SearchByName(string name, string sortBy, string filter);
        Patient GetById(int id);
        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(int id);
        void Deactivate(int id);
        bool EmailExists(string email);
        bool EmailExists(string email, int excludePatientId);
        bool InsuranceIdExists(string insuranceId);
        bool InsuranceIdExists(string insuranceId, int excludePatientId);
        int GetAppointmentCount(int patientId);
    }
}
