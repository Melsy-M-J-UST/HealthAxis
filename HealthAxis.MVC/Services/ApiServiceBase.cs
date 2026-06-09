using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HealthAxis.Mvc.Services
{
    public abstract class ApiServiceBase
    {
        protected HttpClient CreateClient()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(
                ConfigurationManager.AppSettings["HealthAxisApiBaseUrl"]);

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}