using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Repository.Implementation
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        public HealthRecord AddRecord(HealthRecord record)
        {
            throw new NotImplementedException();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            throw new NotImplementedException();
        }
    }
}