using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTwentyOne.Services
{
    public class LocationService : LocationServiceModelBase
    {
        private static LocationService _instance;
        public static LocationService Instance
        {
            get
            {
                _instance ??= new LocationService();
                return _instance;
            }
        }


        public void SetLocation(double latitude, double longitude, string city, string state)
        {
            Latitude = latitude;
            Longitude = longitude;
            City = city;
            State = state;
        }
    }

}
