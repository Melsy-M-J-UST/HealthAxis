using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using System.Collections.Generic;
using System.Linq;

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
            _db.Patients.Add(patient);
            Console.WriteLine("Patient registered successfully.");
            return patient;
        }

        public List<Patient> GetAllPatients()
        {
            return _db.Patients.ToList();
        }

        public Patient? GetPatientById(int patientid)
        {
            var patient = _db.Patients
                .FirstOrDefault(p => p.PatientId == patientid);

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
            existingPatient.InsuranceID = updatedPatient.InsuranceId;
            existingPatient.Gender = updatedPatient.Gender;
            return true;
        }
    }
}