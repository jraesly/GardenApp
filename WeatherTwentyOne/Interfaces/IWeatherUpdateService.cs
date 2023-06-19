using WeatherTwentyOne.Models;

namespace WeatherTwentyOne.Interfaces
{
    public interface IWeatherUpdateService
    {
        // Define necessary methods for the WeatherUpdateService.cs
        event EventHandler<OpenWeatherMapModel> WeatherDataUpdated;
        Task UpdateWeatherData();
        void Start();
        void Stop();


    }
}