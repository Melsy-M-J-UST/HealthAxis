using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Services.Impl
{
    public class PatientService : IPatientService
    {
        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }

        public Patient? GetPatientById(int patientId)
        {
            return _repository.GetPatientById(patientId);
        }
        public Patient RegisterPatient(Patient patient)
        {
            return _repository.RegisterPatient(patient);
        }
    }
}
