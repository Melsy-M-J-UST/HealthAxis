using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IPatientService
    {
        IEnumerable<PatientDto> GetAll(string insuranceStatus = null);

        PatientDto GetById(int id);

        bool Create(PatientDto dto, out string errorMessage);

        bool Update(int id, PatientDto dto, out string errorMessage);

        bool Deactivate(int id);
    }
}