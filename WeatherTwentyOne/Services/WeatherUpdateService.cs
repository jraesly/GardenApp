using System;
using System.Timers;
using WeatherTwentyOne.Interfaces;
using WeatherTwentyOne.Models;

namespace WeatherTwentyOne.Helpers
{
    public class WeatherUpdateService : IWeatherUpdateService
    {
        private readonly RestService _restService;
        private readonly Timer _timer;
        public event EventHandler<OpenWeatherMapModel> WeatherDataUpdated;

        public WeatherUpdateService(int updateIntervalInSeconds = 30)
        {
            _restService = new RestService();
            _ = UpdateWeatherData();
            _timer = CreateTimer(updateIntervalInSeconds);
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        private Timer CreateTimer(int updateIntervalInSeconds)
        {
            var timer = new Timer(updateIntervalInSeconds * 1000);
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
            return timer;
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await UpdateWeatherData();
        }

        public async Task UpdateWeatherData()
        {
            double userLatitude = LocationService.Instance.Latitude;
            double userLongitude = LocationService.Instance.Longitude;
            if (userLatitude != 0.0 && userLongitude != 0.0)
            {
                OpenWeatherMapModel weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint, userLatitude, userLongitude));
                WeatherDataUpdated?.Invoke(this, weatherData);
            }
        }

        private string GenerateRequestURL(string endPoint, double lat, double lon)
        {
            string requestUri = $"{endPoint}lat={lat}&lon={lon}&APPID={Constants.OpenWeatherMapAPIKey}&units=imperial";
            return requestUri;
        }
    }
}