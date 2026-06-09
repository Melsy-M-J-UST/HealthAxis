using HealthAxisMVC.Database;
using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Repositories.Impl
{
    public class PatientRepository : IPatientRepository
    {
        public List<Patient> GetAllPatients()
        {
            return AppDB.Patients;
        }

        public Patient GetPatientById(int id)
        {
            var patient = AppDB.Patients.FirstOrDefault(p => p.PatientId == id);
            return patient;
        }

        public void RegisterPatient(Patient patient)
        {
            AppDB.Patients.Add(patient);
        }

        public void UpdatePatient(int id, Patient patient)
        {
            var existingPatient = AppDB.Patients.First(p => p.PatientId == id);
            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Email = patient.Email;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;

        }
    }
}