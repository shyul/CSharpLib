using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shyu
{
    public enum PeriodUnit
    {
        Year,
        Quarter,
        Month,
        Week,
        Day,
        Hour,
        Minute,
        Second
    }

    public enum DataFileType
    {
        INFO = 0,
        EOD = 1,
        RATIOS = 2,

    }

    public enum StockExchange
    {
        NASDAQ = 0,
        NYSE = 1,
        NYSEMKT = 2,
        OTCMKT = 3,
    }

    public enum StockCountry
    {
        US = 0,
        CN = 1,
    }

    public partial class TechDataBase
    {
        public StockCountry StockCountry
        {
            get
            {
                return _StockCountry;
            }
            set
            {
                _StockCountry = value;
                GetCountrySetup();
            }
        }

        public string Currency;

        private StockCountry _StockCountry;
        
        private string DataDir = @"D:\";
        public FileInfo DataFile_Info, DataFile_EOD, DataFile_Ratios;
        public DataTable Table_Info, Table_EOD, Table_Ratios;

        private void GetCountrySetup()
        {
            switch (_StockCountry)
            {
                case (StockCountry.US):
                    DataFile_Info = new FileInfo(DataDir + "US_STK_INFO.xml");
                    DataFile_EOD = new FileInfo(DataDir + "US_STK_EOD.mdf");
                    DataFile_Ratios = new FileInfo(DataDir + "US_STK_RATIOS.mdf");
                    Currency = "$USD";
                    break;
                case (StockCountry.CN):
                    DataFile_Info = new FileInfo(DataDir + "CN_STK_INFO.xml");
                    DataFile_EOD = new FileInfo(DataDir + "CN_STK_EOD.mdf");
                    DataFile_Ratios = new FileInfo(DataDir + "CN_STK_RATIOS.mdf");
                    Currency = "¥CNY";
                    break;
                default:
                    break;
            }
        }

        private void InitInfoTable()
        {
            Table_Info = new DataTable();
            Table_Info.TableName = DataFile_Info.Name;
            uTable.AddColumn(Table_Info, new DataColumn("LastUpdatedEID", typeof(long)));
            uTable.AddColumn(Table_Info, new DataColumn("SymbolName", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("CompanyName", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("Exchange", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("CUSIP", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("ISIN", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("AddressStreet", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("AddressCity", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("PhoneNumer", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("WebSite", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("BusinessSummary", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("FinancialSummary", typeof(string)));
            uTable.AddColumn(Table_Info, new DataColumn("Other", typeof(string)));

            Table_Info.PrimaryKey = new DataColumn[] { Table_Info.Columns["SymbolName"] };
            Table_Info.DefaultView.Sort = "SymbolName ASC";
            Table_Info.AcceptChanges();
        }

        private void InitTables(string SymbolName)
        {
            if (Table_EOD != null) Table_EOD.Dispose();
            Table_EOD = new DataTable();
            Table_EOD.TableName = SymbolName;
            Table_EOD.Columns.Add(new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0 NOT NULL PRIMARY KEY"
            Table_EOD.Columns.Add(new DataColumn("OPEN", typeof(float))); // "REAL"
            Table_EOD.Columns.Add(new DataColumn("LOW", typeof(float))); // "REAL"
            Table_EOD.Columns.Add(new DataColumn("HIGH", typeof(float))); // "REAL"
            Table_EOD.Columns.Add(new DataColumn("CLOSE", typeof(float))); // "REAL"
            Table_EOD.Columns.Add(new DataColumn("VOLUME", typeof(long))); // "BIGINT"
            Table_EOD.Columns.Add(new DataColumn("SOURCE", typeof(char))); // "CHAR"
            Table_EOD.PrimaryKey = new DataColumn[] { Table_EOD.Columns["EID"] };
            Table_EOD.DefaultView.Sort = "EID ASC";
            Table_EOD.AcceptChanges();

            if (Table_Ratios != null) Table_Ratios.Dispose();
            Table_Ratios = new DataTable();
            Table_Ratios.TableName = SymbolName;
            Table_Ratios.Columns.Add(new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0"
            Table_Ratios.Columns.Add(new DataColumn("RATIOTYPE", typeof(int))); // "INT"
            Table_Ratios.Columns.Add(new DataColumn("PARAM", typeof(string))); // "NVARCHAR(100)"
            Table_Ratios.Columns.Add(new DataColumn("VALUE", typeof(double))); // "FLOAT"
            Table_Ratios.Columns.Add(new DataColumn("SOURCE", typeof(char))); // "NCHAR(1)"
            Table_Ratios.AcceptChanges();
        }

        private void WriteDataBase()
        {
            if (!Table_EOD.TableName.Contains('_'))
            {
                PrintInfo("Writing Table: " + Table_EOD.TableName + ", Size: " + Table_EOD.Rows.Count);
                Table_EOD.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + Table_EOD.TableName + "] ([EID] BIGINT NOT NULL PRIMARY KEY DEFAULT 0, [OPEN] REAL NULL, [LOW] REAL NULL, [HIGH] REAL NULL, [CLOSE] REAL NOT NULL, [VOLUME] BIGINT NOT NULL, [SOURCE] CHAR(1) NOT NULL)";
                DBUtil.SaveTable(DataFile_EOD, Table_EOD, SqlCmd);
                if(Table_Ratios.Rows.Count > 0)
                {
                    SqlCmd = "CREATE TABLE [dbo].[" + Table_EOD.TableName + "] ([EID] BIGINT NOT NULL, [RATIOTYPE] INT NOT NULL, [PARAM] NVARCHAR(100) NULL, [VALUE] FLOAT NOT NULL, [SOURCE] NCHAR(1) NOT NULL)";
                    DBUtil.SaveTable(DataFile_Ratios, Table_Ratios, SqlCmd);
                }
                
                if (!Table_Info.Rows.Contains(Table_EOD.TableName))
                {
                    DataRow Row = Table_Info.NewRow();
                    Row["SymbolName"] = Table_EOD.TableName;
                    Row["LastUpdatedEID"] = 0;
                    Table_Info.Rows.Add(Row);
                }
            }
        }
        private void ReadDataBase(string SymbolName)
        {
            if (DBUtil.CheckExistTable(DataFile_EOD, SymbolName))
            {
                string SqlCmd = "SELECT [EID], [OPEN], [LOW], [HIGH], [CLOSE], [VOLUME], [SOURCE] from [" + SymbolName + "] ORDER BY [EID] ASC;\n";
                Table_EOD = DBUtil.LoadTable(DataFile_EOD, SymbolName, SqlCmd);
                SqlCmd = "SELECT [EID], [NAME], [VALUE], [SOURCE] from [" + SymbolName + "] ORDER BY [EID] ASC;\n";
                Table_Ratios = DBUtil.LoadTable(DataFile_Ratios, SymbolName, SqlCmd);
            }
            else
                InitTables(SymbolName);
        }

        public bool CancelPending = false;
        public FileInfo EODInputFile;

        private string CleanUpSymbolName(string SymbolName)
        {
            SymbolName = SymbolName.Replace(" ", "");
            SymbolName = SymbolName.Replace("_P_", "-P");
            SymbolName = SymbolName.Replace("_P", "-P");
            return SymbolName;
        }
        public void LoadDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitInfoTable();
            PrintInfo("Start...");
            DBUtil.ResetDataBaseFile(DataFile_EOD);
            DBUtil.ResetDataBaseFile(DataFile_Ratios);
            int lineCount = File.ReadLines(EODInputFile.FullName).Count();
            PrintInfo("TotalLines: " + lineCount.ToString());
            StreamReader FileCSV_read = new StreamReader(EODInputFile.FullName);
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
                        string CurrentSymbolName = CleanUpSymbolName(val[0]);
                        if (CurrentSymbolName != LastSymbolName)
                        {
                            if (p > 0) WriteDataBase();
                            ReadDataBase(CurrentSymbolName);
                        }

                        bool Valid = true;
                        DataRow Row = Table_EOD.NewRow();
                        long eid = 0;
                        try { eid = uConv.TimeToEID(DateTime.Parse(val[1])); Row["EID"] = eid; } catch { Valid = false; }
                        try { Row["OPEN"] = Convert.ToSingle(val[2]); } catch { }
                        try { Row["HIGH"] = Convert.ToSingle(val[3]); } catch { }
                        try { Row["LOW"] = Convert.ToSingle(val[4]); } catch { }
                        try { Row["CLOSE"] = Convert.ToSingle(val[5]); } catch { Valid = false; }
                        try { Row["VOLUME"] = (long)Convert.ToDouble(val[6]); } catch { Row["VOLUME"] = 0; }

                        if (Valid)
                        {
                            Row["SOURCE"] = 'Q';
                            Table_EOD.Rows.Add(Row);
                            
                            float divident = Convert.ToSingle(val[7]);
                            if (divident > 0)
                            {
                                Row = Table_Ratios.NewRow();
                                Row["EID"] = eid;
                                Row["RATIOTYPE"] = (int)RatioType.Dividend;
                                Row["PARAM"] = "QUARTER";
                                Row["VALUE"] = divident;
                                Row["SOURCE"] = 'Q';
                                Table_Ratios.Rows.Add(Row);
                            }

                            float split = Convert.ToSingle(val[8]);
                            if (split != 1)
                            {
                                Row = Table_Ratios.NewRow();
                                Row["EID"] = eid;
                                Row["RATIOTYPE"] = (int)RatioType.Split;
                                Row["VALUE"] = split;
                                Row["SOURCE"] = 'Q';
                                Table_Ratios.Rows.Add(Row);
                            }
                        }
                        LastSymbolName = CurrentSymbolName;
                        LoadDataWorker.ReportProgress((int)(p * 100.0f / lineCount));
                    }

                }
                p++;
                if (CancelPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
            WriteDataBase();
            Table_Info.AcceptChanges();
            Table_Info.WriteXml(DataFile_Info.FullName);
        }

        public BackgroundWorker LoadDataWorker;
        public Queue<string> Message;

        private void PrintInfo(string Text)
        {
            if (Message.Count < 100) Message.Enqueue(" " + Text + "\n");
        }
    }

    public enum RatioType
    {
        Split = 0,
        Dividend = 1,
        EPS = 2,
    }

    /*select TOP 1 * from [AAPL] ORDER BY [EID] DESC;
    select * from [AAPL] ORDER BY [EID] ASC
    select * from [AAPL] where [NAME] = 'DIVIDEND' ORDER BY [EID] DESC;
    select [EID], [VALUE] from [AAPL] where [RATIOTYPE] = 1 ORDER BY [EID] DESC;*/

    public static class DataSource
    {
        public const string Q = "QuoteMedia";
        public const string I = "Interactive Brokers";
        public const string Y = "Yahoo";
    }
}
