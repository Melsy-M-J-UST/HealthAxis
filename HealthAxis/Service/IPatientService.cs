using HealthAxis.Models;
namespace HealthAxis.Services
{
    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);
        Patient? GetPatientById(int patientId);
        List<Patient> GetAllPatients();
    }
}
