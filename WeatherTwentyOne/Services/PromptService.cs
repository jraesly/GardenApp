using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using WeatherTwentyOne.Interfaces;

namespace WeatherTwentyOne.Services
{


    public class PromptService : IPromptService
    {
        public async Task<string> DisplayPromptAsync(string title, string message)
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message);
        }
    }

}
