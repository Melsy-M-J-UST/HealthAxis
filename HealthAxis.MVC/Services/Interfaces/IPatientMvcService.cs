using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IPatientMvcService
    {
        IEnumerable<PatientDto> GetAll(string insuranceStatus = null);

        PatientDto GetById(int id);

        bool Create(
            PatientDto dto,
            out string error);

        bool Update(
            PatientDto dto,
            out string error);

        bool Delete(
            int id,
            out string error);
    }
}