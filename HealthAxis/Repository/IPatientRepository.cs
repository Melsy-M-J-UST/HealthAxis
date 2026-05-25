using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient? GetPatientById(int patientid);
        Patient RegisterPatient(Patient patient);
        bool UpdatePatient(Patient updatedPatient);
    }
}
