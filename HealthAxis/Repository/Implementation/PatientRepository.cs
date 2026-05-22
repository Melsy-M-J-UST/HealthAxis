using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly Database _db;
        public PatientRepository(Database db)
        {
            _db = db;
        }

        public List<Patient> GetAllPatients()
        {
            return _db.Patients.ToList();
        }

        public Patient? GetPatientById(int patientid)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.PatientId == patientid);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with id {patientid} not registered.");
            }
            return patient;
        }

        public Patient RegisterPatient(Patient patient)
        {
            _db.Patients.Add(patient);
            Console.WriteLine("Patient Added successfully");
            return patient;
        }
    }
}
