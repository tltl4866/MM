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
using Senior_Project_V1.Helpers;
using Senior_Project_V1.Weather;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Senior_Project_V1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        private WebcamHelper webcam;
        private string visitorName;
        private string backbutton_clicked = "false";

        public WelcomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            visitorName = e.Parameter as string;
        }
        

        private async void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //var welcometext = String.Format("Welcome, {0}!", visitorName);
                string now_time = DateTime.Now.ToString("h:mm:ss tt");

                time.Text = now_time + " ";
                //Debug.Print(time);
                string now_date = DateTime.Now.ToString("M/d/yyyy");
                date.Text = now_date + " ";
                var myWeather = await WeatherClass.GetWeather(37.335480, -121.893028);
                CurrentTemp.Text = myWeather.current.temp_f.ToString() + "°F";

            }
            catch
            {
                var welcometext = String.Format("Welcome, {0}!", visitorName);
                var time = DateTime.Now.ToString("h:mm:ss tt");
                Debug.Print(time);
                Debug.Print("WHATUP");
            }
            //Greetings.Text = welcometext;
        }


        private void weatherapp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WeatherApp));
        }

        private void musicapp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MusicApp));
        }

        private void backbutton_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(MainPage));
        }

        private void calendarApp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CalendarApp));
        }
    }
}