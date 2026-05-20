using HAP_Pod4_ConsoleApp_au.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HAP_Pod4_ConsoleApp_au.Repository
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient? GetPatientById(int id);
        Patient RegisterPatient(Patient patient);
    }
}
