using WeatherTwentyOne.Models;

namespace WeatherTwentyOne.Interfaces
{
    public interface IFavoriteLocationsService
    {
        // reate interface for FavoriteLocationsService.cs
        List<FavoriteLocation> GetFavoriteLocations();
        void AddFavoriteLocation(FavoriteLocation location);
        void RemoveFavoriteLocation(FavoriteLocation location);
        bool LocationExists(FavoriteLocation locationToCheck);

    }
}