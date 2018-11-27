using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Senior_Project_V1.Weather;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Senior_Project_V1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherApp : Page
    {
        public WeatherApp()
        {
            this.InitializeComponent();
        }

        private async void WeatherApp_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                //var position = await LocationManager.GetPosition(); //Need. Don't change.    
                //double lat = position.Coordinate.Latitude;
                //double lon = position.Coordinate.Longitude;
                //Console.WriteLine(lat);
                //var textbox = lat.ToString();
                //Debug.Print(textbox);

                //var cli = new System.Net.WebClient();
                //var result = cli.DownloadString("https://api.apixu.com/v1/forecast.json?key=9038eee216524157bfb235747181807&q=31.2143,121.4324&days=7");
                //Console.WriteLine(result);
                //Debug.Print(result);
                //Console.WriteLine("TRY");
                //Debug.Print("TRY");

                //RootObject myWeather = await WeatherClass.GetWeather(position.Coordinate.Latitude, position.Coordinate.Longitude);
                
                var myWeather = await WeatherClass.GetWeather(37.335480, -121.893028);
                Console.WriteLine("KILLMONGO");
                Console.WriteLine(myWeather);

                string icon = String.Format("http:{0}", myWeather.current.condition.icon);
                ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));

                CurrentLocation.Text = myWeather.location.name;
                Console.WriteLine("HELLO");
                Console.WriteLine(CurrentLocation);
                CurrentCondition.Text = myWeather.current.condition.text;
                CurrentTemp.Text = myWeather.current.temp_f.ToString() + "°F";
                Humidity.Text = myWeather.current.humidity.ToString() + "%";
                FeelsLike.Text = myWeather.current.feelslike_f.ToString() + "°F";
                maxtemp.Text = myWeather.forecast.forecastday[0].day.maxtemp_f.ToString() + "°F";
                mintemp.Text = myWeather.forecast.forecastday[0].day.mintemp_f.ToString() + "°F";
                sunrise.Text = myWeather.forecast.forecastday[0].astro.sunrise;
                sunset.Text = myWeather.forecast.forecastday[0].astro.sunset;
            }
            catch
            {
                CurrentLocation.Text = "Unable to get weather at this time";
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WelcomePage));
        }
    }

    
}
