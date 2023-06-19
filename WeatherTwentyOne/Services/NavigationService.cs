using WeatherTwentyOne.Interfaces;
using WeatherTwentyOne.Pages;

namespace WeatherTwentyOne.Services
{
    public class NavigationService : INavigationService
    {
        private Shell shell;

        public NavigationService()
        {
            shell = (Shell)Application.Current.MainPage;
        }

        // Define necessary methods for the NavigationService.cs
        public Task NavigateToHomeAsync()
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                return Shell.Current.GoToAsync("//TabHome");
            }
            else
            {
                return Shell.Current.GoToAsync("//FlyoutHome");
            }
        }

        //navigate to favorites page
        public Task NavigateToFavoritesAsync()
        {
            return Shell.Current.GoToAsync("//Favorites");
        }
    }
}
