using HealthAxis.Api.Data;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories.Interfaces
{
    public interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetByPatient(int patientId);
        HealthRecord Add(HealthRecord record);
    }
}