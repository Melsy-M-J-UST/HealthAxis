using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IPatientMvcService
    {
        IEnumerable<PatientDto> GetAllPatients(string insuranceStatus = null);

        PatientDto GetPatientById(int id);

        bool CreatePatient(PatientDto dto, out string errorMessage);

        bool UpdatePatient(PatientDto dto, out string errorMessage);

        bool DeactivatePatient(int id, out string errorMessage);
    }
}