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
        public string AddDoctor(Doctor doctor)
        {
            Database.Doctors.Add(doctor);
            return "Doctor Added Successfully";
        }
        public List<Doctor> SearchDoctorBySpecialisation(string specialisation)
        {
            var doctors = Database.Doctors
                .Where(doc => doc.Specialisation == specialisation)
                .ToList();

            if (!doctors.Any())
            {
                throw new DoctorNotFoundException("No doctors found with the given specialization.");
            }

            return doctors;
        }
    }
