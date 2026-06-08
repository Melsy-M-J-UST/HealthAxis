using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IDoctorMvcService
    {
        IEnumerable<DoctorDto> GetAllDoctors();
        IEnumerable<DoctorDto> GetActiveDoctors();
        IEnumerable<DoctorDto> GetDoctorsBySpecialisation(string specialisation);
        DoctorDto GetDoctorById(int id);
        bool CreateDoctor(DoctorDto dto, out string errorMessage);
        bool UpdateDoctor(DoctorDto dto, out string errorMessage);
        bool ToggleStatus(int id, out string errorMessage);
    }
}
