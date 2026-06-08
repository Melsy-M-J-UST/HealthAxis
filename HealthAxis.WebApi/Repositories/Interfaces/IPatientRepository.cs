using HealthAxis.Api.Data;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAll(string insuranceStatus = null);

        Patient GetById(int id);

        Patient GetByEmail(string email);

        Patient Add(Patient patient);

        bool Update(Patient patient);

        bool Deactivate(int id);

        int GetAppointmentCount(int patientId);
    }
}