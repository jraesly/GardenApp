using Newtonsoft.Json;
using System.Diagnostics;
using WeatherTwentyOne.Models;

namespace WeatherTwentyOne
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<OpenWeatherMapModel> GetWeatherData(string query) {
            OpenWeatherMapModel weatherData = null;

            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode) { 
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<OpenWeatherMapModel>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return weatherData;
        }
    }
}