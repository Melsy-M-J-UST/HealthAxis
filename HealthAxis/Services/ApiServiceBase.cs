using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HealthAxis.Mvc.Services
{
    public abstract class ApiServiceBase
    {
        protected readonly string BaseUrl = "https://localhost:44326/api/";
        protected HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
