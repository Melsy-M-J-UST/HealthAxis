using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        IEnumerable<HealthRecordDto> GetByPatient(int patientId);

        bool Create(
            HealthRecordDto dto,
            out string errorMessage);
    }
}