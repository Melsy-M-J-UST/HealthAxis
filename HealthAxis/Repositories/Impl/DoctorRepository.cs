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

		public Doctor? GetById(int doctorId)
		{
			return _ContextDb.Doctors
				.FirstOrDefault(d => d.DoctorId == doctorId);
		}
		public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
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