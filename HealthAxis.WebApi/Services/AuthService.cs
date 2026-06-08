using HealthAxis.Api.Data;
using HealthAxis.Api.Helpers;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Numerics;

namespace HealthAxis.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        public AuthService(IUserRepository userRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _userRepository = userRepository; _patientRepository = patientRepository; _doctorRepository = doctorRepository;
        }
        public LoginDto Login(LoginDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null || user.Role != dto.Role || !PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return new LoginDto { IsSuccess = false, Message = "Invalid email, password, or role.", Role = dto.Role, Email = dto.Email };
            return new LoginDto { IsSuccess = true, Message = "Login successful.", UserId = user.UserId, Email = user.Email, Role = user.Role, ReferenceId = user.ReferenceId };
        }
        public bool SignUpPatient(PatientDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_userRepository.GetByEmail(dto.Email) != null) { errorMessage = "Email already exists."; return false; }
            string salt = PasswordHelper.GenerateSalt();
            var user = _userRepository.Add(new User1 { UserId = _userRepository.GenerateNextUserId("Patient"), Email = dto.Email, PasswordHash = PasswordHelper.HashPassword(dto.Password, salt), PasswordSalt = salt, Role = "Patient", CreatedDate = DateTime.Now });
            var patient = _patientRepository.Add(new Patient { FullName = dto.FullName, DateOfBirth = dto.DateOfBirth, Gender = dto.Gender, PhoneNumber = dto.PhoneNumber, Email = dto.Email, InsuranceID = dto.InsuranceID, CreatedDate = DateTime.Now });
            _userRepository.UpdateReferenceId(user.UserId, patient.PatientId);
            return true;
        }
        public bool SignUpDoctor(DoctorDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_userRepository.GetByEmail(dto.Email) != null) { errorMessage = "Email already exists."; return false; }
            string salt = PasswordHelper.GenerateSalt();
            var user = _userRepository.Add(new User1 { UserId = _userRepository.GenerateNextUserId("Doctor"), Email = dto.Email, PasswordHash = PasswordHelper.HashPassword(dto.Password, salt), PasswordSalt = salt, Role = "Doctor", CreatedDate = DateTime.Now });
            var doctor = _doctorRepository.Add(new Doctor { FullName = dto.FullName, Specialisation = dto.Specialisation, YearsOfExperience = dto.YearsOfExperience, ConsultationFee = dto.ConsultationFee, IsActive = true });
            _userRepository.UpdateReferenceId(user.UserId, doctor.DoctorId);
            return true;
        }
    }
}
