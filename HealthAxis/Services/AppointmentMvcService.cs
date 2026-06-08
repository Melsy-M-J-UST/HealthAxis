using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HealthAxis.Mvc.Services
{
    public class AppointmentMvcService : ApiServiceBase, IAppointmentMvcService
    {
        public bool Book(AppointmentDto dto, out string errorMessage)
        {
            return SendAppointment("appointments", dto, out errorMessage, "POST");
        }

        public IEnumerable<AppointmentDto> GetPatientAppointments(int patientId)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("appointments/patient/" + patientId).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<AppointmentDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
            }
        }

        public IEnumerable<AppointmentDto> GetDoctorAppointments(int doctorId)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("appointments/doctor/" + doctorId).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<AppointmentDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
            }
        }

        public bool UpdateStatus(AppointmentDto dto, out string errorMessage)
        {
            return SendAppointment(
                "appointments/" + dto.AppointmentId + "/status",
                dto,
                out errorMessage,
                "PUT");
        }

        public bool DeleteAppointment(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            using (var client = CreateClient())
            {
                var response = client.DeleteAsync("appointments/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                errorMessage = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }

        private bool SendAppointment(
            string url,
            AppointmentDto dto,
            out string errorMessage,
            string method)
        {
            errorMessage = string.Empty;

            using (var client = CreateClient())
            {
                string json = JsonConvert.SerializeObject(dto);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response;

                if (method == "POST")
                {
                    response = client.PostAsync(url, content).Result;
                }
                else
                {
                    response = client.PutAsync(url, content).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                errorMessage = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }
    }
}