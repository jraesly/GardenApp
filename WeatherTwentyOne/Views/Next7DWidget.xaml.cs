using WeatherTwentyOne.Models;
using WeatherTwentyOne.ViewModels;

namespace WeatherTwentyOne.Views;

public partial class Next7DWidget
{
    public HomeViewModel ViewModel => BindingContext as HomeViewModel;
    public Next7DWidget()
    {
        InitializeComponent();

    }
}
