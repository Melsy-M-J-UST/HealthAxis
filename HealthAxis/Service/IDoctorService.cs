using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor? GetDoctorById(int id);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation);
    }
}
