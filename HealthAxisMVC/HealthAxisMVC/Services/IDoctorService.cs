using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisMVC.Services
{
    public interface IDoctorService
    {
        void AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetById(int doctorId);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
        void UpdateDoctor(int id, Doctor doctor);
    }
}
