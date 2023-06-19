using WeatherTwentyOne.Models;

namespace WeatherTwentyOne.Interfaces
{
    public interface IHomePage
    {
        // Define necessary methods for the HomePage
        public string HomePage { get; set; }
        public string LocationTitle { get; set; }
        public string FavoriteImageSource { get; set; }
        public bool IsRefreshing { get; set; }
        public OpenWeatherMapModel WeatherData { get; set; }
        public event Action<OpenWeatherMapModel> WeatherDataUpdated;
        void InitializeWidgets(OpenWeatherMapModel weatherData);

    }

}
