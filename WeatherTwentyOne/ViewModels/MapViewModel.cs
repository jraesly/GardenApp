using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class MapViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private double _latitude;
    public double Latitude
    {
        get => _latitude;
        set
        {
            _latitude = value;
            OnPropertyChanged();
        }
    }

    private double _longitude;
    public double Longitude
    {
        get => _longitude;
        set
        {
            _longitude = value;
            OnPropertyChanged();
        }
    }


    public MapViewModel()
    {
        _latitude = LocationService.Instance.Latitude;
        _longitude = LocationService.Instance.Longitude;
    }

    public string LoadData()
    {
        // Load your map data here, e.g., call a REST service
        return $"https://embed.windy.com/embed2.html?lat={_latitude}&lon={_longitude}&detailLat={_latitude}&detailLon={_longitude}&width=650&height=450&zoom=10&level=surface&overlay=wind&product=ecmwf&menu=&message=true&marker=&calendar=24&pressure=&type=map&location=coordinates&detail=&metricWind=mph&metricTemp=%C2%B0F&radarRange=-1";
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
