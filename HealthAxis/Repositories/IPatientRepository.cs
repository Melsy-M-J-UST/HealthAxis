using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories
{
    public class IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient? GetPatientById(int id);
        Patient RegisterPatient(Patient patient);
    }
}
