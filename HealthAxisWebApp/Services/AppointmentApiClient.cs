using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisWebApp.Services
{
    public class AppointmentApiClient
    {
        private const string ApiBaseUrlSettingKey = "HealthAxisApiBaseUrl";
        private const string JsonMediaType = "application/json";
        private const string AppointmentsEndpoint = "api/appointments";

        private const string ConfirmAction = "confirm";
        private const string CompleteAction = "complete";
        private const string CancelAction = "cancel";

        private static readonly HttpClient Client = CreateHttpClient();

        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            HttpResponseMessage response = await Client.GetAsync(AppointmentsEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to load appointments.");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            HttpResponseMessage response = await Client.GetAsync(
                GetAppointmentByIdEndpoint(id));

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to load appointment.");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AppointmentDto>(json);
        }

        public async Task CreateAppointment(AppointmentDto appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            StringContent content = CreateJsonContent(appointment);

            HttpResponseMessage response = await Client.PostAsync(
                AppointmentsEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to create appointment.");
            }
        }

        public async Task UpdateAppointment(AppointmentDto appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            StringContent content = CreateJsonContent(appointment);

            HttpResponseMessage response = await Client.PutAsync(
                GetAppointmentByIdEndpoint(appointment.AppointmentId),
                content);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to update appointment.");
            }
        }

        public async Task ConfirmAppointment(int id)
        {
            HttpResponseMessage response = await Client.PutAsync(
                GetAppointmentActionEndpoint(id, ConfirmAction),
                CreateEmptyJsonContent());

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to confirm appointment.");
            }
        }

        public async Task CompleteAppointment(int id)
        {
            HttpResponseMessage response = await Client.PutAsync(
                GetAppointmentActionEndpoint(id, CompleteAction),
                CreateEmptyJsonContent());

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to complete appointment.");
            }
        }

        public async Task CancelAppointment(int id, string cancellationReason)
        {
            if (string.IsNullOrWhiteSpace(cancellationReason))
            {
                throw new ArgumentException("Cancellation reason is required.");
            }

            var request = new
            {
                CancellationReason = cancellationReason
            };

            StringContent content = CreateJsonContent(request);

            HttpResponseMessage response = await Client.PutAsync(
                GetAppointmentActionEndpoint(id, CancelAction),
                content);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to cancel appointment.");
            }
        }

        public async Task DeleteAppointment(int id)
        {
            HttpResponseMessage response = await Client.DeleteAsync(
                GetAppointmentByIdEndpoint(id));

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(response, "Failed to delete appointment.");
            }
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId)
        {
            HttpResponseMessage response = await Client.GetAsync(
                $"{AppointmentsEndpoint}/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(
                    response,
                    "Failed to load patient appointments.");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetTodayAppointments(int doctorId)
        {
            HttpResponseMessage response = await Client.GetAsync(
                $"{AppointmentsEndpoint}/doctor/{doctorId}/today");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(
                    response,
                    "Failed to load today's schedule.");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetWeeklyAppointments(int doctorId)
        {
            HttpResponseMessage response = await Client.GetAsync(
                $"{AppointmentsEndpoint}/doctor/{doctorId}/week");

            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpRequestException(
                    response,
                    "Failed to load weekly schedule.");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        private static HttpClient CreateHttpClient()
        {
            string apiBaseUrl = ConfigurationManager.AppSettings[ApiBaseUrlSettingKey];

            if (string.IsNullOrWhiteSpace(apiBaseUrl))
            {
                throw new InvalidOperationException("API base URL missing.");
            }

            var client = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(JsonMediaType));

            return client;
        }

        private static string GetAppointmentByIdEndpoint(int id)
        {
            return $"{AppointmentsEndpoint}/{id}";
        }

        private static string GetAppointmentActionEndpoint(int id, string action)
        {
            return $"{AppointmentsEndpoint}/{id}/{action}";
        }

        private static StringContent CreateJsonContent(object value)
        {
            string json = JsonConvert.SerializeObject(value);

            return new StringContent(json, Encoding.UTF8, JsonMediaType);
        }

        private static StringContent CreateEmptyJsonContent()
        {
            return new StringContent(string.Empty, Encoding.UTF8, JsonMediaType);
        }

        private static async Task ThrowHttpRequestException(
            HttpResponseMessage response,
            string message)
        {
            string error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                $"{message} Status Code: {response.StatusCode}. Details: {error}");
        }
    }
}