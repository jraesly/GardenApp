using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeatherTwentyOne.Helpers;
using WeatherTwentyOne.Models;
using WeatherTwentyOne.Pages;
using WeatherTwentyOne.Views;
using Microsoft.Maui.Dispatching;
using System.Windows.Input;
using System.Diagnostics;
using Microsoft.Maui.Devices.Sensors;
using WeatherTwentyOne.Interfaces;

namespace WeatherTwentyOne.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ILocationTitleService _locationTitleService;
        private readonly IPromptService _promptService;
        private readonly IFavoriteLocationsService _favoriteLocationsService;


        private static WeatherUpdateService _weatherUpdateService;
        public event Action<OpenWeatherMapModel> WeatherDataUpdated;

        public static WeatherUpdateService WeatherUpdateServiceInstance
        {
            get => _weatherUpdateService;
            set => _weatherUpdateService ??= value;
        }
        private string _favoriteImageSource;
        public string FavoriteImageSource
        {
            get => _favoriteImageSource;
            set => SetProperty(ref _favoriteImageSource, value);
        }
        private readonly IActivityService _activityService;

        public event PropertyChangedEventHandler PropertyChanged;

        private OpenWeatherMapModel _weatherData;
        public OpenWeatherMapModel WeatherData
        {
            get => _weatherData;
            set => SetProperty(ref _weatherData, value);
        }

        private string _locationTitle = "";
        public string LocationTitle
        {
            get => _locationTitle;
            set => SetProperty(ref _locationTitle, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool IsLoading => _activityService.IsLoading;


        public ICommand QuitCommand { get; }
        public ICommand AddLocationCommand { get; }
        public ICommand ChangeLocationCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ToggleModeCommand { get; }
        public ICommand FavoritesCommand { get; }

        public HomeViewModel(IPromptService promptService, ILocationTitleService locationTitleService, IFavoriteLocationsService favoriteLocationsService,
    WeatherUpdateService weatherUpdateService, IActivityService activityService)
        {
            _promptService = promptService;
            _locationTitleService = locationTitleService;
            _favoriteLocationsService = favoriteLocationsService;
            WeatherUpdateServiceInstance = weatherUpdateService;
            _activityService = activityService;

            // Setup event handlers
            _locationTitleService.PropertyChanged += LocationTitleService_PropertyChanged;
            QuitCommand = new Command(Application.Current.Quit);
            ChangeLocationCommand = new Command(ChangeLocation);
            RefreshCommand = new Command(async () => await RefreshWeatherData());
            ToggleModeCommand = new Command(ToggleMode);
            FavoritesCommand = new Command(Favorites);
        }

        private void LocationTitleService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LocationTitle")
            {
                LocationTitle = _locationTitleService.LocationTitle;
            }
        }

        private void Favorites()
        {
            var favLocation = new FavoriteLocation
            {
                Latitude = LocationService.Instance.Latitude,
                Longitude = LocationService.Instance.Longitude,
                City = LocationService.Instance.City,
                State = LocationService.Instance.State
            };
            var favlocService = new FavoriteLocationsService();
            if (favlocService.LocationExists(favLocation))
            {
                favlocService.RemoveFavoriteLocation(favLocation);
            }
            else
            {
                favlocService.AddFavoriteLocation(favLocation);
            }
            IsLocationAFavorite(favlocService, favLocation);
        }
        private async void ChangeLocation()
        {
            // change primary location
            string cityState = await _promptService.DisplayPromptAsync("Enter City, State", "Please enter the City, State name:");
            // Check if the returned string is null or empty
            if (string.IsNullOrEmpty(cityState))
            {
                // The Cancel button was clicked, handle it here.
                Debug.WriteLine("The Cancel button was clicked.");
                return; // Stop execution if needed.
            }
            var newLocation = await LocationHelper.GetLocationFromUserInputAsync(cityState);
            LocationService.Instance.SetLocation(newLocation.latitude, newLocation.longitude, newLocation.city, newLocation.state);
            // Update the LocationTitle property
            _locationTitleService.LocationTitle = $"{newLocation.city}, {newLocation.state}";
            // check and update if the location is a favorite
            var favLocation = new FavoriteLocation
            {
                Latitude = LocationService.Instance.Latitude,
                Longitude = LocationService.Instance.Longitude,
                City = LocationService.Instance.City,
                State = LocationService.Instance.State
            };
            var favlocService = new FavoriteLocationsService();
            IsLocationAFavorite(favlocService, favLocation);

            await RefreshWeatherData();
        }

        private void ToggleMode()
        {
            App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
        }
        public async Task InitializeAsync()
        {
            var favlocService = _favoriteLocationsService ?? new FavoriteLocationsService();
            await LocationHelper.GetLocationAsync();
            LocationTitle = $"{LocationService.Instance.City}, {LocationService.Instance.State}";
            _weatherUpdateService = new WeatherUpdateService();
            _weatherUpdateService.WeatherDataUpdated += OnWeatherDataUpdated;
            _weatherUpdateService.Start();
            IsLocationAFavorite(favlocService);
            _locationTitleService.LocationTitle = $"{LocationService.Instance.City}, {LocationService.Instance.State}";
        }

        public void IsLocationAFavorite(IFavoriteLocationsService favlocService, FavoriteLocation favoriteLocation = null)
        {
            favoriteLocation ??= new FavoriteLocation
                {
                    Latitude = LocationService.Instance.Latitude,
                    Longitude = LocationService.Instance.Longitude,
                    City = LocationService.Instance.City,
                    State = LocationService.Instance.State
                };

            if (favlocService.LocationExists(favoriteLocation))
            {
                FavoriteImageSource = "heart.png";
            }
            else
            {
                FavoriteImageSource = "unheart.png";
            }
        }

        private void OnWeatherDataUpdated(object sender, OpenWeatherMapModel weatherData)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                WeatherData = weatherData;
            });
            WeatherDataUpdated?.Invoke(weatherData);
        }

        private async Task RefreshWeatherData()
        {
            try
            {
                _activityService.IsLoading = true;
                // Add IsRefreshing = true and task.run
                IsRefreshing = true;
                await Task.Run(async () =>
                {
                    await _weatherUpdateService.UpdateWeatherData();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating weather data: {ex.Message}");
            }
            finally
            {
                _activityService.IsLoading = false;
                IsRefreshing = false;
            }
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
