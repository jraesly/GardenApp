using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTwentyOne.Interfaces
{
    public interface IPromptService
    {
        Task<string> DisplayPromptAsync(string title, string message);
    }

}
