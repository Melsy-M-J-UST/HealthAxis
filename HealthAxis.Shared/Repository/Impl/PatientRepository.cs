using HealthAxis.Shared.Models;
using HealthAxisWebApp.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public List<Patient> GetAllActive(string sortBy, string insuranceFilter)
        {
            var query = db.Patients.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(insuranceFilter) && insuranceFilter != "all")
            {
                if (insuranceFilter == "insured")
                {
                    query = query.Where(p => !string.IsNullOrEmpty(p.InsuranceID));
                }
                else if (insuranceFilter == "uninsured")
                {
                    query = query.Where(p => string.IsNullOrEmpty(p.InsuranceID));
                }
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
            patient.IsActive = true;
            patient.InsuranceID = string.IsNullOrWhiteSpace(patient.InsuranceID)
                ? null
                : patient.InsuranceID.Trim();

            db.Patients.Add(patient);
            db.SaveChanges();
        }

        public void Update(Patient patient)
        {
            patient.InsuranceID = string.IsNullOrWhiteSpace(patient.InsuranceID)
                ? null
                : patient.InsuranceID.Trim();

            db.Entry(patient).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Patient patient = db.Patients.Find(id);

            if (patient != null)
            {
                db.Patients.Remove(patient);
                db.SaveChanges();
            }
        }

        public bool EmailExists(string email)
        {
            return db.Patients.Any(p => p.Email == email);
        }

        public bool EmailExists(string email, int excludePatientId)
        {
            return db.Patients.Any(p => p.Email == email && p.PatientId != excludePatientId);
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

        public int GetAppointmentCount(int patientId)
        {
            return db.Appointments.Count(a => a.PatientId == patientId);
        }
    }
}