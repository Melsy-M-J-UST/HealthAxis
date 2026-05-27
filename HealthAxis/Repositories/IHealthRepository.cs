using System;
using System.Collections.Generic;
using System.Text;
using HealthAxis.Models;

namespace HealthAxis.Repositories
{
    public interface IHealthRepository
    {
        HealthRecord? AddRecord(HealthRecord record);

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}
