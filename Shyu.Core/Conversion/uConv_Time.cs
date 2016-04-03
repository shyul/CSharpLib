using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data;

namespace Shyu.Core
{
    public static partial class uConv
    {
        public static long TimeToEID(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        public static long TimeToEID(DateTime date, DateTimeKind TimeKind)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, TimeKind);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        public static DateTime EIDToTime(long EID)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(EID);
        }
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }

    public enum DateTimeIntervalType
    {
        Years = 0,
        Months = 1,
        Weeks = 2,
        Days = 3,
        Hours = 4,
        Minutes = 5,
        Seconds = 6,
        Auto = 7,
        NotSet = 8
    }
}


