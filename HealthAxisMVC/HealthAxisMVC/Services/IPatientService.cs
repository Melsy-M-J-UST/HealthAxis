using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisMVC.Services
{
    public interface IPatientService
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void RegisterPatient(Patient patient);
        void UpdatePatient(int id, Patient patient);
    }
}
