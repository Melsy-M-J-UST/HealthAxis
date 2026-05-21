using HealthAxis.Models;

namespace HealthAxis.Service
{
    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);
        Patient? GetPatientById(int patientId);
        List<Patient> GetAllPatients();
    }
}
