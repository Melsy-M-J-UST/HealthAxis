using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly Database _ContextDb;

        public DoctorRepository(Database contextDb)
        {
            _ContextDb = contextDb;
        }
        public string AddDoctor(Doctor doctor)
        {
            _ContextDb.Doctors.Add(doctor);
            return "Doctor Added Successfully";
        }
        public List<Doctor> SearchDoctorBySpecialisation(string specialisation)
        {
            var doctors = _ContextDb.Doctors
                .Where(doc => doc.Specialisation == specialisation)
                .ToList();

            if (!doctors.Any())
            {
                throw new DoctorNotFoundException("No doctors found with the given specialization.");
            }

            return doctors;
        }
    }
}
