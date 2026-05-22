using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service
{
    public interface IHealthRecordService
    {
        HealthRecord AddRecord(HealthRecord record);
        List<HealthRecord> GetRecordsByPatient(int patientId);
        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}
