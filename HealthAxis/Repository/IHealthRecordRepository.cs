using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository
{
    public interface IHealthRecordRepository
    {
        HealthRecord AddRecord(HealthRecord record);
        List<HealthRecord> GetRecordsByPatient(int patientId);
        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}
