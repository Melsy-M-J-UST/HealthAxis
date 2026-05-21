using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;

namespace HAP_Pod4_ConsoleApp_au.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
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
                .Any(d =>
                    d.FullName.ToLower() ==
                    doctor.FullName.ToLower());

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