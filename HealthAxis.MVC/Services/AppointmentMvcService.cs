using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HealthAxis.Mvc.Services
{
    public class AppointmentMvcService : ApiServiceBase, IAppointmentMvcService
    {
        public bool Book(AppointmentDto dto, out string error)
        {
            return Send(
                "appointments",
                dto,
                "POST",
                out error);
        }

        public IEnumerable<AppointmentDto> GetByPatient(int patientId)
        {
            return Get("appointments/patient/" + patientId);
        }

        public IEnumerable<AppointmentDto> GetByDoctor(int doctorId)
        {
            return Get("appointments/doctor/" + doctorId);
        }

        public IEnumerable<AppointmentDto> Today(int doctorId)
        {
            return Get("appointments/doctor/" + doctorId + "/today");
        }

        public IEnumerable<AppointmentDto> Weekly(
            int doctorId,
            DateTime startDate)
        {
            return Get(
                "appointments/doctor/" +
                doctorId +
                "/weekly?startDate=" +
                startDate.ToString("yyyy-MM-dd"));
        }

        public bool UpdateStatus(
            int id,
            AppointmentStatusUpdateDto dto,
            out string error)
        {
            return Send(
                "appointments/" + id + "/status",
                dto,
                "PUT",
                out error);
        }

        private IEnumerable<AppointmentDto> Get(string url)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<AppointmentDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
            }
        }

        private bool Send(
            string url,
            object dto,
            string method,
            out string error)
        {
            error = string.Empty;

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

                error = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }
    }
}