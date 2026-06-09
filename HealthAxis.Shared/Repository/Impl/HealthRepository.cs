using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HealthAxisWebApp.Repositories.Interfaces;

namespace HealthAxisWebApp.Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthAxisDBEntities db;

        public HealthRecordRepository()
        {
            db = new HealthAxisDBEntities();
        }

        public List<HealthRecord> GetAll()
        {
            return db.HealthRecords
                     .Include(h => h.Patient)
                     .Include(h => h.Doctor)
                     .ToList();
        }

        public HealthRecord GetById(int id)
        {
            return db.HealthRecords
                     .Include(h => h.Patient)
                     .Include(h => h.Doctor)
                     .FirstOrDefault(h =>
                         h.RecordId == id);
        }

        public void Add(HealthRecord record)
        {
            db.HealthRecords.Add(record);
            db.SaveChanges();
        }

        public void Update(HealthRecord record)
        {
            db.Entry(record).State =
                EntityState.Modified;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            HealthRecord record =
                db.HealthRecords.Find(id);

            if (record != null)
            {
                db.HealthRecords.Remove(record);
                db.SaveChanges();
            }
        }
    }
}
