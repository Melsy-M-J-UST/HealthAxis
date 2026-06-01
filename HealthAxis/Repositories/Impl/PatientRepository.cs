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

        public Patient? GetPatientById(int id)
        {
            var patient = _db.Patients
                .FirstOrDefault(p => p.PatientId == id);

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
            existingPatient.InsuranceID = patient.InsuranceID;
            existingPatient.Gender = patient.Gender;
            return true;
        }
    }
}