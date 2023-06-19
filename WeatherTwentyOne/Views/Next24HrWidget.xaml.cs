using WeatherTwentyOne.Models;
using WeatherTwentyOne.ViewModels;

namespace WeatherTwentyOne.Views;

public partial class Next24HrWidget
{
    public HomeViewModel ViewModel => BindingContext as HomeViewModel;
    public Next24HrWidget()
    {
        InitializeComponent();
    }
}
