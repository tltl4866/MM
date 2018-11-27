using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Senior_Project_V1
{
    public class CalendarClass
    {
        /// <param name="curDay">current day integer</param>
        /// <param name="dayStr">day of the week string</param>
        /// <param name="numDays">total number of days in current month</param>
        /// <returns>first day of the following month as string</returns>
        public string getFirstDayString(int curDay, string dayStr, int numDays)
        {
            Dictionary<string, int> daysDict = new Dictionary<string, int>()
            {
                {"Sunday",      0},
                {"Monday",      1},
                {"Tuesday",     2},
                {"Wednesday",   3},
                {"Thursday",    4},
                {"Friday",      5},
                {"Saturday",    6},
            };

            int temp = (numDays - curDay) % 7;
            int lastIntDay = (daysDict[dayStr] + temp + 1) % 7;

            switch (lastIntDay)
            {
                case 0: return "Sunday";
                case 1: return "Monday";
                case 2: return "Tuesday";
                case 3: return "Wednesday";
                case 4: return "Thursday";
                case 5: return "Friday";
                case 6: return "Saturday";
                default: return "";
            }
        }

        /// <param name="curDay">current day integer</param>
        /// <param name="dayStr">day of the week string</param>
        /// <param name="numDays">total number of days in current month</param>
        /// <returns>last day of previous month as string</returns>
        public string getLastDayString(int curDay, string dayStr)
        {
            Dictionary<string, int> daysDict = new Dictionary<string, int>()
            {
                {"Sunday",      0},
                {"Monday",      1},
                {"Tuesday",     2},
                {"Wednesday",   3},
                {"Thursday",    4},
                {"Friday",      5},
                {"Saturday",    6},
            };

            int temp = ((curDay % 7) - 7) * (-1);
            temp = (temp + daysDict[dayStr]) % 7;

            switch (temp)
            {
                case 0: return "Sunday";
                case 1: return "Monday";
                case 2: return "Tuesday";
                case 3: return "Wednesday";
                case 4: return "Thursday";
                case 5: return "Friday";
                case 6: return "Saturday";
                default: return "";
            }
        }
    }
}