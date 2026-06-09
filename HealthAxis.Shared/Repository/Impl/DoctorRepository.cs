using System.Collections.Generic;
using System.Linq;
using HealthAxisWebApp.Repositories.Interfaces;

namespace HealthAxisWebApp.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAxisDBEntities db;
        public DoctorRepository()
        {
            db = new HealthAxisDBEntities();
        }

        public List<Doctor> GetAll()
        {
            return db.Doctors.ToList();
        }

        public Doctor GetById(int id)
        {
            return db.Doctors.Find(id);
        }

        public void Add(Doctor doctor)
        {
            db.Doctors.Add(doctor);
            db.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            db.Entry(doctor).State =
                System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            if (doctor != null)
            {
                db.Doctors.Remove(doctor);
                db.SaveChanges();
            }
        }
    }
}
