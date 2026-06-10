using HealthAxis.Shared.Models;
using System.Collections.Generic;

namespace HealthAxisWebApp.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();

        List<Patient> GetAllActive(string sortBy, string insuranceFilter);

        Patient GetById(int id);

        void Add(Patient patient);

        void Update(Patient patient);

        void Delete(int id);


        bool EmailExists(string email);

        bool EmailExists(string email, int excludePatientId);

        void Deactivate(int id);

        int GetAppointmentCount(int patientId);
    }
}