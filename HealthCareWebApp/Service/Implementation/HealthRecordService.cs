using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Service.Implementation
{
    public class HealthRecordService : IHealthRecordService
    {
        public HealthRecord AddRecord(HealthRecord record)
        {
            throw new NotImplementedException();
        }
        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            throw new NotImplementedException();
        }
        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }
    }
}