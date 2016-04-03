using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Shyu.Core
{
    public static class uTable
    {
        #region Column Operations
        public static void AddColumn(DataTable t, DataColumn Column)
        {
            if (!t.Columns.Contains(Column.ColumnName))
                t.Columns.Add(Column);
            t.AcceptChanges();
        }
        public static void AddColumn(DataTable t, string ColumnName)
        {
            AddColumn(t, ColumnName, typeof(double));
        }
        public static void AddColumn(DataTable t, string ColumnName, Type DataType)
        {
            if (!t.Columns.Contains(ColumnName.ToUpper()))
                t.Columns.Add(new DataColumn(ColumnName.ToUpper(), DataType));
            t.AcceptChanges();
        }
        public static void AddColumns(DataTable t, string[] ColumnNames)
        {
            AddColumns(t, ColumnNames, typeof(double));
        }
        public static void AddColumns(DataTable t, string[] ColumnNames, Type DataType)
        {
            foreach (string Header in ColumnNames)
                if (!t.Columns.Contains(Header.ToUpper()))
                    t.Columns.Add(new DataColumn(Header.ToUpper(), DataType));
            t.AcceptChanges();
        }
        public static void AddColumns(DataTable t, Dictionary<string, DataColumn> Columns)
        {
            foreach (DataColumn Column in Columns.Values)
                if (!t.Columns.Contains(Column.ColumnName))
                    t.Columns.Add(Column);
            t.AcceptChanges();
        }
        public static void RemoveColumn(DataTable t, string ColumnName)
        {
            if (t.Columns.Contains(ColumnName.ToUpper()))
                t.Columns.Remove(ColumnName);
        }
        #endregion
        public static PropertyString GetConfig(DataTable t, int index, string ColumnName)
        {
            return new PropertyString(GetString(t, index, ColumnName));
        }
        public static Color GetColor(DataTable t, int index, string ColumnName)
        {
            return Color.FromArgb(Convert.ToInt32(GetString(t, index, ColumnName)));
        }
        public static double GetDouble(DataTable t, int index, string ColumnName)
        {
            if (index < 0 || index >= t.Rows.Count)
                return double.NaN;
            else
            {
                var Cell = t.Rows[index][ColumnName];
                if (Cell == DBNull.Value)
                    return double.NaN;
                else
                {
                    double Res = double.NaN;
                    try
                    {
                        Res = Convert.ToDouble(Cell);
                    }
                    catch
                    {
                        Res = double.NaN;
                    }
                    return Res;
                }
            }
        }
        public static bool GetBool(DataTable t, int index, string ColumnName)
        {
            if (index < 0 || index >= t.Rows.Count)
                return false;
            else if (!t.Columns.Contains(ColumnName))
                return false;
            else
                return Convert.ToBoolean(t.Rows[index][ColumnName]);
        }
        public static string GetString(DataTable t, int index, string ColumnName)
        {
            if (index < 0 || index >= t.Rows.Count)
                return string.Empty;
            else
                return t.Rows[index][ColumnName].ToString();
        }
        public static DateTime GetDateTime(DataTable t, int index, string ColumnName)
        {
            if (index < 0 || index >= t.Rows.Count)
                return DateTime.MinValue;
            else
            {
                var Cell = t.Rows[index][ColumnName];
                if (Cell == DBNull.Value)
                    return DateTime.MinValue;
                else
                {
                    DateTime Res = DateTime.MinValue;
                    try
                    {
                        Res = Convert.ToDateTime(Cell);
                    }
                    catch
                    {
                        Res = DateTime.MinValue;
                    }
                    return Res;
                }
            }
        }

        public static uCell LoadCell(DataTable t, int index, string ColumnName)
        {
            return LoadCell(t, index, ColumnName, double.NaN);
        }
        public static uCell LoadCell(DataTable t, int index, string ColumnName, double Default)
        {
            if (index < 0) return new uCell() { Result = Default, IsValid = false };
            else if (index >= t.Rows.Count || t.Rows.Count == 0 || !t.Columns.Contains(ColumnName)) return new uCell() { Result = Default, IsValid = false };

            var Cell = t.Rows[index][ColumnName];

            if (Cell != DBNull.Value)// && t.Columns[ColumnName].DataType == typeof(double))
            {
                double Res = double.NaN;
                try
                {
                    Res = Convert.ToDouble(Cell);
                }
                catch
                {
                    Res = double.NaN;
                }

                if (!double.IsNaN(Res))
                    return new uCell() { Result = Res, IsValid = true };
                else
                    return new uCell() { Result = Default, IsValid = false };
            }
            else
                return new uCell() { Result = Default, IsValid = false };
        }
        #region XML File Load/Save
        public static void SaveXML(DataTable t, FileInfo OutputFile)
        {
            if (t.TableName == string.Empty || t.TableName == null) t.TableName = OutputFile.Name;
            SaveXML(t, OutputFile.FullName);
        }
        public static void SaveXML(DataTable t, string FileName)
        {
            if (t.TableName == string.Empty || t.TableName == null) t.TableName = "uTable";
            XmlSerializer x = new XmlSerializer(typeof(DataTable));
            TextWriter writer = new StreamWriter(FileName);
            x.Serialize(writer, t);
            writer.Close();
        }
        public static DataTable LoadXML(FileInfo OutputFile)
        {
            return LoadXML(OutputFile.FullName);
        }
        public static DataTable LoadXML(string FileName)
        {
            DataTable t = new DataTable();
            t.ReadXml(FileName);
            return t;
        }
        public static string ToXML(DataTable t)
        {
            if (t.TableName == string.Empty || t.TableName == null) t.TableName = "uTable";
            XmlSerializer x = new XmlSerializer(typeof(DataTable));
            StringWriter writer = new StringWriter();
            x.Serialize(writer, t);
            return writer.ToString();
        }
        public static DataTable FromXML(string XMLString)
        {
            DataTable t = new DataTable();
            t.ReadXml(new StringReader(XMLString));
            return t;
        }
        #endregion
        #region CSV File Load/Save
        public static void SaveCSV(FileInfo OutputFile, DataTable Table)
        {
            StringBuilder BufferCSV = new StringBuilder();
            foreach (DataColumn DataColumn in Table.Columns) BufferCSV.Append(DataColumn.ColumnName + ',');
            BufferCSV.Remove(BufferCSV.Length - 1, 1);
            BufferCSV.Append(Environment.NewLine);
            foreach (DataRow Row in Table.Rows)
            {
                for (int i = 0; i < Table.Columns.Count; i++)
                {
                    string Value = Row[i].ToString().Replace(Environment.NewLine, "");
                    if (Value.Contains(',')) Value = "\"" + Value + "\"";
                    BufferCSV.Append(Value + ",");
                }
                BufferCSV.Append(Environment.NewLine);
            }
            File.WriteAllText(OutputFile.FullName, BufferCSV.ToString());
        }
        public static DataTable LoadCSV(FileInfo InputFile)
        {
            DataTable Table = new DataTable();
            StreamReader FileRead = new StreamReader(InputFile.FullName);
            string[] ColumnNames = LineToValue(FileRead.ReadLine().Trim());
            foreach (string Header in ColumnNames)
            {
                DataColumn DataColumn = new DataColumn(Header.ToUpper(), typeof(string));
                DataColumn.DefaultValue = string.Empty;
                Table.Columns.Add(DataColumn);
            }
            while (!FileRead.EndOfStream)
            {
                string[] Values = LineToValue(FileRead.ReadLine().Trim());
                if (Values.Length > 0)
                {
                    DataRow Row = Table.NewRow();
                    for (int i = 0; i < Values.Length; i++)
                        if (i <= Table.Columns.Count)
                            Row[Table.Columns[i]] = (Values[i] == null) ? string.Empty : Values[i];
                    Table.Rows.Add(Row);
                }
            }
            FileRead.Close();
            FileRead.Dispose();
            return Table;
        }
        public static DataTable LoadCSV(string CsvBuffer)
        {
            DataTable Table = new DataTable();
            StringReader ReadCsvBuffer = new StringReader(CsvBuffer);
            string[] ColumnNames = LineToValue(ReadCsvBuffer.ReadLine().Trim());
            foreach (string Header in ColumnNames)
            {
                DataColumn DataColumn = new DataColumn(Header.ToUpper(), typeof(string));
                DataColumn.DefaultValue = string.Empty;
                Table.Columns.Add(DataColumn);
            }
            string line = string.Empty;
            while ((line = ReadCsvBuffer.ReadLine()) != null)
            {
                string[] Values = LineToValue(line);
                if (Values.Length > 0)
                {
                    DataRow Row = Table.NewRow();
                    for (int i = 0; i < Values.Length; i++)
                        if (i <= Table.Columns.Count)
                            Row[Table.Columns[i]] = (Values[i] == null) ? string.Empty : Values[i].ToString();
                    Table.Rows.Add(Row);
                }
            }
            return Table;
        }
        public static string[] LineToValue(string line)
        {
            List<string> resultList = new List<string>();

            Regex pattern = new Regex(@"\s*(?:""(?<val>[^""]*(""""[^""]*)*)""\s*|(?<val>[^,]*))(?:,|$)",
                    RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            Match matchResult = pattern.Match(line);
            while (matchResult.Success)
            {
                resultList.Add(matchResult.Groups["val"].Value);
                matchResult = matchResult.NextMatch();
            }
            resultList.RemoveAt(resultList.Count - 1);
            return resultList.ToArray();
        }
        #endregion
    }
    public struct uCell
    {
        public double Result;
        public bool IsValid;
    }

    //[System.ComponentModel.DesignerCategory("Code")]
    public class ConfigTable : DataTable
    {
        public ConfigTable() { }
        public void SaveXml(FileInfo F)
        {
            XmlSerializer x = new XmlSerializer(typeof(DataTable));
            TextWriter writer = new StreamWriter(F.FullName);
            x.Serialize(writer, this);
            writer.Close();
        }
    }
}
