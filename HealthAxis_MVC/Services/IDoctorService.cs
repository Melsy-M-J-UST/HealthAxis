using HealthAxis_MVC.Models;
using System.Collections.Generic;

namespace HealthAxis_MVC.Services
{
    public interface IDoctorService
    {
        void AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetById(int doctorId);
        void UpdateDoctor(int id,Doctor doctor);
        //List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
    }
}