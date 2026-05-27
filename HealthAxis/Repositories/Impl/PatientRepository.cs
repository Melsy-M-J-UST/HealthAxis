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
            
            if (!string.IsNullOrWhiteSpace(patient.InsuranceId))
            {
                var exists = _db.Patients.Any(p => !string.IsNullOrWhiteSpace(p.InsuranceId) &&
                                                  p.InsuranceId.Equals(patient.InsuranceId, StringComparison.OrdinalIgnoreCase));

                if (exists)
                {
                    
                    return null!;
                }
            }

            _db.Patients.Add(patient);
            return patient;
        }
        public List<Patient> GetAllPatients()
        {
            return _db.Patients.ToList();
        }

        public Patient? GetPatientById(int id)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.PatientId == id);
            return patient;
        }
        
        public bool UpdatePatient(Patient patient)
        {
            var existingPatient = _db.Patients.FirstOrDefault(p => p.PatientId == patient.PatientId);

            if (existingPatient == null)
            {
                return false;
            }

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceId = patient.InsuranceId;
            existingPatient.Gender = patient.Gender;
            return true;
        }
    }
}
