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
    public class PatientApiClient
    {
        private readonly HttpClient _client;

        public PatientApiClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44366/")
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<PatientDto>> GetPatients(string sortBy = "name", string filter = "all")
        {
            var url = $"api/patients?sortBy={sortBy}&insuranceFilter={filter}";
            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to load patients.");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatientDto>>(json);
        }

        public async Task<List<PatientDto>> SearchPatients(
            string searchBy,
            string searchValue,
            string sortBy = "name",
            string filter = "all")
        {
            var url =
                $"api/patients?sortBy={sortBy}" +
                $"&insuranceFilter={filter}" +
                $"&searchBy={searchBy}" +
                $"&searchValue={Uri.EscapeDataString(searchValue ?? string.Empty)}";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to search patients.");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatientDto>>(json);
        }

        public async Task<PatientDto> GetPatientById(int id)
        {
            var response = await _client.GetAsync($"api/patients/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to load patient details.");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PatientDto>(json);
        }

        public async Task<PatientDto> GetPatientProfile(int id)
        {
            var response = await _client.GetAsync($"api/patients/{id}/profile");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to load profile.");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PatientDto>(json);
        }

        public async Task<bool> CreatePatient(PatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/patients", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            return true;
        }

        public async Task<bool> UpdatePatient(PatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"api/patients/{patient.PatientId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            return true;
        }

        public async Task<bool> DeletePatient(int id)
        {
            var response = await _client.DeleteAsync($"api/patients/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            return true;
        }

        public async Task<bool> DeactivatePatient(int id)
        {
            var response = await _client.PutAsync($"api/patients/{id}/deactivate", null);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(ExtractErrorMessage(error));
            }

            return true;
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