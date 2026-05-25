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
        Doctor? GetDoctorById(int doctorId);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation);
        bool UpdateDoctor(Doctor doctor);
    }
}
