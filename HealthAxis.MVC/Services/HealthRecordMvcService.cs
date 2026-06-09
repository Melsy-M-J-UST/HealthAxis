using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HealthAxis.Mvc.Services
{
    public class HealthRecordMvcService : ApiServiceBase, IHealthRecordMvcService
    {
        public IEnumerable<HealthRecordDto> GetByPatient(int patientId)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("health-records/" + patientId).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<HealthRecordDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<HealthRecordDto>>(json);
            }
        }

        public bool Create(HealthRecordDto dto, out string error)
        {
            return Send(
                "health-records",
                dto,
                out error);
        }

        private bool Send(
            string url,
            object dto,
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

                var response = client.PostAsync(url, content).Result;

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