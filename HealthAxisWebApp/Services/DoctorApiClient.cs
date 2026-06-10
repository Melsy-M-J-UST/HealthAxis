using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisWebApp.Services
{
    public class DoctorApiClient
    {
        private readonly HttpClient _client;

        public DoctorApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44366/")
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<DoctorDto>> GetDoctors(string sortBy = "name", string specialisation = "all")
        {
            var response = await _client.GetAsync(
                $"api/doctors?sortBy={sortBy}&specialisation={specialisation}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<List<DoctorDto>> GetAllDoctors()
        {
            return await GetDoctors("name", "all");
        }

        public async Task<DoctorDto> GetDoctorById(int id)
        {
            var response = await _client.GetAsync($"api/doctors/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task<DoctorDto> GetDoctorProfile(int id)
        {
            var response = await _client.GetAsync($"api/doctors/{id}/profile");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task CreateDoctor(DoctorDto doctor)
        {
            var json = JsonConvert.SerializeObject(doctor);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("api/doctors", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }
        }

        public async Task UpdateDoctor(DoctorDto doctor)
        {
            var json = JsonConvert.SerializeObject(doctor);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync($"api/doctors/{doctor.DoctorId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }
        }

        public async Task ToggleDoctorStatus(int id)
        {
            var response = await _client.PutAsync($"api/doctors/{id}/toggle", null);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }
        }

        public async Task DeleteDoctor(int id)
        {
            var response = await _client.DeleteAsync($"api/doctors/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }
        }

        private string ExtractErrorMessage(string errorContent)
        {
            if (string.IsNullOrWhiteSpace(errorContent))
            {
                return "An unexpected error occurred.";
            }

            try
            {
                var json = JObject.Parse(errorContent);
                var message = json["Message"]?.ToString();

                if (!string.IsNullOrWhiteSpace(message))
                {
                    if (message.Contains("\r\n"))
                    {
                        message = message.Split(new[] { "\r\n" }, StringSplitOptions.None)[0];
                    }

                    if (message.Contains("Parameter name:"))
                    {
                        message = message.Split(new[] { "Parameter name:" }, StringSplitOptions.None)[0].Trim();
                    }

                    return message.Trim();
                }
            }
            catch
            {
            }

            if (errorContent.Contains("\r\n"))
            {
                errorContent = errorContent.Split(new[] { "\r\n" }, StringSplitOptions.None)[0];
            }

            if (errorContent.Contains("Parameter name:"))
            {
                errorContent = errorContent.Split(new[] { "Parameter name:" }, StringSplitOptions.None)[0].Trim();
            }

            return errorContent.Trim();
        }
    }
}