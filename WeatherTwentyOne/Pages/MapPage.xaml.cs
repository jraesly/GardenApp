using Microsoft.Maui.Controls;
using WeatherTwentyOne.Models;
using WeatherTwentyOne.ViewModels;

namespace WeatherTwentyOne.Pages
{
    public partial class MapPage : ContentPage
    {
        private OpenWeatherMapModel _weatherData;
        public MapViewModel ViewModel { get; }
        public MapPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("mapPage", typeof(MapPage));

            ViewModel = new MapViewModel(); // Default values for lat and lon
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var url = ViewModel.LoadData();
            WindyWebView.Source = url;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            WindyWebView.Source = null;
        }
    }
}
