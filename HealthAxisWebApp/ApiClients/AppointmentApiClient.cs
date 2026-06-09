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
    public class AppointmentApiClient
    {
        private readonly HttpClient _client;

        public AppointmentApiClient()
        {
            _client = new HttpClient();

            // Web API base URL
            _client.BaseAddress = new Uri("https://localhost:44366/");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            var response = await _client.GetAsync("api/appointments");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to load appointments. " + error);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            var response = await _client.GetAsync("api/appointments/" + id);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to load appointment. " + error);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(json);
        }

        public async Task CreateAppointment(AppointmentDto appointment)
        {
            var json = JsonConvert.SerializeObject(appointment);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("api/appointments", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task UpdateAppointment(AppointmentDto appointment)
        {
            var json = JsonConvert.SerializeObject(appointment);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync(
                "api/appointments/" + appointment.AppointmentId,
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task ConfirmAppointment(int id)
        {
            var response = await _client.PutAsync(
                "api/appointments/" + id + "/confirm",
                null
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task CompleteAppointment(int id)
        {
            var response = await _client.PutAsync(
                "api/appointments/" + id + "/complete",
                null
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task CancelAppointment(int id, string cancellationReason)
        {
            var request = new
            {
                CancellationReason = cancellationReason
            };

            var json = JsonConvert.SerializeObject(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync(
                "api/appointments/" + id + "/cancel",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task DeleteAppointment(int id)
        {
            var response = await _client.DeleteAsync("api/appointments/" + id);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }
}