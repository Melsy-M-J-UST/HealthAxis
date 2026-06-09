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
        public IEnumerable<DoctorDto> GetAll(
            string specialisation = null,
            bool activeOnly = false)
        {
            using (var client = CreateClient())
            {
                string url =
                    "doctors?activeOnly=" +
                    activeOnly.ToString().ToLower() +
                    (string.IsNullOrWhiteSpace(specialisation)
                        ? ""
                        : "&specialisation=" + specialisation);

                var response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<DoctorDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<DoctorDto>>(json);
            }
        }

        public DoctorDto GetById(int id)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("doctors/" + id).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<DoctorDto>(json);
            }
        }

        public bool Create(DoctorDto dto, out string error)
        {
            return Send(
                "doctors",
                dto,
                "POST",
                out error);
        }

        public bool Update(DoctorDto dto, out string error)
        {
            return Send(
                "doctors/" + dto.DoctorId,
                dto,
                "PUT",
                out error);
        }

        public bool ToggleStatus(int id, out string error)
        {
            error = string.Empty;

            using (var client = CreateClient())
            {
                var content = new StringContent(
                    "{}",
                    Encoding.UTF8,
                    "application/json");

                var response = client.PutAsync(
                    "doctors/" + id + "/toggle-status",
                    content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                error = response.Content.ReadAsStringAsync().Result;
                return false;
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