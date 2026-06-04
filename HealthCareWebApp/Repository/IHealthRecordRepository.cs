using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareWebApp.Repository
{
    public interface IHealthRecordRepository
    {
        HealthRecord AddRecord(HealthRecord record);
        List<HealthRecord> GetRecordsByPatient(int patientId);
        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}
