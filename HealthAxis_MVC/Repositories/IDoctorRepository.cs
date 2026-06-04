using HealthAxis_MVC.Models;
using System.Collections.Generic;

namespace HealthAxis_MVC.Repositories
{
    public interface IDoctorRepository
    {
        void AddDoctor(Doctor doctor);
        //List<Doctor> (Doctor.SpecialisationOption specialisation);
        List<Doctor> GetAllDoctors();
        void UpdateDoctor(int id,Doctor doctor);
        Doctor GetById(int id);
    }
}
