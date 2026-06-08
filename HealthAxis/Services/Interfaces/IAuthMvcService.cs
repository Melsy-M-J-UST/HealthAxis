using HealthAxis.Shared.DTOs;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IAuthMvcService
    {
        LoginDto Login(LoginDto dto, out string errorMessage);
        bool SignUpPatient(PatientDto dto, out string errorMessage);
        bool SignUpDoctor(DoctorDto dto, out string errorMessage);
    }
}
