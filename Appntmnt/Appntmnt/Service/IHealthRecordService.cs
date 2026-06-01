using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Service
{
    public interface IHealthRecordService
    {
        HealthRecord? AddRecord(HealthRecord record);

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}
