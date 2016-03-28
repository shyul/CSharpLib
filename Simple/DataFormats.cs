using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shyu.Finance
{
    public class FinancialDataInfo
    {
        public FileInfo DataFile_Info, DataFile_EOD, DataFile_Ratios;
        public string Currency, SecType;

        public FinancialDataInfo() { }
        public FinancialDataInfo(FinancialDataType dataType)
        {
            _DataType = dataType;
            InitSetup();
        }

        public FinancialDataType DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
                InitSetup();
            }
        }

        //private string DataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Osiano\Data\";//    @"D:\";
        private string DataDir = @"D:\";
        private FinancialDataType _DataType;

        private void InitSetup()
        {
            switch (_DataType)
            {
                case (FinancialDataType.US_STK):
                    DataFile_Info = new FileInfo(DataDir + "US_STK_INFO.xml");
                    DataFile_EOD = new FileInfo(DataDir + "US_STK_EOD.mdf");
                    DataFile_Ratios = new FileInfo(DataDir + "US_STK_RATIOS.mdf");
                    Currency = "$USD";
                    SecType = "Stock";
                    break;
                case (FinancialDataType.CN_STK):
                    DataFile_Info = new FileInfo(DataDir + "CN_STK_INFO.xml");
                    DataFile_EOD = new FileInfo(DataDir + "CN_STK_EOD.mdf");
                    DataFile_Ratios = new FileInfo(DataDir + "CN_STK_RATIOS.mdf");
                    Currency = "¥CNY";
                    SecType = "Stock";
                    break;
                default:
                    break;
            }
        }
    }
    public enum FinancialDataType
    {
        FOREX = 0,
        US_STK = 10,
        US_OPT = 11,
        US_FUT = 12,
        US_BOND = 13,
        CN_STK = 20,
    }

    public class StockInformationList
    {
        public static DataTable Format_StockInfoTable(DataTable StockInfoTable)
        {
            StockInfoTable.TableName = "StockInfo";
            uTable.AddColumn(StockInfoTable, new DataColumn("LastUpdatedEID", typeof(long)));
            uTable.AddColumn(StockInfoTable, new DataColumn("SymbolName", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("CompanyName", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("Exchange", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("CUSIP", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("ISIN", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("AddressStreet", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("AddressCity", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("PhoneNumer", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("WebSite", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("BusinessSummary", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("FinancialSummary", typeof(string)));
            uTable.AddColumn(StockInfoTable, new DataColumn("Other", typeof(string)));
            StockInfoTable.PrimaryKey = new DataColumn[] { StockInfoTable.Columns["SymbolName"] };
            StockInfoTable.DefaultView.Sort = "SymbolName ASC";
            StockInfoTable.AcceptChanges();
            return StockInfoTable;
        }
        public static DataTable Init_StockInfoTable()
        {
            DataTable StockInfoTable = new DataTable();
            return Format_StockInfoTable(StockInfoTable);
        }
    }
    public class HistoricalData
    {
        #region EOD RAW Data Table 
        public static DataTable Format_StockEODTable(DataTable EODTable)
        {
            uTable.AddColumn(EODTable, new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0 NOT NULL PRIMARY KEY"
            uTable.AddColumn(EODTable, new DataColumn("OPEN", typeof(float))); // "REAL"
            uTable.AddColumn(EODTable, new DataColumn("LOW", typeof(float))); // "REAL"
            uTable.AddColumn(EODTable, new DataColumn("HIGH", typeof(float))); // "REAL"
            uTable.AddColumn(EODTable, new DataColumn("CLOSE", typeof(float))); // "REAL"
            uTable.AddColumn(EODTable, new DataColumn("VOLUME", typeof(long))); // "BIGINT"
            uTable.AddColumn(EODTable, new DataColumn("SOURCE", typeof(char))); // "CHAR"
            EODTable.PrimaryKey = new DataColumn[] { EODTable.Columns["EID"] };
            EODTable.DefaultView.Sort = "EID ASC";
            EODTable.AcceptChanges();
            return EODTable;
        }
        public static DataTable Init_StockEODTable(string SymbolName)
        {
            DataTable EODTable = new DataTable();
            EODTable.TableName = SymbolName;
            return Format_StockEODTable(EODTable);
        }
        public static void WriteDataBase_EOD(FileInfo DataFile, DataTable EODTable)
        {
            if (EODTable.Rows.Count > 0)
            {
                EODTable.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + EODTable.TableName + "] ([EID] BIGINT NOT NULL PRIMARY KEY DEFAULT 0, [OPEN] REAL NULL, [LOW] REAL NULL, [HIGH] REAL NULL, [CLOSE] REAL NOT NULL, [VOLUME] BIGINT NOT NULL, [SOURCE] CHAR(1) NOT NULL)";
                DBUtil.SaveTable(DataFile, EODTable, SqlCmd);
            }
        }
        public static DataTable ReadDataBase_EOD(FileInfo DataFile, string SymbolName)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                DataTable EODTable = new DataTable();
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select * from [{0}] ORDER BY [EID] ASC;", SymbolName);
                EODTable = DBUtil.LoadTable(DataFile, SymbolName, s.ToString());
                EODTable.TableName = SymbolName;
                EODTable.PrimaryKey = new DataColumn[] { EODTable.Columns["EID"] };
                EODTable.DefaultView.Sort = "EID ASC";
                EODTable.AcceptChanges();
                return EODTable;
            }
            else
                return Init_StockEODTable(SymbolName);
        }
        public static DataTable Load_EOD(FileInfo DataFile, string SymbolName)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                DataTable EODTable = new DataTable();
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select [EID], [OPEN], [HIGH], [LOW], [CLOSE], [VOLUME] from [{0}] ORDER BY [EID] ASC;", SymbolName);
                EODTable = DBUtil.LoadTable(DataFile, SymbolName, s.ToString());
                EODTable.TableName = SymbolName;
                EODTable.PrimaryKey = new DataColumn[] { EODTable.Columns["EID"] };
                EODTable.DefaultView.Sort = "EID ASC";
                EODTable.AcceptChanges();
                return EODTable;
            }
            else
                return Init_StockEODTable(SymbolName);
        }
        #endregion
        #region Ratio / Fundamental Data Table
        public static DataTable Format_StockRatiosTable(DataTable RatiosTable)
        {
            uTable.AddColumn(RatiosTable, new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0"
            uTable.AddColumn(RatiosTable, new DataColumn("RATIOTYPE", typeof(int))); // "INT"
            uTable.AddColumn(RatiosTable, new DataColumn("PARAM", typeof(string))); // "NVARCHAR(50)"
            uTable.AddColumn(RatiosTable, new DataColumn("VALUE", typeof(double))); // "FLOAT"
            uTable.AddColumn(RatiosTable, new DataColumn("SOURCE", typeof(char))); // "NCHAR(1)"
            RatiosTable.PrimaryKey = new DataColumn[] { RatiosTable.Columns["EID"], RatiosTable.Columns["RATIOTYPE"] };
            RatiosTable.DefaultView.Sort = "EID ASC";
            RatiosTable.AcceptChanges();
            return RatiosTable;
        }
        public static DataTable InitRatiosTable(string SymbolName)
        {
            DataTable RatiosTable = new DataTable();
            RatiosTable.TableName = SymbolName;
            return Format_StockRatiosTable(RatiosTable);
        }
        public static void WriteDataBase_Ratios(FileInfo DataFile, DataTable RatiosTable)
        {
            if (RatiosTable.Rows.Count > 0)
            {
                RatiosTable.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + RatiosTable.TableName + "] ([EID] BIGINT NOT NULL, [RATIOTYPE] INT NOT NULL, [PARAM] NVARCHAR(50) NULL, [VALUE] FLOAT NOT NULL, [SOURCE] NCHAR(1) NOT NULL)";
                DBUtil.SaveTable(DataFile, RatiosTable, SqlCmd);
            }
        }
        public static DataTable ReadDataBase_Ratios(FileInfo DataFile, string SymbolName)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                DataTable RatiosTable = new DataTable();
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select * from [{0}] ORDER BY [EID] ASC;", SymbolName);
                RatiosTable = DBUtil.LoadTable(DataFile, SymbolName, s.ToString());
                RatiosTable.TableName = SymbolName;
                RatiosTable.PrimaryKey = new DataColumn[] { RatiosTable.Columns["EID"], RatiosTable.Columns["RATIOTYPE"] };
                RatiosTable.DefaultView.Sort = "EID ASC";
                RatiosTable.AcceptChanges();
                return RatiosTable;
            }
            else
                return InitRatiosTable(SymbolName);
        }
        public static DataTable Load_Ratios(FileInfo DataFile, string SymbolName)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                DataTable RatiosTable = new DataTable();
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select [EID], [RATIOTYPE], [PARAM], [VALUE] from [{0}] ORDER BY [EID] ASC;", SymbolName);
                RatiosTable = DBUtil.LoadTable(DataFile, SymbolName, s.ToString());
                RatiosTable.TableName = SymbolName;
                RatiosTable.PrimaryKey = new DataColumn[] { RatiosTable.Columns["EID"], RatiosTable.Columns["RATIOTYPE"] };
                RatiosTable.DefaultView.Sort = "EID ASC";
                RatiosTable.AcceptChanges();
                return RatiosTable;
            }
            else
                return InitRatiosTable(SymbolName);
        }
        public static DataTable Load_Ratios(FileInfo DataFile, string SymbolName, RatioType ratioType)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                DataTable RatiosTable = new DataTable();
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select [EID], [RATIOTYPE], [PARAM], [VALUE] from [{0}] where [RATIOTYPE] = {1} ORDER BY [EID] ASC;", SymbolName, (int)ratioType);
                RatiosTable = DBUtil.LoadTable(DataFile, SymbolName, s.ToString());
                RatiosTable.TableName = SymbolName;
                RatiosTable.PrimaryKey = new DataColumn[] { RatiosTable.Columns["EID"], RatiosTable.Columns["RATIOTYPE"] };
                RatiosTable.DefaultView.Sort = "EID ASC";
                RatiosTable.AcceptChanges();
                return RatiosTable;
            }
            else
                return InitRatiosTable(SymbolName);
        }
        public static DataTable Load_Ratios(FileInfo DataFile, string SymbolName, RatioType ratioType, long EID)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                DataTable RatiosTable = new DataTable();
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select [EID], [RATIOTYPE], [PARAM], [VALUE] from [{0}] where [RATIOTYPE] = {1} AND [EID] = {2} ORDER BY [EID] ASC;", SymbolName, (int)ratioType, EID);
                RatiosTable = DBUtil.LoadTable(DataFile, SymbolName, s.ToString());
                RatiosTable.TableName = SymbolName;
                RatiosTable.PrimaryKey = new DataColumn[] { RatiosTable.Columns["EID"], RatiosTable.Columns["RATIOTYPE"] };
                RatiosTable.DefaultView.Sort = "EID ASC";
                RatiosTable.AcceptChanges();
                return RatiosTable;
            }
            else
                return InitRatiosTable(SymbolName);
        }
        #endregion
        public static long EODLastEID(FileInfo DataFile, string SymbolName)
        {
            if (DBUtil.CheckExistTable(DataFile, SymbolName))
            {
                StringBuilder s = new StringBuilder();
                s.AppendFormat("select TOP 1 [EID] from [{0}] ORDER BY [EID] DESC;", SymbolName);
                string ResultStr = string.Empty;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = DBUtil.GetConnectionString(DataFile);
                    conn.Open();
                    SqlCommand command = new SqlCommand(s.ToString(), conn);
                    SqlDataReader res = command.ExecuteReader();
                    while (res.Read()) ResultStr = res[0].ToString();
                    conn.Close();
                    conn.Dispose();
                }
                return Convert.ToInt64(ResultStr);
            }
            else
                return 0;
        }
    }
    public enum RatioType
    {
        Split = 0,
        Dividend = 1,
        EPS = 2,
    }

    public enum EODCorrectionLevel
    {
        None = 0,
        Split = 1,
        SplitDividend = 2
    }

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

    public enum StockExchange
    {
        NASDAQ = 0,
        NYSE = 1,
        NYSEMKT = 2,
        OTCMKT = 3,
    }

    public static class DataSource
    {
        public const string Q = "QuoteMedia";
        public const string I = "Interactive Brokers";
        public const string Y = "Yahoo";
    }
}
