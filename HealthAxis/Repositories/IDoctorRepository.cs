using HealthAxis.Models;
using System.Collections.Generic;

namespace HealthAxis.Repositories
{
    public interface IDoctorRepository
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
        List<Doctor> GetAllDoctors();
        bool UpdateDoctor(Doctor doctor);
        Doctor? GetById(int id);
    }
}
