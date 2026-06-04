using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareWebApp.Service
{
    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);
        Patient GetPatientById(int patientId);
        List<Patient> GetAllPatients();
        bool UpdatePatient(Patient patient);
    }
}
