using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository
{
    public interface IDoctorRepository
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor? GetDoctorById(int id);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation);
    }
}
