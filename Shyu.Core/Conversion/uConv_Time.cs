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
        Years = 31536000, // 365 days
        Months = 2419200, // 28 days
        Weeks = 604800,
        Days = 86400,
        Hours = 3600,
        Minutes = 60,
        Seconds = 1,
    }
    public class Period
    {
        public int Length = 1;
        public DateTimeIntervalType IntervalType = DateTimeIntervalType.Days;

        public Period(int Length, DateTimeIntervalType IntervalType)
        {
            this.Length = Length;
            this.IntervalType = IntervalType;
        }

        public long GetIntervalEIDLength()
        {
            return (long)IntervalType;
        }
        public long ToEIDLength()
        {
            return GetIntervalEIDLength() * Length;
        }
        public override string ToString()
        {
            if (Length == 1)
            {
                switch (IntervalType)
                {
                    case (DateTimeIntervalType.Seconds):
                        return "1 Second";
                    case (DateTimeIntervalType.Minutes):
                        return "1 Minute";
                    case (DateTimeIntervalType.Hours):
                        return "Hourly";
                    case (DateTimeIntervalType.Days):
                        return "Daily";
                    case (DateTimeIntervalType.Weeks):
                        return "Weekly";
                    case (DateTimeIntervalType.Months):
                        return "Monthly";
                    case (DateTimeIntervalType.Years):
                        return "Annually";
                    default:
                        throw new System.NotImplementedException();
                }
            }
            else if (Length > 1)
            {
                switch (IntervalType)
                {
                    case (DateTimeIntervalType.Seconds):
                        return Length.ToString() + " Seconds";
                    case (DateTimeIntervalType.Minutes):
                        return Length.ToString() + " Minutes";
                    case (DateTimeIntervalType.Hours):
                        return Length.ToString() + " Hours";
                    case (DateTimeIntervalType.Days):
                        return Length.ToString() + " Days";
                    case (DateTimeIntervalType.Weeks):
                        return Length.ToString() + " Weeks";
                    case (DateTimeIntervalType.Months):
                        return Length.ToString() + " Months";
                    case (DateTimeIntervalType.Years):
                        return Length.ToString() + " Years";
                    default:
                        throw new System.NotImplementedException();
                }
            }
            else
                throw new System.NotImplementedException();
        }
    }
}


