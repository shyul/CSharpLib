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
        public static double StringToNum(string input)
        {
            if (input != null && !input.Equals(""))
                return Double.Parse(input);
            else
                return 0;
        }
        public static string NumToString(double input)
        {
            if (double.IsNaN(input))
            {
                return "No Data";
            }
            else if (input >= 10000 || input <= -10000)
            {
                return input.ToString("N0", CultureInfo.InvariantCulture);
            }
            else if (input > -10 && input < 10)
            {
                input = (double)Round(input * 1000) / 1000;
                return input.ToString("0.00#");
            }
            else
            {
                input = (double)Round(input * 1000) / 1000;
                return input.ToString("0.00");
            }
        }
        public static long Round(double input)
        {
            return (long)Math.Round(input, MidpointRounding.AwayFromZero);
        }
        public static int Round(float input)
        {
            return (int)Math.Round(input, MidpointRounding.AwayFromZero);
        }

        public static DataRange GetRange(double[] input)
        {
            DataRange dr = new DataRange();
            for (int i = 0; i < input.Length; i++)
            {
                dr.Maximum = Math.Max(input[i], dr.Maximum);
                dr.Minimum = Math.Min(input[i], dr.Minimum);
            }
            return dr;
        }

        public static int LoWord(IntPtr dWord)
        {
            return LoWord(dWord.ToInt32());
        }
        public static int HiWord(IntPtr dWord)
        {
            return HiWord(dWord.ToInt32());
        }
        public static int LoWord(int dWord)
        {
            return dWord & 0xffff;
        }
        public static int HiWord(int dWord)
        {
            if ((dWord & 0x80000000) == 0x80000000)
                return (dWord >> 16);
            else
                return (dWord >> 16) & 0xffff;
        }
        public static UInt32 BinaryReverse(UInt32 input, int BitLength)
        {
            UInt32 result = 0;
            for (int i = 0; i < BitLength; i++)
            {
                result = (result << 1) | (input & 1);
                input >>= 1;
            }
            return result;
        }
    }

    public class DataRange
    {
        public double Maximum = double.MinValue;
        public double Minimum = double.MaxValue;

        public DataRange() { }
        public DataRange(double[] input)
        {
            Maximum = double.MinValue;
            Minimum = double.MaxValue;
            for (int i = 0; i < input.Length; i++)
            {
                Maximum = Math.Max(input[i], Maximum);
                Minimum = Math.Min(input[i], Minimum);
            }
        }
        public double GetRange() { return Math.Abs(Maximum - Minimum); }
    }
}
