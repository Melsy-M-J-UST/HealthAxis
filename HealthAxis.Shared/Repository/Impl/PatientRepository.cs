using System.Collections.Generic;
using System.Linq;
using HealthAxis.Shared.Models;
using HealthAxisWebApp.Repositories.Interfaces;

namespace HealthAxisWebApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthAxisDBEntities db;

        public PatientRepository()
        {
            db = new HealthAxisDBEntities();
        }

        public List<Patient> GetAll()
        {
            return db.Patients.ToList();
        }

        public List<Patient> GetAllActive(string sortBy, string filter)
        {
            var query = db.Patients.Where(p => p.IsActive);

            if (filter == "insured")
            {
                query = query.Where(p => p.InsuranceID != null && p.InsuranceID != "");
            }
            else if (filter == "uninsured")
            {
                query = query.Where(p => p.InsuranceID == null || p.InsuranceID == "");
            }

            if (sortBy == "id")
            {
                query = query.OrderBy(p => p.PatientId);
            }
            else
            {
                query = query.OrderBy(p => p.FullName);
            }

            return query.ToList();
        }

        public List<Patient> SearchByName(string name, string sortBy, string filter)
        {
            var query = db.Patients
                .Where(p => p.IsActive && p.FullName.Contains(name));

            if (filter == "insured")
            {
                query = query.Where(p => p.InsuranceID != null && p.InsuranceID != "");
            }
            else if (filter == "uninsured")
            {
                query = query.Where(p => p.InsuranceID == null || p.InsuranceID == "");
            }

            if (sortBy == "id")
            {
                query = query.OrderBy(p => p.PatientId);
            }
            else
            {
                query = query.OrderBy(p => p.FullName);
            }

            return query.ToList();
        }

        public Patient GetById(int id)
        {
            return db.Patients.Find(id);
        }

        public void Add(Patient patient)
        {
            db.Patients.Add(patient);
            db.SaveChanges();
        }

        public void Update(Patient patient)
        {
            db.Entry(patient).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var patient = db.Patients.Find(id);

            if (patient != null)
            {
                db.Patients.Remove(patient);
                db.SaveChanges();
            }
        }

        public void Deactivate(int id)
        {
            var patient = db.Patients.Find(id);

            if (patient != null)
            {
                patient.IsActive = false;
                db.SaveChanges();
            }
        }

        public bool EmailExists(string email)
        {
            return db.Patients.Any(p => p.Email == email);
        }

        public bool EmailExists(string email, int excludePatientId)
        {
            return db.Patients.Any(p =>
                p.PatientId != excludePatientId &&
                p.Email == email);
        }

        public bool InsuranceIdExists(string insuranceId)
        {
            if (string.IsNullOrWhiteSpace(insuranceId))
            {
                return false;
            }

            return db.Patients.Any(p => p.InsuranceID == insuranceId);
        }

        public bool InsuranceIdExists(string insuranceId, int excludePatientId)
        {
            if (string.IsNullOrWhiteSpace(insuranceId))
            {
                return false;
            }

            return db.Patients.Any(p =>
                p.PatientId != excludePatientId &&
                p.InsuranceID == insuranceId);
        }

        public int GetAppointmentCount(int patientId)
        {
            return db.Appointments.Count(a => a.PatientId == patientId);
        }
    }
}