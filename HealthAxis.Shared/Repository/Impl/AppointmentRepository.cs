using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HealthAxisWebApp.Repositories.Interfaces;

namespace HealthAxisWebApp.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthAxisDBEntities db;

        public AppointmentRepository()
        {
            db = new HealthAxisDBEntities();
        }

        public List<Appointment> GetAll()
        {
            return db.Appointments
                     .Include(a => a.Patient)
                     .Include(a => a.Doctor)
                     .ToList();
        }

        public Appointment GetById(int id)
        {
            return db.Appointments
                     .Include(a => a.Patient)
                     .Include(a => a.Doctor)
                     .FirstOrDefault(a =>
                         a.AppointmentId == id);
        }

        public void Add(Appointment appointment)
        {
            db.Appointments.Add(appointment);
            db.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            db.Entry(appointment).State =
                EntityState.Modified;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Appointment appointment =
                db.Appointments.Find(id);

            if (appointment != null)
            {
                db.Appointments.Remove(appointment);
                db.SaveChanges();
            }
        }
    }

}
