using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;

namespace HAP_Pod4_ConsoleApp_au.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;
        private IDoctorRepository @object;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public DoctorService(AppDbContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            _context = context;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public DoctorService(IDoctorRepository @object)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            this.@object = @object;
        }

        public Doctor AddDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new Exception(
                    "Doctor object cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(
                    doctor.FullName))
            {
                throw new Exception(
                    "Doctor name cannot be empty.");
            }

            bool doctorExists = _context.Doctors
                .Any(d => d.FullName.Equals(doctor.FullName, StringComparison.CurrentCultureIgnoreCase));

            if (doctorExists)
            {
                throw new Exception(
                    "Doctor already exists.");
            }

            doctor.DoctorId =
                _context.GetNextDoctorId();

            _context.Doctors.Add(doctor);

            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors;
        }

        public List<Doctor>
            SearchDoctorBySpecialisation(
                Doctor.SpecialisationOption specialisation)
        {
            return _context.Doctors
                .Where(d =>
                    d.Specialisation ==
                    specialisation)
                .ToList();
        }
    }
}

