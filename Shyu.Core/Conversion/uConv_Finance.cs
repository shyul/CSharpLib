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
        public static double MoneyToNum(string input)
        {
            double num = 0;
            try { num = Convert.ToDouble(Regex.Replace(input, @"\s*[^0-9.]", "")); }
            catch { num = 0; }
            string numSuffix = input.Substring(input.Length - 1, 1).ToUpper();
            switch (numSuffix)
            {
                case ("Q"): num = num * 1e15; break;
                case ("T"): num = num * 1e12; break;
                case ("B"): num = num * 1e9; break;
                case ("M"): num = num * 1e6; break;
                case ("K"): num = num * 1e3; break;
            }
            return num;
        }
        public static string NumToMoney(double input)
        {
            return NumToMoney(input, 1000);
        }
        public static string NumToMoney(double input, double Limit)
        {
            string[] numSuffixText = new string[] { "", "", "K", "M", "B", "T", "Q" };
            double[] numSuffixBase = new double[] { 1, 1e3, 1e6, 1e9, 1e12, 1e15 };
            int TempSuffixIndex = 0;
            double num = input;
            for (TempSuffixIndex = 0; TempSuffixIndex < numSuffixBase.Length; TempSuffixIndex++)
            {
                if (num < Limit) break;
                num = input / numSuffixBase[TempSuffixIndex];
            }
            num = (double)Round(num * 1000) / 1000;

            if (num < 10 && num > -10)
            {
                if (TempSuffixIndex > 0)
                    return num.ToString("0.###") + numSuffixText[TempSuffixIndex];
                else
                    return num.ToString("0.###");
            }
            else
            {
                if (TempSuffixIndex > 0)
                    return num.ToString("0.##") + numSuffixText[TempSuffixIndex];
                else
                    return num.ToString("0.##");
            }
        }

        public static bool IsLastDayOfMonth(long EID)
        {
            return IsLastDayOfMonth(EIDToTime(EID));
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
            return IsMonday(EIDToTime(EID));
        }
        public static bool IsMonday(DateTime Date)
        {
            if (Date.DayOfWeek == DayOfWeek.Monday) return true;
            else return false;
        }
        public static bool IsTradingDate(long EID)
        {
            return IsTradingDate(EIDToTime(EID));
        }
        public static bool IsTradingDate(DateTime Date)
        {
            if ((Date.DayOfWeek == DayOfWeek.Sunday) || (Date.DayOfWeek == DayOfWeek.Saturday)) return false;
            else if ((Date.Month == 1) && (Date.Day == 1)) return false;
            else if ((Date.Month == 7) && (Date.Day == 4)) return false;
            else if ((Date.Month == 12) && (Date.Day == 25)) return false;
            else return true;
        }
        public static bool IsTradingTime(DateTime Time)
        {
            return IsTradingTime(Time, true);
        }
        public static bool IsTradingTime(long EID, bool IsRTH)
        {
            return IsTradingTime(EIDToTime(EID), IsRTH);
        }
        public static bool IsTradingTime(DateTime Time, bool IsRTH)
        {
            if ((Time.DayOfWeek == DayOfWeek.Sunday) || (Time.DayOfWeek == DayOfWeek.Saturday)) return false;
            else if ((Time.Month == 1) && (Time.Day == 1)) return false;
            else if ((Time.Month == 7) && (Time.Day == 4)) return false;
            else if ((Time.Month == 12) && (Time.Day == 25)) return false;
            else
            {
                if (IsRTH)
                {
                    if (Time.Hour >= 10 && Time.Hour <= 15) return true;
                    else if (Time.Hour == 9 && Time.Minute >= 30) return true;
                    else return false;
                }
                else
                {
                    if (Time.Hour >= 4 && Time.Hour <= 20) return true;
                    else return false;
                }
            }
        }
    }
}
