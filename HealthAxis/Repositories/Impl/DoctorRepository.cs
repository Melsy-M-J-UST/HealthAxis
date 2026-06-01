using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Data;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly Database _ContextDb;

        public DoctorRepository(Database contextDb)
        {
            _ContextDb = contextDb;
        }

        public Doctor AddDoctor(Doctor doctor)
        {
            _ContextDb.Doctors.Add(doctor);
            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _ContextDb.Doctors;
        }

		public Doctor? GetById(int id)
		{
			return _ContextDb.Doctors
				.FirstOrDefault(d => d.DoctorId == id);
		}
        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            var doctors = _ContextDb.Doctors
                .Where(doc => doc.Specialisation == specialisation)
                .ToList();

            if (doctors == null || doctors.Count == 0)
            {
                throw new Exception("No doctors found with the given specialization.");
            }

            return doctors;
        }

        public bool UpdateDoctor(Doctor doctor)
        {
            var existing = _ContextDb.Doctors.FirstOrDefault(d => d.DoctorId == doctor.DoctorId);
            if (existing == null)
            {
                return false;
            }
            existing.FullName = doctor.FullName;
            existing.Specialisation = doctor.Specialisation;
            existing.YearsOfExperience = doctor.YearsOfExperience;
            existing.ConsultationFee = doctor.ConsultationFee;
            existing.IsActive = doctor.IsActive;
            return true;
        }
    }
}