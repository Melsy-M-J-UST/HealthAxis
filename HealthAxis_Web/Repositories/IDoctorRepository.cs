using HealthAxis_Web.Models;
using System.Collections.Generic;

namespace HealthAxis_MVC.Repositories
{
    public interface IDoctorRepository
    {
        Doctor AddDoctor(Doctor doctor);
        //List<Doctor> (Doctor.SpecialisationOption specialisation);
        List<Doctor> GetAllDoctors();
        Doctor UpdateDoctor(int id,Doctor doctor);
        Doctor GetById(int id);
    }
}
