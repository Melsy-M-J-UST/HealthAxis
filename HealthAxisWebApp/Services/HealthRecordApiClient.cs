using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisWebApp.Services
{
    public class HealthRecordApiClient
    {
        private readonly HttpClient _client;

        public HealthRecordApiClient()
        {
            _client = new HttpClient();

            // Web API base URL
            _client.BaseAddress = new Uri("https://localhost:44366/");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<List<HealthRecordDto>> GetAllHealthRecords()
        {
            var response = await _client.GetAsync("api/healthrecords");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to load health records. " + error);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<HealthRecordDto> GetHealthRecordById(int id)
        {
            var response = await _client.GetAsync("api/healthrecords/" + id);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to load health record. " + error);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HealthRecordDto>(json);
        }

        public async Task CreateHealthRecord(HealthRecordDto healthRecord)
        {
            var json = JsonConvert.SerializeObject(healthRecord);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("api/healthrecords", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task UpdateHealthRecord(HealthRecordDto healthRecord)
        {
            var json = JsonConvert.SerializeObject(healthRecord);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync(
                "api/healthrecords/" + healthRecord.RecordId,
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task DeleteHealthRecord(int id)
        {
            var response = await _client.DeleteAsync("api/healthrecords/" + id);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }
}