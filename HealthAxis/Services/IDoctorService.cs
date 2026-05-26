using HealthAxis.Models;
using System.Collections.Generic;

namespace HealthAxis.Services
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor? GetById(int doctorId);
        bool UpdateDoctor(Doctor doctor);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
    }
}