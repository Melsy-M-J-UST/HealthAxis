using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Service
{
    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);
        Patient? GetPatientById(int patientId);
        List<Patient> GetAllPatients();
        bool UpdatePatient(Patient patient);
    }
}
