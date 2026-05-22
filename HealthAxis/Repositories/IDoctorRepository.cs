using HealthAxis.Models;
using System.Collections.Generic;

namespace HealthAxis.Repositories
{
    public interface IDoctorRepository
    {
        string AddDoctor(Doctor doctor);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
        List<Doctor> GetAllDoctors();
    }
}
