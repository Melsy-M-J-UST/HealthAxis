using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HealthAxis.Mvc.Services
{
    public class DoctorMvcService : ApiServiceBase, IDoctorMvcService
    {
        public IEnumerable<DoctorDto> GetAllDoctors()
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("doctors").Result;
                if (!response.IsSuccessStatusCode) return new List<DoctorDto>();
                return JsonConvert.DeserializeObject<IEnumerable<DoctorDto>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public IEnumerable<DoctorDto> GetActiveDoctors()
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("doctors?activeOnly=true").Result;
                if (!response.IsSuccessStatusCode) return new List<DoctorDto>();
                return JsonConvert.DeserializeObject<IEnumerable<DoctorDto>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public IEnumerable<DoctorDto> GetDoctorsBySpecialisation(string specialisation)
        {
            if (string.IsNullOrWhiteSpace(specialisation)) return GetAllDoctors();

            using (var client = CreateClient())
            {
                var response = client.GetAsync("doctors?specialisation=" + specialisation).Result;
                if (!response.IsSuccessStatusCode) return new List<DoctorDto>();
                return JsonConvert.DeserializeObject<IEnumerable<DoctorDto>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public DoctorDto GetDoctorById(int id)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("doctors/" + id).Result;
                if (!response.IsSuccessStatusCode) return null;
                return JsonConvert.DeserializeObject<DoctorDto>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public bool CreateDoctor(DoctorDto dto, out string errorMessage)
        {
            return SendDoctor("doctors", dto, out errorMessage, "POST");
        }

        public bool UpdateDoctor(DoctorDto dto, out string errorMessage)
        {
            return SendDoctor("doctors/" + dto.DoctorId, dto, out errorMessage, "PUT");
        }

        public bool ToggleStatus(int id, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (var client = CreateClient())
            {
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = client.PutAsync("doctors/" + id + "/toggle-status", content).Result;
                if (response.IsSuccessStatusCode) return true;
                errorMessage = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }

        private bool SendDoctor(string url, DoctorDto dto, out string errorMessage, string method)
        {
            errorMessage = string.Empty;
            using (var client = CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = method == "POST"
                    ? client.PostAsync(url, content).Result
                    : client.PutAsync(url, content).Result;

                if (response.IsSuccessStatusCode) return true;
                errorMessage = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }
    }
}
