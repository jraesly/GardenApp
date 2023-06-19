using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherTwentyOne.Interfaces;
using WeatherTwentyOne.Models;

namespace WeatherTwentyOne.ViewModels;

public class FavoritesViewModel : INotifyPropertyChanged
{
    private ObservableCollection<FavoriteLocation> favorites;
    //private static WeatherUpdateService _weatherUpdateService;
    private string _searchText;
    private readonly INavigation _navigation;
    private readonly IActivityService _activityService;
    private readonly ILocationTitleService _locationTitleService;


    public string SearchText
    {
        get { return _searchText; }
        set
        {
            _searchText = value;
            OnPropertyChanged();

            // Perform search when the SearchText property is set
            SearchCommand.Execute(null);
        }
    }
    public ObservableCollection<FavoriteLocation> Favorites {
        get {
            return favorites;
        }

        set {
            favorites = value;
            OnPropertyChanged();
        }
    }

    public ICommand LocationSelectedCommand { get; }
    public ICommand SearchCommand { get; }
    private readonly IWeatherUpdateService _weatherUpdateService;
    private readonly INavigationService _navigationService;
    private readonly IFavoriteLocationsService _favoriteLocationsService;



    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public FavoritesViewModel(IWeatherUpdateService weatherUpdateService, INavigationService navigationService, IFavoriteLocationsService favoriteLocationsService, ILocationTitleService locationTitleService, IActivityService activityService)
    {
        // Initialize _favoriteLocationsService
        _activityService = activityService;
        _locationTitleService = locationTitleService; 
        _favoriteLocationsService = favoriteLocationsService;
        _navigation = Application.Current.MainPage.Navigation;
        LoadFavoriteLocations();
        LocationSelectedCommand = new Command<FavoriteLocation>(OnLocationSelected);
        SearchCommand = new Command(PerformSearch);
        _weatherUpdateService = weatherUpdateService;
        _navigationService = navigationService;
    }
    private void PerformSearch()
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            // If the search text is null or empty, show all locations
            LoadFavoriteLocations();
        }
        else
        {
            // Filter favorite locations by city and state
            Favorites = new ObservableCollection<FavoriteLocation>(_favoriteLocationsService.GetFavoriteLocations().Where(location =>
                location.City.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                location.State.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }

    private void LoadFavoriteLocations()
    {
        Favorites = new ObservableCollection<FavoriteLocation>(_favoriteLocationsService.GetFavoriteLocations());
    }

    public void RefreshFavoriteLocations()
    {
        LoadFavoriteLocations();
    }

    private async void OnLocationSelected(FavoriteLocation location)
    {
        if (location == null)
        {
            return;
        }
        _activityService.IsLoading = true;

        // Set LocationService.Instance latitude, longitude, city, and state to locations
        LocationService.Instance.Latitude = location.Latitude ;
        LocationService.Instance.Longitude = location.Longitude;
        LocationService.Instance.City = location.City;
        LocationService.Instance.State = location.State;

        await _weatherUpdateService.UpdateWeatherData();

        // Update the location title in the homeViewModel
        _locationTitleService.LocationTitle = $"{location.City}, {location.State}";

        // Navigate to the home page
        await _navigationService.NavigateToHomeAsync();
        _activityService.IsLoading = false;
    }
}
