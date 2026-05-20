using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repository;
using System;
using System.Collections.Generic;

namespace HAP_Pod4_ConsoleApp_au.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly List<Patient> _patients;

        public PatientRepository()
        {
            _patients = new List<Patient>();
        }

        public List<Patient> GetAllPatients()
        {
            return _patients;
        }

        public Patient? GetPatientById(int id)
        {
            foreach (var patient in _patients)
            {
                if (patient.PatientId == id)
                {
                    return patient;
                }
            }

            return null;
        }

        public Patient RegisterPatient(Patient patient)
        {
            _patients.Add(patient);

            return patient;
        }
    }
}