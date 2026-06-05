using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HealthAxis_Web.Models;

namespace HealthAxis_Web.Database
{
    public class AppDBContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }

    }
}