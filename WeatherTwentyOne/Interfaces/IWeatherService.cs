using WeatherClient2021;

namespace WeatherTwentyOne.Interfaces;

public interface IWeatherService
{
    Task<IEnumerable<Location>> GetLocations(string query);
    Task<WeatherResponse> GetWeather(Coordinate location);
}
