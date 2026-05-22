using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Repositories.Impl
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _db;
        public PatientRepository(AppDbContext db)
        {
            _db = db;
        }

        public Patient RegisterPatient(Patient patient)
        {
            _db.Patients.Add(patient);
            Console.WriteLine("Patient Added successfully");
            return patient;
        }
        public List<Patient> GetAllPatients()
        {
            return _db.Patients.ToList();
        }

        public Patient? GetPatientById(int patientid)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.PatientId == patientid);//find also works but not with lambda expression
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with id {patientid} not registered.");
            }
            return patient;
        }
    }
}
