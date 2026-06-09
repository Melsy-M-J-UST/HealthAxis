using System.Collections.Generic;
using System.Linq;
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
            Patient patient = db.Patients.Find(id);

            if (patient != null)
            {
                db.Patients.Remove(patient);
                db.SaveChanges();
            }
        }
    }

}
