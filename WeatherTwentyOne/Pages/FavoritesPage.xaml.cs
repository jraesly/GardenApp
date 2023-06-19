using WeatherTwentyOne.Interfaces;
using WeatherTwentyOne.ViewModels;

namespace WeatherTwentyOne.Pages;

public partial class FavoritesPage : ContentPage
{
    // Add constructor for InavigationService and IWeatherUpdateService
    private readonly INavigationService _navigationService;
    private readonly IWeatherUpdateService _weatherUpdateService;
    private readonly IFavoriteLocationsService _favoriteLocationsService;
    private readonly ILocationTitleService _locationTitleService;
    private readonly IActivityService _activityService;

    public FavoritesPage(IWeatherUpdateService weatherUpdateService, INavigationService navigationService, IFavoriteLocationsService favoriteLocationsService, ILocationTitleService locationTitleService, IActivityService activityService)
    {
        InitializeComponent();
        //Initialize Services
        _navigationService = navigationService;
        _weatherUpdateService = weatherUpdateService;
        _favoriteLocationsService = favoriteLocationsService;
        _locationTitleService = locationTitleService;
        _activityService = activityService;
        //Create viewmodel with dependencies and set binding context
        BindingContext = new FavoritesViewModel(_weatherUpdateService, _navigationService, _favoriteLocationsService, _locationTitleService, _activityService);
        _activityService = activityService;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(300);

        // Refresh the favorite locations
        if (BindingContext is FavoritesViewModel viewModel)
        {
            viewModel.RefreshFavoriteLocations();
        }

        TransitionIn();

    }

    async void TransitionIn()
    {
        foreach (var item in tiles)
        {
            await item.FadeTo(1, 800);
            await Task.Delay(50);
        }
    }

    int tileCount = 0;
    List<Frame> tiles = new List<Frame>();

    void OnHandlerChanged(object sender, EventArgs e)
    {
        Frame f = (Frame)sender;
        tiles.Add(f);
        tileCount++;
    }
}
