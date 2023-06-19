using System.ComponentModel;

namespace WeatherTwentyOne.Interfaces
{
    public interface ILocationTitleService
    {
        string LocationTitle { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}