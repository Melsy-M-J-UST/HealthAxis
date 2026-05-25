using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using System;
using System.Collections.Generic;

namespace HAP_Pod4_ConsoleApp_au.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly List<Doctor> _doctors;

        public DoctorRepository()
        {
            _doctors = new List<Doctor>();
        }

        public DoctorRepository(AppDbContext dbcontext)
        {
            Dbcontext = dbcontext;
        }

        public AppDbContext Dbcontext { get; }

        public Doctor AddDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);

            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctors;
        }

        public List<Doctor> SearchDoctorBySpecialisation(
            Doctor.SpecialisationOption specialisation)
        {
            List<Doctor> filteredDoctors =
                new List<Doctor>();

            foreach (var doctor in _doctors)
            {
                if (doctor.Specialisation == specialisation)
                {
                    filteredDoctors.Add(doctor);
                }
            }

            return filteredDoctors;
        }
    }
}