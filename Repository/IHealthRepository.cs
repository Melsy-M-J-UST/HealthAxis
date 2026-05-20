using System;
using System.Collections.Generic;
using System.Text;
using HAP_Pod4_ConsoleApp_au.Models;

namespace AppointmentPortal.ConsoleApp.Repositories
{
    public interface IHealthRepository
    {
        HealthRecord AddRecord(HealthRecord record);

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);
    }
}
