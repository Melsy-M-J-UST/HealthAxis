using HAP_Pod4_ConsoleApp_au.Models;
namespace HAP_Pod4_ConsoleApp_au.Services
{
    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);
        Patient? GetPatientById(int patientId);
        List<Patient> GetAllPatients();
    }
}
