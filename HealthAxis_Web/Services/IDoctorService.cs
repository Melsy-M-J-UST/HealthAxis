using HealthAxis_Web.Models.Dtos;
using System.Collections.Generic;

namespace HealthAxis_MVC.Services
{
    public interface IDoctorService
    {
        DoctorDto AddDoctor(DoctorDto entity);
        List<DoctorDto> GetAllDoctors();
        DoctorDto GetById(int doctorId);
        DoctorDto UpdateDoctor(int id,DoctorDto entity);
        //List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
    }
}