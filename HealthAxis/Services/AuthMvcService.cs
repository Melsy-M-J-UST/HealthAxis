using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HealthAxis.Mvc.Services
{
    public class AuthMvcService : ApiServiceBase, IAuthMvcService
    {
        public LoginDto Login(LoginDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (var client = CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = client.PostAsync("auth/login", content).Result;
                if (!response.IsSuccessStatusCode) { errorMessage = response.Content.ReadAsStringAsync().Result; return null; }
                return JsonConvert.DeserializeObject<LoginDto>(response.Content.ReadAsStringAsync().Result);
            }
        }
        public bool SignUpPatient(PatientDto dto, out string errorMessage) { return SignUp("auth/signup/patient", dto, out errorMessage); }
        public bool SignUpDoctor(DoctorDto dto, out string errorMessage) { return SignUp("auth/signup/doctor", dto, out errorMessage); }
        private bool SignUp(string url, object dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (var client = CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode) return true;
                errorMessage = response.Content.ReadAsStringAsync().Result; return false;
            }
        }
    }
}
