using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace WeatherTwentyOne.Helpers
{
    public class LocationHelper
    {
        public static async Task GetLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        string city = placemark.Locality;
                        string state = placemark.AdminArea;
                        LocationService.Instance.SetLocation(location.Latitude, location.Longitude, city, state);
                    }
                    else
                    {
                        LocationService.Instance.SetLocation(location.Latitude, location.Longitude, "Unknown", "Unknown");
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        public static async Task SetLocationAsync(double lat, double lon)
        {
            try
            {
                var results = GetCityAndStateFromLocationAsync(lat, lon);


                //LocationService.Instance.SetLocation(lat, location.Longitude, city, state);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        public async static Task<string> GetCityAndStateFromLocationAsync(double latitude, double longitude)
        {
            // Get Bing maps api key essentials.UseMapServiceToken
            string requestUri = $"https://dev.virtualearth.net/REST/v1/Locations/{latitude},{longitude}?key={Constants.BingMapsAPIKey}";

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject responseJson = JObject.Parse(responseContent);
                JToken address = responseJson.SelectToken("$.resourceSets[0].resources[0].address");

                string city = address.Value<string>("locality");
                string state = address.Value<string>("adminDistrict");

                return $"{city}, {state}";
            }

            return null;
        }
        public async static Task<(double latitude, double longitude, string city, string state)> GetLocationFromUserInputAsync(string userInput)
        {
            //initialize requestUri and result
            string requestUri;
            int result;
            string _city;
            string _state;

            // check if userInput can be converted from string to integer
            if (int.TryParse(userInput, out _))
            {
                result = int.Parse(userInput);
                requestUri = $"https://dev.virtualearth.net/REST/v1/Locations/{result}?includeEntityTypes=Address?key={Constants.BingMapsAPIKey}";
            }
            else
            {
                char[] delimiters = { ' ', ',' };
                string[] strings = userInput.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                _city = strings[0];
                _state = strings.Length > 1 ? strings[1] : string.Empty;
                requestUri = $"https://dev.virtualearth.net/REST/v1/Locations?locality={_city}&adminDistrict={_state}?includeEntityTypes=Address&key={Constants.BingMapsAPIKey}";
            }


            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject responseJson = JObject.Parse(responseContent);
                JToken location = responseJson.SelectToken("$.resourceSets[0].resources[0].point.coordinates");
                JToken address = responseJson.SelectToken("$.resourceSets[0].resources[0].address");

                double latitude = location.Value<double>(0);
                double longitude = location.Value<double>(1);
                string city = address.Value<string>("locality");
                string state = address.Value<string>("adminDistrict");

                return (latitude, longitude, city, state);
            }

            return (0, 0, string.Empty, string.Empty);
        }

    }
}
