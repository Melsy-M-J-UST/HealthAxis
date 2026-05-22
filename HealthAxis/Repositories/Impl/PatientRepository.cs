using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories.Impl
{
    public class PatientRepository : IPatientRepository
    {
        private readonly Database _db;
        public PatientRepository(Database db)
        {
            _db = db;
        }
        public Patient RegisterPatient(Patient patient)
        {
            _ContextDb.Patients.Add(patient);
            Console.WriteLine("Patient Added successfully");
            return patient;
        }
        public List<Patient> GetAllPatients()
        {
            return _ContextDb.Patients.ToList();
        }

        public Patient? GetPatientById(int patientid)
        {
            var patient = _ContextDb.Patients.FirstOrDefault(p => p.PatientId == patientid);//find also works but not with lambda expression
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with id {patientid} not registered.");
            }
            return patient;
        }
    }
}
