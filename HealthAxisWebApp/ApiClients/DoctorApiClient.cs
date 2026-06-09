using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisWebApp.ApiClients
{
    public class DoctorApiClient
    {
        private readonly HttpClient _client;

        public DoctorApiClient()
        {
            _client = new HttpClient();

            // Web API base URL
            _client.BaseAddress = new Uri("https://localhost:44366/");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<DoctorDto>> GetAllDoctors()
        {
            var response = await _client.GetAsync("api/doctors");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to load doctors. " + error);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<DoctorDto> GetDoctorById(int id)
        {
            var response = await _client.GetAsync("api/doctors/" + id);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to load doctor. " + error);
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
                throw new Exception(error);
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

            var response = await _client.PutAsync("api/doctors/" + doctor.DoctorId, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task DeleteDoctor(int id)
        {
            var response = await _client.DeleteAsync("api/doctors/" + id);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }
}