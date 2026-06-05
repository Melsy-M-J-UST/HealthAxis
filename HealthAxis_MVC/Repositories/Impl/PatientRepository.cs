using HealthAxis_MVC.Models;
using HealthAxis_MVC.Database;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis_MVC.Repositories.Impl
{
    public class PatientRepository : IPatientRepository
    {
        public void AddPatient(Patient patient)
        {
            AppContextDB.Patients.Add(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return AppContextDB.Patients;
        }

        public Patient GetById(int id)
        {
            return AppContextDB.Patients.Single(x => x.PatientId == id);
        }

        public void UpdatePatient(int id, Patient patient)
        {
            var existingPatient = AppContextDB.Patients
                .First(x => x.PatientId == id);

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceID = patient.InsuranceID;
        }
    }
}