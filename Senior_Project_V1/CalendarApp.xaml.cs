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
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Senior_Project_V1
{
    public sealed partial class CalendarApp : Page
    {
        CalendarAPI calObj = new CalendarAPI();
        CalendarClass calClass = new CalendarClass();
        Calendar cal = new Calendar();
        //class contains results of one token acquisition operation
        AuthenticationResult authResult = null;
        //identifies what the user can do
        string[] scopes = new string[] { "user.read", "user.readbasic.all", "user.readwrite", "calendars.read", "calendars.read.shared", "calendars.readwrite" };
        string setMonth = "";
        string setYear = "";

        public CalendarApp()
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
                setMonth = parameters.Month;
                setYear = parameters.Year;
            }
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
                CreateCalendar();

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

        /// <summary>
        /// creates calendar display with default values, if creating current month's calendary.
        /// uses the 2 parameters when going to previous or next month
        /// </summary>
        /// <param name="dayInt">numerical equivalent of current day</param>
        /// <param name="dayStr">string day of the week</param>
        private void CreateCalendar(int dayInt = -1, string dayStr = "")
        {
            string month = "";
            if (setMonth == "")
            {
                month = cal.MonthAsSoloString();
            }
            else
            {
                Dictionary<string, int> daysDict = new Dictionary<string, int>()
                {
                    {"January",     1},
                    {"February",    2},
                    {"March",       3},
                    {"April",       4},
                    {"May",         5},
                    {"June",        6},
                    {"July",        7},
                    {"August",      8},
                    {"September",   9},
                    {"October",     10},
                    {"November",    11},
                    {"December",    12},
                };
                
                cal.Month = daysDict[setMonth];
                month = setMonth;
            }

            //get previous month's last day
            cal.AddMonths(-1);
            string lastDayofPrevMonth = cal.LastDayInThisMonth.ToString();
            cal.AddMonths(1);

            string lastDayofThisMonth = cal.LastDayInThisMonth.ToString();

            //JanMonth.Text = cal.Month.ToString();

            //TestPrintOut.Text = cal.DayOfWeekAsString(7).ToString();
            string year;
            if (setYear == "")
            {
                year = cal.YearAsString();
            }
            else
            {
                year = setYear;
                cal.Year = Int32.Parse(setYear);
            }

            DisplayMonthYear.Text = month + " " + year;

            int numDay = 0;
            if (dayInt == -1)
            {
                numDay = Int32.Parse(cal.DayAsString());
            }
            else
            {
                numDay = dayInt;
            }

            string strDay = "";
            if (dayStr == "")
            {
                strDay = cal.DayOfWeekAsSoloString();
            }
            else
            {
                strDay = dayStr;
            }

            //get index for first of the month
            int tmp = 0;
            bool greaterThan = false;
            if (strDay == "Sunday")
            {
                if (numDay > 0)
                {
                    greaterThan = true;
                    tmp = numDay - 0;
                }
                else
                {
                    tmp = 0 - (numDay - 1);
                }
            }
            else if (strDay == "Monday")
            {
                if (numDay > 1)
                {
                    greaterThan = true;
                    tmp = numDay - 1;
                }
                else
                {
                    tmp = 1 - (numDay - 1);
                }
            }
            else if (strDay == "Tuesday")
            {
                if (numDay > 2)
                {
                    greaterThan = true;
                    tmp = numDay - 2;
                }
                else
                {
                    tmp = 2 - (numDay - 1);
                }
            }
            else if (strDay == "Wednesday")
            {
                if (numDay > 3)
                {
                    greaterThan = true;
                    tmp = numDay - 3;
                }
                else
                {
                    tmp = 3 - (numDay - 1);
                }
            }
            else if (strDay == "Thursday")
            {
                if (numDay > 4)
                {
                    greaterThan = true;
                    tmp = numDay - 4;
                }
                else
                {
                    tmp = 4 - (numDay - 1);
                }
            }
            else if (strDay == "Friday")
            {
                if (numDay > 5)
                {
                    greaterThan = true;
                    tmp = numDay - 5;
                }
                else
                {
                    tmp = 5 - (numDay - 1);
                }
                    
            }
            else if (strDay == "Saturday")
            {
                if (numDay > 6)
                {
                    greaterThan = true;
                    tmp = numDay - 6;
                }
                else
                {
                    tmp = 6 - (numDay - 1);
                }
            }
            else
            {
                tmp = numDay;
            }

            if (greaterThan)
            {
                tmp = (tmp % 7) - 7;
                tmp = (tmp * (-1) + 1) % 7;
            }

            TextBlock[] textBlkArr = {row0Col0, row0Col1, row0Col2, row0Col3, row0Col4, row0Col5, row0Col6,
                                        row1Col0, row1Col1, row1Col2, row1Col3, row1Col4, row1Col5, row1Col6,
                                        row2Col0, row2Col1, row2Col2, row2Col3, row2Col4, row2Col5, row2Col6,
                                        row3Col0, row3Col1, row3Col2, row3Col3, row3Col4, row3Col5, row3Col6,
                                        row4Col0, row4Col1, row4Col2, row4Col3, row4Col4, row4Col5, row4Col6,
                                        row5Col0, row5Col1, row5Col2, row5Col3, row5Col4, row5Col5, row5Col6};

            UInt16 startingInt = 0;
            //find out which part of the grid month starts on
            if (tmp == 0)
            {
                textBlkArr[0].Text = "1";
                startingInt = 0;
            }
            else if (tmp == 1)
            {
                textBlkArr[1].Text = "1";
                startingInt = 1;
            }
            else if (tmp == 2)
            {
                textBlkArr[2].Text = "1";
                startingInt = 2;
            }
            else if (tmp == 3)
            {
                textBlkArr[3].Text = "1";
                startingInt = 3;
            }
            else if (tmp == 4)
            {
                textBlkArr[4].Text = "1";
                startingInt = 4;
            }
            else if (tmp == 5)
            {
                textBlkArr[5].Text = "1";
                startingInt = 5;
            }
            else if (tmp == 6)
            {
                textBlkArr[6].Text = "1";
                startingInt = 6;
            }
            
            //fill previous month's numerical days and make number look gray
            for (int i = startingInt; i > 0; i--)
            {
                textBlkArr[i - 1].Text = (Int32.Parse(lastDayofPrevMonth) - (startingInt - i)).ToString();
                textBlkArr[i - 1].Foreground = new SolidColorBrush(Colors.Gray);
            }

            //fill rest of current month's days
            int monthLastDay = Int32.Parse(lastDayofThisMonth);
            int upperIndex = monthLastDay + startingInt;
            for (int i = startingInt; i < upperIndex; i++)
            {
                textBlkArr[i].Text = (i - startingInt + 1).ToString();
            }

            //fill remaining spaces with following month's days and make number look gray
            for (int i = upperIndex; i != 42; i++)
            {
                textBlkArr[i].Text = (i - upperIndex + 1).ToString();
                textBlkArr[i].Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        //change every number color to default black color; to be used every time you're switching calendar months
        private void DefaultColor()
        {
            TextBlock[] textBlkArr = {row0Col0, row0Col1, row0Col2, row0Col3, row0Col4, row0Col5, row0Col6,
                                        row1Col0, row1Col1, row1Col2, row1Col3, row1Col4, row1Col5, row1Col6,
                                        row2Col0, row2Col1, row2Col2, row2Col3, row2Col4, row2Col5, row2Col6,
                                        row3Col0, row3Col1, row3Col2, row3Col3, row3Col4, row3Col5, row3Col6,
                                        row4Col0, row4Col1, row4Col2, row4Col3, row4Col4, row4Col5, row4Col6,
                                        row5Col0, row5Col1, row5Col2, row5Col3, row5Col4, row5Col5, row5Col6};

            for (int i = 0; i < 42; ++i)
            {
                textBlkArr[i].Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultColor();
            string firstDay = calClass.getFirstDayString(cal.Day, cal.DayOfWeekAsSoloString(), cal.LastDayInThisMonth);
            cal.AddMonths(1);
            CreateCalendar(1, firstDay);
        }

        private void PrevMonthButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultColor();
            string lastDay = calClass.getLastDayString(cal.Day, cal.DayOfWeekAsSoloString());
            cal.AddMonths(-1);
            CreateCalendar(cal.LastDayInThisMonth, lastDay);
        }

        private void DisplayMonths_Click(object sender, RoutedEventArgs e)
        {
            var parameters = new CalendarMonths
            {
                Name = cal.YearAsString()
            };
            //string something = clicked.Name;
            //JanMonth.Text = parameters.Name;
            //JanMonth.Content = ((Button)sender).Name.ToString();

            this.Frame.Navigate(typeof(CalendarMonths), parameters);
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

        private void HomeButtonClick(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                this.Frame.Navigate(typeof(WelcomePage));
            }
        }
    }
}
