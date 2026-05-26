using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Repository
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient? GetPatientById(int id);
        Patient? RegisterPatient(Patient patient);
        bool UpdatePatient(Patient patient);

    }
}
