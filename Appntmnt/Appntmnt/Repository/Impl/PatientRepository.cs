using Appntmnt.Data;
using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Repository.Impl
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
            // Ensure InsuranceId uniqueness if provided
            if (!string.IsNullOrWhiteSpace(patient.InsuranceId))
            {
                var exists = _db.Patients.Any(p => !string.IsNullOrWhiteSpace(p.InsuranceId) &&
                                                  p.InsuranceId.Equals(patient.InsuranceId, StringComparison.OrdinalIgnoreCase));

                if (exists)
                {
                    // Return null to indicate repository-level conflict; service will translate to exception
                    return null!;
                }
            }

            _db.Patients.Add(patient);
            Console.WriteLine("Patient Added successfully");
            return patient;
        }
        public List<Patient> GetAllPatients()
        {
            return _db.Patients.ToList();
        }

        public Patient? GetPatientById(int id)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.PatientId == id);
            // Repository should not throw; return null and let service layer handle exceptions
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
