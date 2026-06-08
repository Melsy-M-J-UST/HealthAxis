using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IHealthRecordMvcService
    {
        IEnumerable<HealthRecordDto> GetPatientHistory(int patientId);
        bool Create(HealthRecordDto dto, out string errorMessage);
    }
}
