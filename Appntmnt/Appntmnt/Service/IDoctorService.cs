using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Service
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
