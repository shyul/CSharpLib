using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shyu.UserControl;
using System.IO;
using System.Text.RegularExpressions;

namespace Shyu
{
    public partial class SimpleMainForm
    {
        FileInfo EODFile = new FileInfo(@"D:\US_STOCK_EOD.mdf");
        FileInfo CalendarFile = new FileInfo(@"D:\US_STOCK_CALENDAR.mdf");

        DataTable EODTable, CalendarTable;

        bool CancelPending = false;

        private void GetTables(string SymbolName)
        {
            if (EODTable != null) EODTable.Dispose();
            EODTable = new DataTable();
            EODTable.TableName = SymbolName;
            EODTable.Columns.Add(new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0 NOT NULL PRIMARY KEY"
            EODTable.Columns.Add(new DataColumn("OPEN", typeof(float))); // "REAL"
            EODTable.Columns.Add(new DataColumn("LOW", typeof(float))); // "REAL"
            EODTable.Columns.Add(new DataColumn("HIGH", typeof(float))); // "REAL"
            EODTable.Columns.Add(new DataColumn("CLOSE", typeof(float))); // "REAL"
            EODTable.Columns.Add(new DataColumn("VOLUME", typeof(long))); // "BIGINT"
            EODTable.Columns.Add(new DataColumn("SOURCE", typeof(char))); // "CHAR"
            EODTable.PrimaryKey = new DataColumn[] { t.Columns["EID"] };
            EODTable.DefaultView.Sort = "EID ASC";
            EODTable.AcceptChanges();

            if (CalendarTable != null) CalendarTable.Dispose();
            CalendarTable = new DataTable();
            CalendarTable.TableName = SymbolName;
            CalendarTable.Columns.Add(new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0"
            CalendarTable.Columns.Add(new DataColumn("CID", typeof(string))); // "NTEXT"
            CalendarTable.Columns.Add(new DataColumn("VALUE", typeof(string))); // "NTEXT"
            CalendarTable.Columns.Add(new DataColumn("SOURCE", typeof(char))); // "CHAR"
            CalendarTable.AcceptChanges();
        }
        private void WriteTables()
        {
            //File.WriteAllText(@"d:\"+ EODTable.TableName + ".csv", CsvString);
            //CsvString = string.Empty;
            if (!EODTable.TableName.Contains('_'))
            {
                SymbolNames += EODTable.TableName + "\n";
                PrintInfo("Writing Table: " + EODTable.TableName + ", Size: " + EODTable.Rows.Count);
                EODTable.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + EODTable.TableName + "] ([EID] BIGINT NOT NULL PRIMARY KEY DEFAULT 0, [OPEN] REAL NULL, [LOW] REAL NULL, [HIGH] REAL NULL, [CLOSE] REAL NOT NULL, [VOLUME] BIGINT NOT NULL, [SOURCE] CHAR(1) NOT NULL)";
                DBUtil.SaveTable(EODFile, EODTable, SqlCmd);
                SqlCmd = "CREATE TABLE [dbo].[" + EODTable.TableName + "] ([EID] BIGINT NOT NULL, [CID] NTEXT NOT NULL, [VALUE] NTEXT NULL, [SOURCE] CHAR(1) NOT NULL)";
                DBUtil.SaveTable(CalendarFile, CalendarTable, SqlCmd);
            }
        }
        private void ReadTables(string SymbolName)
        {
            if (DBUtil.CheckExistTable(EODFile, SymbolName))
            {
                string SqlCmd = "SELECT [EID], [OPEN], [LOW], [HIGH], [CLOSE], [VOLUME], [SOURCE] from [" + SymbolName + "] ORDER BY [EID] ASC;\n";
                EODTable = DBUtil.LoadTable(EODFile, SymbolName, SqlCmd);
                SqlCmd = "SELECT [EID], [CID], [VALUE], [SOURCE] from [" + SymbolName + "] ORDER BY [EID] ASC;\n";
                CalendarTable = DBUtil.LoadTable(CalendarFile, SymbolName, SqlCmd);
            }
            else
                GetTables(SymbolName);
        }
        private string CleanUpSymbolName(string SymbolName)
        {
            SymbolName = SymbolName.Replace(" ", "");
            SymbolName = SymbolName.Replace("_P_", "-P");
            SymbolName = SymbolName.Replace("_P", "-P");
            return SymbolName;
        }
        string SymbolNames = string.Empty;
        string CsvString = string.Empty;

        private void LoadDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PrintInfo("Start...");
            DBUtil.ResetDataBaseFile(EODFile);
            DBUtil.ResetDataBaseFile(CalendarFile);
            int lineCount = File.ReadLines(EOD_File.FullName).Count();
            PrintInfo("TotalLines: " + lineCount.ToString());
            StreamReader FileCSV_read = new StreamReader(EOD_File.FullName);
            int p = 0;
            string line = string.Empty;

            //Symbol,Date,Open,High,Low,Close,Volume,Dividend,Split,Adj_Open,Adj_High,Adj_Low,Adj_Close,Adj_Volume
            //A,1999-11-18,45.5,50.0,40.0,44.0,44739900.0,0.0,1.0,29.92820636107177,32.88813885832062,26.3105110866565,28.94156219532215,44739900.0

            string LastSymbolName = string.Empty;
            while (!FileCSV_read.EndOfStream)
            //while (p < lineCount)
            {
                line = FileCSV_read.ReadLine().Trim();
                if (line.Length > 9)
                {
                    string[] val = Regex.Split(line, ",");
                    if (val.Length == 14)
                    {
                        //CsvString += line + "\n";
                        string CurrentSymbolName = CleanUpSymbolName(val[0]);
                        if (CurrentSymbolName != LastSymbolName)
                        {
                            if (p > 0) WriteTables();
                            ReadTables(CurrentSymbolName);
                        }

                        bool Valid = true;
                        DataRow Row = EODTable.NewRow();
                        long eid = 0;
                        try { eid = TimeToEID(DateTime.Parse(val[1])); Row["EID"] = eid; } catch { Valid = false; }
                        try { Row["OPEN"] = Convert.ToSingle(val[2]); } catch { }
                        try { Row["HIGH"] = Convert.ToSingle(val[3]); } catch { }
                        try { Row["LOW"] = Convert.ToSingle(val[4]); } catch { }
                        try { Row["CLOSE"] = Convert.ToSingle(val[5]); } catch { Valid = false; }
                        try { Row["VOLUME"] = Convert.ToSingle(val[6]); } catch { Row["VOLUME"] = 0; }

                        if (Valid)
                        {
                            Row["SOURCE"] = 'Q';
                            EODTable.Rows.Add(Row);

                            float divident = Convert.ToSingle(val[7]);
                            if (divident > 0)
                            {
                                Row = CalendarTable.NewRow();
                                Row["CID"] = "DIVIDEND";
                                Row["EID"] = eid;
                                Row["VALUE"] = divident.ToString();
                                Row["SOURCE"] = 'Q';
                                CalendarTable.Rows.Add(Row);
                            }

                            float split = Convert.ToSingle(val[8]);
                            if (split != 1)
                            {
                                Row = CalendarTable.NewRow();
                                Row["CID"] = "SPLIT";
                                Row["EID"] = eid;
                                Row["VALUE"] = split.ToString();
                                Row["SOURCE"] = 'Q';
                                CalendarTable.Rows.Add(Row);

                            }
                        }
                        LastSymbolName = CurrentSymbolName;
                        LoadDataWorker.ReportProgress((int)(p * 100.0f / lineCount));
                    }

                }
                //if (p < 1000) PrintInfo(line);
                p++;
                if (CancelPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
            WriteTables();
            t = EODTable.Copy();
        }

        private void LoadDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void LoadDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            File.WriteAllText(@"d:\SymbolNames.txt", SymbolNames);
            PrintInfo("Done.");
        }

        public FileInfo EOD_File;

        public DataTable t = new DataTable();
        public DataTable Calendar = new DataTable();

        public DataSet LoadQmFullCSV(string CsvPath)
        {
            DataTable CsvTable = new DataTable();
            DataColumn tColumn = new DataColumn();
            StreamReader FileCSV_read = new StreamReader(CsvPath);
            string line = "Symbol,Date,Open,High,Low,Close,Volume,Dividend,Split,Adj_Open,Adj_High,Adj_Low,Adj_Close,Adj_Volume";
            string[] tColumnNames = Regex.Split(line, ",");
            for (int i = 0; i < tColumnNames.Length; i++)
            {
                tColumn = new DataColumn(tColumnNames[i].ToUpper(), typeof(string));
                tColumn.DefaultValue = string.Empty;
                CsvTable.Columns.Add(tColumn);
            }
            while ((line = FileCSV_read.ReadLine().Trim()) != null)
            {
                string[] val = Regex.Split(line, ",");
                DataRow tDataRow = CsvTable.NewRow();
                for (int i = 0; i < tColumnNames.Length; i++)
                {
                    tDataRow[tColumnNames[i]] = (val[i] == null) ? string.Empty : val[i].ToString();
                }
                CsvTable.Rows.Add(tDataRow);
            }


            DataSet Osiano = new DataSet();

            for (int i = 0; i < CsvTable.Rows.Count; i++)
            {
                if (Osiano.Tables.Contains(CsvTable.Rows[i]["SYMBOL"].ToString()))
                {
                    DataTable tmpTable = new DataTable();
                    for (int j = 0; j < t.Rows.Count; j++)
                    {
                        DataColumn sColumn = new DataColumn(t.Rows[j]["ColumeName"].ToString().ToUpper(), Type.GetType(t.Rows[j]["DataType"].ToString()));
                        sColumn.DefaultValue = null;
                        tmpTable.Columns.Add(sColumn);
                    }
                    tmpTable.TableName = CsvTable.Rows[i]["SYMBOL"].ToString();
                    tmpTable.AcceptChanges();
                    Osiano.Merge(tmpTable);
                }

                bool Valid = true;
                bool Error = false;

                DateTime Time = DateTime.Parse(CsvTable.Rows[i]["DATE"].ToString());
                double high = Double.NaN;
                double low = Double.NaN;
                double open = Double.NaN;
                double close = Double.NaN;
                double volume = Double.NaN;
                double adj_high = Double.NaN;
                double adj_low = Double.NaN;
                double adj_open = Double.NaN;
                double adj_close = Double.NaN;
                double adj_volume = Double.NaN;
                double split = Double.NaN;
                double dividend = Double.NaN;

                try { close = Convert.ToDouble(CsvTable.Rows[i]["CLOSE"]); }
                catch { Valid = false; }

                try { adj_close = Convert.ToDouble(CsvTable.Rows[i]["ADJ_CLOSE"]); }
                catch { Valid = false; }

                try { volume = Convert.ToDouble(CsvTable.Rows[i]["VOLUME"]); }
                catch { Valid = false; }

                try { adj_volume = Convert.ToDouble(CsvTable.Rows[i]["ADJ_VOLUME"]); }
                catch { Valid = false; }

                try { high = Convert.ToDouble(CsvTable.Rows[i]["HIGH"]); }
                catch { high = close; Error = true; }
                try { low = Convert.ToDouble(CsvTable.Rows[i]["LOW"]); }
                catch { low = close; Error = true; }
                try { open = Convert.ToDouble(CsvTable.Rows[i]["OPEN"]); }
                catch { open = close; Error = true; }

                try { adj_high = Convert.ToDouble(CsvTable.Rows[i]["ADJ_HIGH"]); }
                catch { adj_high = adj_close; Error = true; }
                try { adj_low = Convert.ToDouble(CsvTable.Rows[i]["ADJ_LOW"]); }
                catch { adj_low = adj_close; Error = true; }
                try { adj_open = Convert.ToDouble(CsvTable.Rows[i]["ADJ_OPEN"]); }
                catch { adj_open = adj_close; Error = true; }

                try { split = Convert.ToDouble(CsvTable.Rows[i]["SPLIT"]); }
                catch { split = 1; Error = true; }
                try { dividend = Convert.ToDouble(CsvTable.Rows[i]["DIVIDEND"]); }
                catch { dividend = 0; Error = true; }

                if (Valid)
                {
                    double adj = 1;
                    long eid = TimeToEID(Time);
                    if (adj_close > 0)
                        adj = close / adj_close;
                    else
                        adj = 0;

                    DataRow tRow = Osiano.Tables[CsvTable.Rows[i]["SYMBOL"].ToString()].NewRow();
                    tRow["EID"] = TimeToEID(Time);
                    tRow["PERIOD"] = 86400;
                    if (!Error) tRow["LAST_UPDATE_SOURCE"] = "QUOTEMEDIA"; else tRow["LAST_UPDATE_SOURCE"] = "ERROR";
                    tRow["LAST_UPDATE"] = DateTime.Now;
                    tRow["DATETIME"] = Time;
                    if (high >= 0) tRow["HIGH"] = (float)high;
                    if (high >= 0) tRow["LOW"] = (float)low;
                    if (high >= 0) tRow["OPEN"] = (float)open;
                    if (high >= 0) tRow["CLOSE"] = (float)close;
                    if (high >= 0) tRow["VOLUME"] = DoubleToLong(volume);
                    if (high >= 0) tRow["ADJ_HIGH"] = (float)adj_high;
                    if (high >= 0) tRow["ADJ_LOW"] = (float)adj_low;
                    if (high >= 0) tRow["ADJ_OPEN"] = (float)adj_open;
                    if (high >= 0) tRow["ADJ_CLOSE"] = (float)adj_close;
                    if (high >= 0) tRow["ADJ_VOLUME"] = DoubleToLong(adj_volume);
                    tRow["ADJ"] = (float)adj;
                    if (high >= 0) tRow["SPLIT"] = (float)split;
                    if (high >= 0) tRow["DIVIDEND"] = (float)dividend;
                    Osiano.Tables[CsvTable.Rows[i]["SYMBOL"].ToString()].Rows.Add(tRow);
                }

            }

            for (int i = 0; i < Osiano.Tables.Count; i++)
            {
                Osiano.Tables[i].DefaultView.Sort = "EID DESC";
                Osiano.Tables[i].AcceptChanges();
            }

            Osiano.AcceptChanges();

            return Osiano;
        }

        private long DoubleToLong(double e)
        {
            if (e - (long)e >= 0.5f) e = e + 0.5f;
            return (long)e;
        }
        public static long TimeToEID(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

    }
}
