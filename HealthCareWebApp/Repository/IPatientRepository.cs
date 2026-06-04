using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareWebApp.Repository
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int patientid);
        Patient RegisterPatient(Patient patient);
        bool UpdatePatient(Patient updatedPatient);
    }
}
