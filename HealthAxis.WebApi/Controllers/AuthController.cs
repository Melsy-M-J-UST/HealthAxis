using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) { _authService = authService; }
        [HttpPost, Route("login")]
        public IHttpActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = _authService.Login(dto);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPost, Route("signup/patient")]
        public IHttpActionResult SignUpPatient(PatientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool result = _authService.SignUpPatient(dto, out string errorMessage);
            if (!result) return BadRequest(errorMessage);
            return Ok("Patient registered successfully.");
        }
        [HttpPost, Route("signup/doctor")]
        public IHttpActionResult SignUpDoctor(DoctorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool result = _authService.SignUpDoctor(dto, out string errorMessage);
            if (!result) return BadRequest(errorMessage);
            return Ok("Doctor registered successfully.");
        }
    }
}
