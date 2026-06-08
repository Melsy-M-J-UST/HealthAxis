using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthAxisEntities _context;

        public PatientRepository(HealthAxisEntities context)
        {
            _context = context;
        }

        public IEnumerable<Patient> GetAll(string insuranceStatus = null)
        {
            var patients = _context.Patients.AsQueryable();

            if (insuranceStatus == "Insured")
            {
                patients = patients.Where(p =>
                    p.InsuranceID != null &&
                    p.InsuranceID != "");
            }
            else if (insuranceStatus == "NotInsured")
            {
                patients = patients.Where(p =>
                    p.InsuranceID == null ||
                    p.InsuranceID == "");
            }

            return patients
                .OrderBy(p => p.FullName)
                .ToList();
        }

        public Patient GetById(int id)
        {
            return _context.Patients.Find(id);
        }

        public Patient GetByEmail(string email)
        {
            return _context.Patients
                .FirstOrDefault(p => p.Email == email);
        }

        public Patient Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();

            return patient;
        }

        public bool Update(Patient patient)
        {
            var existingPatient = _context.Patients.Find(patient.PatientId);

            if (existingPatient == null)
            {
                return false;
            }

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceID = patient.InsuranceID;

            _context.SaveChanges();

            return true;
        }

        public bool Deactivate(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return false;
            }

            patient.IsActive = false;

            _context.SaveChanges();

            return true;
        }

        public int GetAppointmentCount(int patientId)
        {
            return _context.Appointments
                .Count(a => a.PatientId == patientId);
        }
    }
}