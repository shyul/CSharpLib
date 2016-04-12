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
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        public static long TimeToEID(DateTime date, DateTimeKind TimeKind)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, TimeKind);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        public static DateTime EIDToTime(long EID)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
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
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }
        public static bool IsFirstDayOfYear(long EID)
        {
            return IsFirstDayOfYear(uConv.EIDToTime(EID));
        }
        public static bool IsFirstDayOfYear(DateTime Date)
        {
            if (Date.Month == 1 && Date.Day == 1)
                return true;
            else return false;
        }
        public static bool IsLastDayOfYear(long EID)
        {
            return IsLastDayOfYear(uConv.EIDToTime(EID));
        }
        public static bool IsLastDayOfYear(DateTime Date)
        {
            if (Date.Month == 12 && Date.Day == 31)
                return true;
            else return false;
        }
        public static bool IsLastDayOfMonth(long EID)
        {
            return IsLastDayOfMonth(uConv.EIDToTime(EID));
        }
        public static bool IsLastDayOfMonth(DateTime Date)
        {
            if (((Date.Month == 1 || Date.Month == 3 || Date.Month == 5 || Date.Month == 7 || Date.Month == 8 || Date.Month == 10 || Date.Month == 12) && Date.Day == 31) ||
                ((Date.Month == 4 || Date.Month == 6 || Date.Month == 9 || Date.Month == 11) && Date.Day == 30) ||
                (!DateTime.IsLeapYear(Date.Year) && Date.Month == 2 && Date.Day == 28) ||
                (DateTime.IsLeapYear(Date.Year) && Date.Month == 2 && Date.Day == 29))
                return true;
            else return false;
        }
        public static bool IsMonday(long EID)
        {
            return IsMonday(uConv.EIDToTime(EID));
        }
        public static bool IsMonday(DateTime Date)
        {
            if (Date.DayOfWeek == DayOfWeek.Monday) return true;
            else return false;
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
    public class DateTimePeriodTicks
    {
        public DateTimeIntervalType Major = DateTimeIntervalType.Months;
        public int MajorInterval = 1;
        public DateTimeIntervalType Minor = DateTimeIntervalType.Weeks;
        public int MinorInterval = 1;
    }
    public class DateTimePeriod
    {
        public int Interval = 1;
        public DateTimeIntervalType IntervalType = DateTimeIntervalType.Days;

        public DateTimePeriod(int Interval, DateTimeIntervalType IntervalType)
        {
            this.Interval = Interval;
            this.IntervalType = IntervalType;
        }

        public DateTimePeriodTicks GetTicks(int PointCnt, int MinorRatio, int TicksCnt)
        { 
            DateTimePeriodTicks Ticks = new DateTimePeriodTicks();

            Ticks.MinorInterval = uConv.Round(Interval * PointCnt / TicksCnt);
            if (Ticks.MinorInterval < Interval) Ticks.MinorInterval = Interval;
            Ticks.MajorInterval = MinorRatio * Ticks.MinorInterval;

            switch (IntervalType)
            {
                case (DateTimeIntervalType.Years):
                    break;
                case (DateTimeIntervalType.Months):
                    break;
                default:
                    break;
            }
            return Ticks;
        }
        public long GetIntervalEIDLength()
        {
            return (long)IntervalType;
        }
        public long ToEIDLength()
        {
            return GetIntervalEIDLength() * Interval;
        }
        public override string ToString()
        {
            if (Interval == 1)
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
            else if (Interval == 3 && IntervalType == DateTimeIntervalType.Months)
            {
                return "Quarterly";
            }
            else if (Interval > 1)
            {
                switch (IntervalType)
                {
                    case (DateTimeIntervalType.Seconds):
                        return Interval.ToString() + " Seconds";
                    case (DateTimeIntervalType.Minutes):
                        return Interval.ToString() + " Minutes";
                    case (DateTimeIntervalType.Hours):
                        return Interval.ToString() + " Hours";
                    case (DateTimeIntervalType.Days):
                        return Interval.ToString() + " Days";
                    case (DateTimeIntervalType.Weeks):
                        return Interval.ToString() + " Weeks";
                    case (DateTimeIntervalType.Months):
                        return Interval.ToString() + " Months";
                    case (DateTimeIntervalType.Years):
                        return Interval.ToString() + " Years";
                    default:
                        throw new System.NotImplementedException();
                }
            }
            else
                throw new System.NotImplementedException();
        }
    }
}


