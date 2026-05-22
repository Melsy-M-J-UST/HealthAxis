using HealthAxis.Models;
using System.Collections.Generic;

namespace HealthAxis.Repositories
{
    public interface IHealthRecordRepository
    {
        HealthRecord AddRecord(HealthRecord record);

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}