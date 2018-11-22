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
using Windows.Globalization;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Senior_Project_V1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalendarMonths : Page
    {
        //use new keywoard to hide FrameworkElement.Name
        public string Month { get; set; }
        public string Year { get; set; }
        string monthYear = "";

        public CalendarMonths()
        {
            this.InitializeComponent();
        }

        //function to get parameters passed to class during navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = (CalendarMonths)e.Parameter;

            if (parameters == null)
            {
                return;
            }
            else
            {
                monthYear = parameters.Name;
            }
        }

        //private async void CalendarApp_Loaded(object sender, RoutedEventArgs e)
        private void CalendarMonths_Loaded(object sender, RoutedEventArgs e)
        {
            Calendar cal = new Calendar();
            if (monthYear == "")
            {
                CalYear.Text = cal.Year.ToString();
            }
            else
            {
                CalYear.Text = monthYear;
            }
        }

        //HomeButton_Click
        private void CalendarApp_Click(object sender, RoutedEventArgs e)
        {
            var parameters = new CalendarMonths
            {
                Month = ((Button)sender).Name.ToString(),
                Year = monthYear
            };
            //string something = clicked.Name;
            //JanMonth.Text = parameters.Name;
            //JanMonth.Content = ((Button)sender).Name.ToString();
            this.Frame.Navigate(typeof(CalendarApp), parameters);
        }
    }
}
