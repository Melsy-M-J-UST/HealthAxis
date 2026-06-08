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
        public IEnumerable<PatientDto> GetAllPatients(string insuranceStatus = null)
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

        public PatientDto GetPatientById(int id)
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

        public bool CreatePatient(PatientDto dto, out string errorMessage)
        {
            return SendPatient("patients", dto, out errorMessage, "POST");
        }

        public bool UpdatePatient(PatientDto dto, out string errorMessage)
        {
            return SendPatient("patients/" + dto.PatientId, dto, out errorMessage, "PUT");
        }

        public bool DeactivatePatient(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            using (var client = CreateClient())
            {
                var content = new StringContent(
                    "{}",
                    Encoding.UTF8,
                    "application/json");

                var response = client.PutAsync(
                    "patients/" + id + "/deactivate",
                    content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                errorMessage = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }

        private bool SendPatient(
            string url,
            PatientDto dto,
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