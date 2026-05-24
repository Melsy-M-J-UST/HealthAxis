using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
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
            var patient = _db.Patients.FirstOrDefault(p => p.PatientId == patientid);
            // Repository should not throw; return null and let service layer handle exceptions
            return patient;
        }
        
        public bool UpdatePatient(Patient updatedPatient)
        {
            var existingPatient = _db.Patients.FirstOrDefault(p => p.PatientId == updatedPatient.PatientId);

            if (existingPatient == null)
            {
                return false;
            }

            existingPatient.FullName = updatedPatient.FullName;
            existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.Email = updatedPatient.Email;
            existingPatient.InsuranceId = updatedPatient.InsuranceId;
            existingPatient.Gender = updatedPatient.Gender;
            return true;
        }
    }
}
