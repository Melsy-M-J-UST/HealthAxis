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
        public IEnumerable<HealthRecordDto> GetPatientHistory(int patientId)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("health-records/patient/" + patientId).Result;
                if (!response.IsSuccessStatusCode) return new List<HealthRecordDto>();
                return JsonConvert.DeserializeObject<IEnumerable<HealthRecordDto>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public bool Create(HealthRecordDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (var client = CreateClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = client.PostAsync("health-records", content).Result;
                if (response.IsSuccessStatusCode) return true;
                errorMessage = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }
    }
}
