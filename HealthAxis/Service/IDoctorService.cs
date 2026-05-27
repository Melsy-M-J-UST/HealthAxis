using System;
using System.Collections.Generic;
using System.Text;
using HealthAxis.Models;

namespace HealthAxis.Services
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);
        Doctor? GetById(int doctorId);
        List<Doctor> GetAllDoctors();
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
        bool UpdateDoctor(Doctor doctor);
    }
}
