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
            return [.. _db.Patients];
        }

        public Patient? GetPatientById(int patientid)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.PatientId == patientid);
            return patient;
        }

        public Patient RegisterPatient(Patient patient)
        {
            _db.Patients.Add(patient);
            Console.WriteLine("Patient Added successfully");
            return patient;
        }
        public bool UpdatePatient(Patient updatedPatient)
        {
            var existingPatient = _db.Patients.FirstOrDefault(p => p.PatientId == updatedPatient.PatientId);

            if (existingPatient == null)
            {
                return false;
            }

            existingPatient.PatientName = updatedPatient.PatientName;
            existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.Email = updatedPatient.Email;
            existingPatient.InsuranceId = updatedPatient.InsuranceId;
            existingPatient.Gender = updatedPatient.Gender;
            return true;
        }
    }
}
