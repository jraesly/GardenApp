using Microsoft.Maui.Devices.Sensors;
using System.Diagnostics;
using WeatherTwentyOne.Helpers;
using WeatherTwentyOne.Interfaces;
using WeatherTwentyOne.Models;
using WeatherTwentyOne.ViewModels;
using WeatherTwentyOne.Views;

namespace WeatherTwentyOne.Pages
{
    public partial class HomePage : ContentPage
    {
        static bool isSetup = false;
        private RestService _restService;
        private WindLiveWidget _windLiveWidget;
        private Next24HrWidget _next24HrWidget;
        private Next7DWidget _next7DWidget;
        private WidgetsPanel _widgetsPanelWidget;
        private static WeatherUpdateService _weatherUpdateService = new WeatherUpdateService();

        public ContentView _next24HrWidgetPlaceholder => this.Next24HrWidgetPlaceholder;
        public ContentView _next7DWidgetPlaceholder => this.Next7DWidgetPlaceholder;
        public ContentView _windLiveWidgetPlaceholder => this.WindLiveWidgetPlaceholder;

        public HomePage()
        {
            InitializeComponent();
            //var vm = new HomeViewModel(this);
            //BindingContext = vm;
            InitializeViewModel();

            // Setup app actions and tray icon if not already done
            if (!isSetup)
            {
                isSetup = true;
                SetupAppActions();
                SetupTrayIcon();
            }
        }

        private void SetupAppActions()
        {
            try
            {
                // Set up app actions
#if WINDOWS
                //AppActions.IconDirectory = Application.Current.On<WindowsConfiguration>().GetImageDirectory();
#endif
                AppActions.SetAsync(
                    new AppAction("current_info", "Check Current Weather", icon: "current_info"),
                    new AppAction("add_location", "Add a Location", icon: "add_location")
                );
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("App Actions not supported", ex);
            }
        }

        private void SetupTrayIcon()
        {
            var trayService = ServiceProvider.GetService<ITrayService>();

            if (trayService != null)
            {
                trayService.Initialize();
                trayService.ClickHandler = () =>
                    ServiceProvider.GetService<INotificationService>()
                        ?.ShowNotification("Hello Build! 😻 From .NET MAUI", "How's your weather?  It's sunny where we are 🌞");
            }
        }
        private async void InitializeViewModel()
        {
            var promptService = new PromptService();
            var locationTitleService = new LocationTitleService();
            var favoriteLocationsService = new FavoriteLocationsService();
            var weatherUpdateService = new WeatherUpdateService();
            var activityService = new ActivityService();
            var viewModel = new HomeViewModel(promptService, locationTitleService, favoriteLocationsService, weatherUpdateService, activityService);
            await viewModel.InitializeAsync();
            BindingContext = viewModel;
            viewModel.WeatherDataUpdated += InitializeWidgets;
            LocationTitleWidgetControl.BindingContext = viewModel;

        }
        private async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            // Get user's location and weather data
            double userLatitude = LocationService.Instance.Latitude;
            double userLongitude = LocationService.Instance.Longitude;
            string userCity = LocationService.Instance.City;
            string userState = LocationService.Instance.State;

            // Update location title in homepage.xaml
            var _locationTitleService = new LocationTitleService();
            _locationTitleService.LocationTitle = $"{userCity}, {userState}";

            // Call REST API to get weather data
            _restService = new RestService();
            OpenWeatherMapModel weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint, userLatitude, userLongitude));

            // Initialize widgets with weather data
            InitializeWidgets(weatherData);
        }

        private string GenerateRequestURL(string endPoint, double lat, double lon)
        {
            // Generate request URL for weather data using latitude and longitude
            string requestUri = endPoint;
            requestUri += $"lat={lat}";
            requestUri += $"&lon={lon}";
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            requestUri += $"&units=imperial";
            return requestUri;
        }

        public void InitializeWidgets(OpenWeatherMapModel weatherData)
        {
            // Dispatch to the UI thread
            this.Dispatcher.Dispatch(() =>
            {
                // Set the binding context to the weather data
                if(BindingContext is HomeViewModel homeViewModel)
                {
                    homeViewModel.WeatherData = weatherData;
                }

                // Initialize the WindLiveWidget
                if (_windLiveWidget == null)
                {
                    _windLiveWidget = new WindLiveWidget();
                    WindLiveWidgetPlaceholder.Content = _windLiveWidget;
                }
                _windLiveWidget.BindingContext = BindingContext;


                // Initialize the Next24HrWidget
                if (_next24HrWidget == null)
                {
                    _next24HrWidget = new Next24HrWidget();
                    Next24HrWidgetPlaceholder.Content = _next24HrWidget;
                }
                _next24HrWidget.BindingContext = BindingContext;


                // Initialize the Next7DWidget
                if (_next7DWidget == null)
                {
                    _next7DWidget = new Next7DWidget();
                    Next7DWidgetPlaceholder.Content = _next7DWidget;
                }
                _next7DWidget.BindingContext = BindingContext;


                // Initialize the WidgetsPanel
                if (_widgetsPanelWidget == null)
                {
                    _widgetsPanelWidget = new WidgetsPanel(weatherData);
                    //WidgetsPanelPlaceholder.Content = _widgetsPanelWidget;
                }
                _widgetsPanelWidget.BindingContext = BindingContext;

            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is HomeViewModel homeViewModel)
            {
                homeViewModel.WeatherDataUpdated -= InitializeWidgets;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // do not call this if this is the first time the app is running
            if (LocationService.Instance.Latitude != 0)
            {
                this.Title = $"{LocationService.Instance.City}, {LocationService.Instance.State}";
            }
        }

    }
}
