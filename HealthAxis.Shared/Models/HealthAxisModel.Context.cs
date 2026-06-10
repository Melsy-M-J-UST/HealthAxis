namespace HealthAxis.Shared.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HealthAxisDBEntities : DbContext
    {
        public HealthAxisDBEntities()
            : base("name=HealthAxisDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<HealthRecord> HealthRecords { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
    }
}
