using HealthAxis.Shared.DTOs;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IAuthService
    {
        LoginDto Login(LoginDto dto);
        bool SignUpPatient(PatientDto dto, out string errorMessage);
        bool SignUpDoctor(DoctorDto dto, out string errorMessage);
    }
}
