using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HealthAxis.Mvc.Services
{
    public class PatientMvcService : ApiServiceBase, IPatientMvcService
    {
        public IEnumerable<PatientDto> GetAll(string insuranceStatus = null)
        {
            using (var client = CreateClient())
            {
                string url = string.IsNullOrWhiteSpace(insuranceStatus)
                    ? "patients"
                    : "patients?insuranceStatus=" + insuranceStatus;

                var response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<PatientDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<PatientDto>>(json);
            }
        }

        public PatientDto GetById(int id)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("patients/" + id).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<PatientDto>(json);
            }
        }

        public bool Create(PatientDto dto, out string error)
        {
            return Send(
                "patients",
                dto,
                "POST",
                out error);
        }

        public bool Update(PatientDto dto, out string error)
        {
            return Send(
                "patients/" + dto.PatientId,
                dto,
                "PUT",
                out error);
        }

        public bool Delete(int id, out string error)
        {
            error = string.Empty;

            using (var client = CreateClient())
            {
                var response = client.DeleteAsync("patients/" + id).Result;

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