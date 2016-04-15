using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Shyu.Core
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class PropertyString : Dictionary<string, string>
    {
        public PropertyString() { }
        public PropertyString(string s)
        {
            string[] Sections = Regex.Split(s, "&");
            for (int i = 0; i < Sections.Length; i++)
            {
                string[] Config = Regex.Split(Sections[i], "=");
                if (Config.Length == 2) this.Add(Config[0], Config[1]);
                else this.Add(Config[0], string.Empty);
            }
        }
        public override string ToString()
        {
            string Data = string.Empty;
            for (int i = 0; i < this.Count; i++)
            {
                string Value = this.ElementAt(i).Value;
                string ValueName = this.ElementAt(i).Key;
                Value = CleanUp(Value);
                ValueName = CleanUp(ValueName);
                Data += "&" + ValueName + "=" + Value;
            }
            Data = Data.TrimStart('&');
            return Data;
        }
        public void SetString(string ValueName, string Value)
        {
            ValueName = CleanUp(ValueName);
            Value = Value.Replace("%20", "");
            Value = Value.Replace(" ", "%20");
            Value = CleanUp(Value);
            if (this.ContainsKey(ValueName)) this[ValueName] = Value;
            else this.Add(ValueName, Value);
        }
        public string GetString(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
                return this[ValueName].Replace("%20", " ");
            else
                return string.Empty;
        }

        public void SetBoolean(string ValueName, bool Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public bool GetBoolean(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                if (CleanUp(this[ValueName]).ToLower() == "true")
                    return true;
                else
                    return false;
            }
            else return false;
        }

        public void SetByte(string ValueName, byte Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public int GetByte(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                    return Convert.ToByte(Value);
                else
                    return 0;
            }
            else return 0;
        }

        public void SetInt32(string ValueName, int Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public int GetInt32(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                    return Convert.ToInt32(Value);
                else
                    return 0;
            }
            else return 0;
        }

        public void SetInt64(string ValueName, long Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public long GetInt64(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                    return Convert.ToInt64(Value);
                else
                    return 0;
            }
            else return 0;
        }

        public void SetDouble(string ValueName, double Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public double GetDouble(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                    return Convert.ToDouble(Value);
                else
                    return double.NaN;
            }
            else return double.NaN;
        }

        public void SetDoubleArray(string ValueName, double[] Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = string.Empty;
            if (Value != null)
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    ValueToSet += Value[i].ToString() + ",";
                }
                ValueToSet = ValueToSet.Substring(0, ValueToSet.Length - 1);
            }
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public double[] GetDoubleArray(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                {
                    try
                    {
                        string[] Sections = Regex.Split(Value, ",");
                        if (Sections != null)
                        {
                            List<double> ListDouble = new List<double>();
                            for (int i = 0; i < Sections.Length; i++)
                                if (Sections[i] != string.Empty)
                                    ListDouble.Add(Convert.ToDouble(Sections[i]));
                            return ListDouble.ToArray();
                        }
                        else
                            return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            else return null;
        }

        public void SetFloat(string ValueName, float Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public float GetFloat(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                    return Convert.ToSingle(Value);
                else
                    return 0;
            }
            else return 0;
        }

        public void SetFloatArray(string ValueName, float[] Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = string.Empty;
            if (Value != null)
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    ValueToSet += Value[i].ToString() + ",";
                }
                ValueToSet = ValueToSet.Substring(0, ValueToSet.Length - 1);
            }
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public float[] GetFloatArray(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                {
                    try
                    {
                        string[] Sections = Regex.Split(Value, ",");
                        if (Sections != null)
                        {
                            List<float> ListDouble = new List<float>();
                            for (int i = 0; i < Sections.Length; i++)
                                if (Sections[i] != string.Empty)
                                    ListDouble.Add(Convert.ToSingle(Sections[i]));
                            return ListDouble.ToArray();
                        }
                        else
                            return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            else return null;
        }

        public void SetColor(string ValueName, Color Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = CleanUp(Value.ToArgb().ToString());
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public Color GetColor(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string Value = CleanUp(this[ValueName]);
                if (Value.Length > 0)
                    return Color.FromArgb(Convert.ToInt32(Value));
                else
                    return Color.Transparent;
            }
            else return Color.Transparent;
        }

        public void SetFont(string ValueName, Font Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.Name.ToString().Replace(" ", "%20") + "%30" + Value.Size.ToString() + "%30" + Value.Style.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public Font GetFont(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string[] Sections = Regex.Split(CleanUp(this[ValueName]), "%30");
                Font Fs = new Font(Sections[0].Replace("20%", " "), Convert.ToSingle(Sections[1]));
                switch (Sections[2])
                {
                    case ("Bold"):
                        Fs = new Font(Fs, FontStyle.Bold);
                        break;
                    case ("Italic"):
                        Fs = new Font(Fs, FontStyle.Italic);
                        break;
                    case ("Strikeout"):
                        Fs = new Font(Fs, FontStyle.Strikeout);
                        break;
                    case ("Underline"):
                        Fs = new Font(Fs, FontStyle.Underline);
                        break;
                    default:
                        break;
                }
                return Fs;
            }
            else return new Font("Tahoma", 5f);
        }

        public void SetTimePeriod(string ValueName, DateTimePeriod Value)
        {
            ValueName = CleanUp(ValueName);
            string ValueToSet = Value.Interval.ToString() + "%30" + Value.IntervalType.ToString();
            if (this.ContainsKey(ValueName)) this[ValueName] = ValueToSet;
            else this.Add(ValueName, ValueToSet);
        }
        public DateTimePeriod GetDateTimePeriod(string ValueName)
        {
            ValueName = CleanUp(ValueName);
            if (this.ContainsKey(ValueName))
            {
                string[] Sections = Regex.Split(CleanUp(this[ValueName]), "%30");
                DateTimeIntervalType Fs = DateTimeIntervalType.Days;
                switch (Sections[1])
                {
                    case ("Years"):
                        Fs = DateTimeIntervalType.Years;
                        break;
                    case ("Months"):
                        Fs = DateTimeIntervalType.Months;
                        break;
                    case ("Weeks"):
                        Fs = DateTimeIntervalType.Weeks;
                        break;
                    case ("Days"):
                        Fs = DateTimeIntervalType.Days;
                        break;
                    case ("Hours"):
                        Fs = DateTimeIntervalType.Hours;
                        break;
                    case ("Minutes"):
                        Fs = DateTimeIntervalType.Minutes;
                        break;
                    case ("Seconds"):
                        Fs = DateTimeIntervalType.Seconds;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return new DateTimePeriod(Convert.ToInt32(Sections[0]), Fs);
            }
            return new DateTimePeriod(1, DateTimeIntervalType.Days);
        }

        public void SetVar(string ValueName, object Value)
        {
            try
            {
                SetInt32(ValueName, Convert.ToInt32(Value));
            }
            catch { }
        }

        private static string CleanUp(string Input)
        {
            return Regex.Replace(Input, @"\s*[ =&]", "");
        }
    }
}