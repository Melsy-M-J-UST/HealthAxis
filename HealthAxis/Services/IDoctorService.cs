using HealthAxis.Models;
using System.Collections.Generic;

namespace HealthAxis.Services
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
    }
}