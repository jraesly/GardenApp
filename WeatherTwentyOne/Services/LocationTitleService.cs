using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeatherTwentyOne.Interfaces;

namespace WeatherTwentyOne.Services
{
    public class LocationTitleService : INotifyPropertyChanged, ILocationTitleService
    {
        private string _locationTitle;
        public string LocationTitle
        {
            get => _locationTitle;
            set
            {
                if (_locationTitle != value)
                {
                    _locationTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
