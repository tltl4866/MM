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
//from the api tutorial setup
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Senior_Project_V1
{
    public sealed partial class CalendarApp : Page
    {
        CalendarAPI calObj = new CalendarAPI();
        Calendar cal = new Calendar();
        //class contains results of one token acquisition operation
        AuthenticationResult authResult = null;
        //identifies what the user can do
        string[] scopes = new string[] { "user.read", "user.readbasic.all", "user.readwrite", "calendars.read", "calendars.read.shared", "calendars.readwrite" };

        public CalendarApp()
        {
            this.InitializeComponent();
        }

        private async void CalendarApp_Loaded(object sender, RoutedEventArgs e)
        {
            ResultText.Text = string.Empty;
            TokenInfoText.Text = string.Empty;

            //get all available IAccount in the user token cache for the application
            IEnumerable<IAccount> x = await App.PublicClientApp.GetAccountsAsync();
            //returns first elemet of sequence, or default value, if not element is found
            IAccount acc = x.FirstOrDefault();

            try
            {
                //token authentication and requires user to sign with a pop up login window
                authResult = await App.PublicClientApp.AcquireTokenSilentAsync(scopes, acc);
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await App.PublicClientApp.AcquireTokenAsync(scopes);
                }
                catch (MsalException msalex)
                {
                    ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }
            //authentication was successful
            if (authResult != null)
            {
                //check if calendar is already created by calling getCalendar, else just get calendar
                //if ((await calObj.getCalendar(authResult)).value.Capacity == 0)
                //{
                //await calObj.createCalendar(authResult);
                //}
                //else
                //{
                //await calObj.getCalendar(authResult);
                //}

                //create calendar display
                createCalendar();

                //DisplayBasicTokenInfo(authResult);

                //this.SignOutButton.Visibility = Visibility.Visible;
            }
        }

        /// Display basic information contained in the token
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfoText.Text = "";
            if (authResult != null)
            {
                TokenInfoText.Text += $"Name: {authResult.Account.Username}" + Environment.NewLine;
                //TokenInfoText.Text += $"Username: {authResult.User.DisplayableId}" + Environment.NewLine;
                TokenInfoText.Text += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
                TokenInfoText.Text += $"Access Token: {authResult.AccessToken}" + Environment.NewLine;
            }
        }

        private void createCalendar()
        {
            string lastDayofPrevMonth = "";
            string month = cal.MonthAsSoloString();
            string year = cal.YearAsString();
            DisplayMonth.Text = month;
            DisaplyYear.Text = year;

            //populate day areas
            int numDay = Int32.Parse(cal.DayAsString());
            string strDay = cal.DayOfWeekAsSoloString();
            int tmp = 0;

            //check if leap year
            if (((Int32.Parse(year) % 4) == 0) && (month == "March"))
            {
                lastDayofPrevMonth = "29";
            }
            else
            {
                if ((month == "January") || (month == "February") || (month == "April") ||
                (month == "June") || (month == "August") || (month == "November"))
                {
                    lastDayofPrevMonth = "31";
                }
                else if ((month == "May") || (month == "July") || (month == "October") ||
                        (month == "December"))
                {
                    lastDayofPrevMonth = "30";
                }
                else if (month == "March")
                {
                    lastDayofPrevMonth = "28";
                }
            }

            if (strDay == "Monday")
            {
                tmp = numDay - 1;
            }
            else if (strDay == "Tuesday")
            {
                tmp = numDay - 2;
            }
            else if (strDay == "Wednesday")
            {
                tmp = numDay - 3;
            }
            else if (strDay == "Thursday")
            {
                tmp = numDay - 4;
            }
            else if (strDay == "Friday")
            {
                tmp = numDay - 5;
            }
            else if (strDay == "Saturday")
            {
                tmp = numDay - 6;
            }

            tmp = (tmp % 7) - 7;
            //find out which day month starts on
            string startingRowNCol = "";
            if ((tmp * (-1) + 1) == 0)
            {
                row0Col0.Text = "1";
                startingRowNCol = "row0Col0";
            }
            else if ((tmp * (-1) + 1) == 1)
            {
                row0Col1.Text = "1";
                startingRowNCol = "row0Col1";
                row0Col0.Text = lastDayofPrevMonth;
            }
            else if ((tmp * (-1) + 1) == 2)
            {
                row0Col2.Text = "1";
                startingRowNCol = "row0Col2";
                row0Col1.Text = lastDayofPrevMonth;
                row0Col0.Text = (Int32.Parse(lastDayofPrevMonth) - 1).ToString();
            }
            else if ((tmp * (-1) + 1) == 3)
            {
                row0Col3.Text = "1";
                startingRowNCol = "row0Col3";
                row0Col2.Text = lastDayofPrevMonth;
                row0Col1.Text = (Int32.Parse(lastDayofPrevMonth) - 1).ToString();
                row0Col0.Text = (Int32.Parse(lastDayofPrevMonth) - 2).ToString();
            }
            else if ((tmp * (-1) + 1) == 4)
            {
                row0Col4.Text = "1";
                startingRowNCol = "row0Col4";
                row0Col3.Text = lastDayofPrevMonth;
                row0Col2.Text = (Int32.Parse(lastDayofPrevMonth) - 1).ToString();
                row0Col1.Text = (Int32.Parse(lastDayofPrevMonth) - 2).ToString();
                row0Col0.Text = (Int32.Parse(lastDayofPrevMonth) - 3).ToString();
            }
            else if ((tmp * (-1) + 1) == 5)
            {
                row0Col5.Text = "1";
                startingRowNCol = "row0Col5";
                row0Col4.Text = lastDayofPrevMonth;
                row0Col3.Text = (Int32.Parse(lastDayofPrevMonth) - 1).ToString();
                row0Col2.Text = (Int32.Parse(lastDayofPrevMonth) - 2).ToString();
                row0Col1.Text = (Int32.Parse(lastDayofPrevMonth) - 3).ToString();
                row0Col0.Text = (Int32.Parse(lastDayofPrevMonth) - 4).ToString();
            }
            else if ((tmp * (-1) + 1) == 6)
            {
                row0Col6.Text = "1";
                startingRowNCol = "row0Col6";
                row0Col5.Text = lastDayofPrevMonth;
                row0Col4.Text = (Int32.Parse(lastDayofPrevMonth) - 1).ToString();
                row0Col3.Text = (Int32.Parse(lastDayofPrevMonth) - 2).ToString();
                row0Col2.Text = (Int32.Parse(lastDayofPrevMonth) - 3).ToString();
                row0Col1.Text = (Int32.Parse(lastDayofPrevMonth) - 4).ToString();
                row0Col0.Text = (Int32.Parse(lastDayofPrevMonth) - 5).ToString();
            }

            //create dictionary reference for each row and column value
            Dictionary<string, TextBlock> obj = new Dictionary<string, TextBlock>();

            string curCol = "";
            while (startingRowNCol != "row0Col0")
            {
                curCol = "row0Col" + startingRowNCol[7];
                
            }
        }

        /// allows user to create event by tapping on a date
        private async void CreateEventButton_Click(object sender, RoutedEventArgs e)
        {
            RootObject myEvent = await calObj.createEvent(authResult);
            //EventSubject.Text = myEvent.Subject;
            //EventContent.Text = myEvent.Body.Content;
            //EventStartTime.Text = myEvent.Start.DateTime;
            //EventEndTime.Text = myEvent.End.DateTime;

            //display array of attendees
            for (int i = 0; i < myEvent.Attendees.Capacity; ++i)
            {
                //AttendeeName.Text = myEvent.Attendees[i].EmailAddress.Name;
                //AttendeeEAddress.Text = myEvent.Attendees[i].EmailAddress.Address;
            }
        }

        /// allow user to update an event
        private async void UpdateEventButton_Click(object sender, RoutedEventArgs e)
        {
            RootObject myEvent = await calObj.updateEvent(authResult);
        }

        /// 

        /// Sign out the current user
        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IAccount> x = await App.PublicClientApp.GetAccountsAsync();
            if (x.Any())
            {
                try
                {
                    IEnumerable<IAccount> y = await App.PublicClientApp.GetAccountsAsync();
                    IAccount acc = y.FirstOrDefault();

                    await App.PublicClientApp.RemoveAsync(acc);
                    this.ResultText.Text = "User has signed-out";
                    //this.SignOutButton.Visibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    ResultText.Text = $"Error signing-out user: {ex.Message}";
                }
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WelcomePage));
        }

        // Handles the Click event on the Button inside the Popup control and 
        // closes the Popup. 
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        // Handles the Click event on the Button on the page and opens the Popup. 
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }
    }
}
