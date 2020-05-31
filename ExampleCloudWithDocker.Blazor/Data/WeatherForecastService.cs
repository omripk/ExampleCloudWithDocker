using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExampleCloudWithDocker.Blazor.Data
{
    public class WeatherForecastService
    {
        private readonly IConfiguration _configuration;

        public WeatherForecastService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
            try
            {
                var apiUrl = _configuration.GetValue<string>("ApiUrl");

                var client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("weatherforecast");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherForecast[]>(content);
                }
            }
            catch
            {
                // ignore
            }

            return null;
        }
    }
}
