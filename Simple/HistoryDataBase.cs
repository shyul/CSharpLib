using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shyu.Finance
{
    public class HistoryDataBase
    {
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

        public FileInfo DataFile_Main;          // Company Info
        public FileInfo DataFile_EOD;           // EOD Data
        public FileInfo DataFile_Ratios;        // Ratios / Calendars

        public DataTable info;
        public DataTable trades;
        public DataTable eod;
        public DataTable ratios;

        public string Currency;
        public string SecType;

        private FinancialDataType _DataType;
        //private string DataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Osiano\Data\";
        private string DataDirPath = @"D:\";

        private void InitSetup()
        {
            switch (_DataType)
            {
                case (FinancialDataType.US_STK):
                    DataFile_Main = new FileInfo(DataDirPath + "US_STK_MAIN.mdf");
                    DataFile_EOD = new FileInfo(DataDirPath + "US_STK_EOD.mdf");
                    DataFile_Ratios = new FileInfo(DataDirPath + "US_STK_RATIOS.mdf");
                    Currency = "$USD";
                    SecType = "Stock";
                    break;
                case (FinancialDataType.CN_STK):
                    DataFile_Main = new FileInfo(DataDirPath + "CN_STK_MAIN.mdf");
                    DataFile_EOD = new FileInfo(DataDirPath + "CN_STK_EOD.mdf");
                    DataFile_Ratios = new FileInfo(DataDirPath + "CN_STK_RATIOS.mdf");
                    Currency = "¥CNY";
                    SecType = "Stock";
                    break;
                default:
                    break;
            }
        }
        #region Stock Infomation Data Table 
        public void info_FormatTable()
        {
            info.TableName = "StockInfo";
            uTable.AddColumn(info, new DataColumn("LastUpdatedEID", typeof(long))); // "BIGINT"
            uTable.AddColumn(info, new DataColumn("SymbolName", typeof(string))); // "NVARCHAR(20)"
            uTable.AddColumn(info, new DataColumn("CompanyName", typeof(string))); // "NVARCHAR(50)"
            uTable.AddColumn(info, new DataColumn("Exchange", typeof(string))); // "NVARCHAR(20)"
            uTable.AddColumn(info, new DataColumn("CUSIP", typeof(string))); // "NVARCHAR(20)"
            uTable.AddColumn(info, new DataColumn("ISIN", typeof(string)));  // "NVARCHAR(20)"
            uTable.AddColumn(info, new DataColumn("AddressStreet", typeof(string))); // "NTEXT"
            uTable.AddColumn(info, new DataColumn("AddressCity", typeof(string))); // "NTEXT"
            uTable.AddColumn(info, new DataColumn("PhoneNumber", typeof(string))); // "NVARCHAR(20)"
            uTable.AddColumn(info, new DataColumn("WebSite", typeof(string))); // "NVARCHAR(120)"
            uTable.AddColumn(info, new DataColumn("BusinessSummary", typeof(string))); // "NTEXT"
            uTable.AddColumn(info, new DataColumn("FinancialSummary", typeof(string))); // "NTEXT"
            info.PrimaryKey = new DataColumn[] { info.Columns["SymbolName"] };
            info.DefaultView.Sort = "SymbolName ASC";
            info.AcceptChanges();
        }
        public void info_InitTable()
        {
            if (info != null) info.Dispose();
            info = new DataTable();
            info_FormatTable();
        }
        public void info_WriteDataBase()
        {
            if (info.Rows.Count > 0)
            {
                info.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + info.TableName + "] " +
                    "([EID] BIGINT NOT NULL, " +
                    "[SymbolName] NVARCHAR(20) NOT NULL, " +
                    "[CompanyName] NTEXT NULL, " +
                    "[Exchange] NTEXT NULL, " +
                    "[CUSIP] NTEXT NULL, " +
                    "[ISIN] NTEXT NULL, " +
                    "[AddressStreet] NTEXT NULL, " +
                    "[AddressCity] NTEXT NULL, " +
                    "[PhoneNumber] NTEXT NULL, " +
                    "[WebSite] NTEXT NULL, " +
                    "[BusinessSummary] NTEXT NULL, " +
                    "[FinancialSummary] NTEXT NULL)";
                DBUtil.SaveTable(DataFile_Main, info, SqlCmd);
            }
        }
        public void info_ReadDataBase(string SqlCmd)
        {
            if (DBUtil.CheckExistTable(DataFile_Main, "StockInfo"))
            {
                DataTable info = new DataTable();
                info = DBUtil.LoadTable(DataFile_Main, "StockInfo", SqlCmd);
                info_FormatTable();
            }
            else
                info_InitTable();
        }
        public void info_ReadDataBase()
        {
            info_ReadDataBase("select * from [StockInfo];");
        }
        #endregion
        #region Trade History Table
        public void trades_FormatTable()
        {
            trades.TableName = "TradeHistory";
            uTable.AddColumn(trades, new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0 NOT NULL PRIMARY KEY"
            uTable.AddColumn(trades, new DataColumn("TYPE", typeof(int))); // "REAL"
            uTable.AddColumn(trades, new DataColumn("SYMBOLNAME", typeof(string))); // "NVARCHAR(20)"
            uTable.AddColumn(trades, new DataColumn("QUANTITY", typeof(float))); // "REAL"
            uTable.AddColumn(trades, new DataColumn("PRICE", typeof(float))); // "REAL"
            uTable.AddColumn(trades, new DataColumn("ACCOUNT", typeof(string))); // "NVARCHAR(20)"
            uTable.AddColumn(trades, new DataColumn("METHOD", typeof(string))); // "NTEXT"
            trades.DefaultView.Sort = "EID ASC";
            trades.AcceptChanges();
        }
        public void trades_InitTable()
        {
            if (trades != null) trades.Dispose();
            trades = new DataTable();
            trades_FormatTable();
        }
        public void trades_WriteDataBase()
        {
            if (trades.Rows.Count > 0)
            {
                trades.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + trades.TableName + "] " +
                    "([EID] BIGINT NOT NULL, " +
                    "[TYPE] INT NOT NULL, " +
                    "[SYMBOLNAME] NVARCHAR(20) NOT NULL, " +
                    "[QUANTITY] REAL NOT NULL, " +
                    "[PRICE] REAL NOT NULL, " +
                    "[ACCOUNT] NVARCHAR(20) NULL, " +
                    "[METHOD] NTEXT NULL)";
                DBUtil.SaveTable(DataFile_Main, trades, SqlCmd);
            }
        }
        #endregion
        #region EOD RAW Data Table 
        public void eod_FormatTable()
        {
            uTable.AddColumn(eod, new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0 NOT NULL PRIMARY KEY"
            uTable.AddColumn(eod, new DataColumn("OPEN", typeof(float))); // "REAL"
            uTable.AddColumn(eod, new DataColumn("LOW", typeof(float))); // "REAL"
            uTable.AddColumn(eod, new DataColumn("HIGH", typeof(float))); // "REAL"
            uTable.AddColumn(eod, new DataColumn("CLOSE", typeof(float))); // "REAL"
            uTable.AddColumn(eod, new DataColumn("VOLUME", typeof(long))); // "BIGINT"
            uTable.AddColumn(eod, new DataColumn("SOURCE", typeof(char))); // "CHAR"
            eod.PrimaryKey = new DataColumn[] { eod.Columns["EID"] };
            eod.DefaultView.Sort = "EID ASC";
            eod.AcceptChanges();
        }
        public void eod_InitTable(string SymbolName)
        {
            if (eod != null) eod.Dispose();
            eod = new DataTable();
            eod.TableName = SymbolName;
            eod_FormatTable();
        }
        public void eod_WriteDataBase()
        {
            if (eod.Rows.Count > 0)
            {
                eod.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + eod.TableName + "] ([EID] BIGINT NOT NULL PRIMARY KEY DEFAULT 0, [OPEN] REAL NULL, [LOW] REAL NULL, [HIGH] REAL NULL, [CLOSE] REAL NOT NULL, [VOLUME] BIGINT NOT NULL, [SOURCE] CHAR(1) NOT NULL)";
                DBUtil.SaveTable(DataFile_EOD, eod, SqlCmd);
            }
        }
        public void eod_ReadDataBase(string SymbolName, string SqlCmd)
        {
            if (DBUtil.CheckExistTable(DataFile_EOD, SymbolName))
            {
                DataTable eod = new DataTable();
                eod = DBUtil.LoadTable(DataFile_EOD, SymbolName, SqlCmd);
                eod.TableName = SymbolName;
                eod_FormatTable();
            }
            else
                eod_InitTable(SymbolName);
        }
        public void eod_ReadDataBase(string SymbolName)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("select * from [{0}] ORDER BY [EID] ASC;", SymbolName);
            eod_ReadDataBase(SymbolName, s.ToString());
        }
        #endregion
        #region Ratio / Fundamental Data Table
        public void ratios_FormatTable()
        {
            uTable.AddColumn(ratios, new DataColumn("EID", typeof(long))); // "BIGINT DEFAULT 0"
            uTable.AddColumn(ratios, new DataColumn("RATIOTYPE", typeof(int))); // "INT"
            uTable.AddColumn(ratios, new DataColumn("PARAM", typeof(string))); // "NVARCHAR(50)"
            uTable.AddColumn(ratios, new DataColumn("VALUE", typeof(double))); // "FLOAT"
            uTable.AddColumn(ratios, new DataColumn("SOURCE", typeof(char))); // "NCHAR(1)"
            ratios.PrimaryKey = new DataColumn[] { ratios.Columns["EID"], ratios.Columns["RATIOTYPE"] };
            ratios.DefaultView.Sort = "EID ASC";
            ratios.AcceptChanges();
        }
        public void ratios_InitTable(string SymbolName)
        {
            if (ratios != null) ratios.Dispose();
            ratios = new DataTable();
            ratios.TableName = SymbolName;
            ratios_FormatTable();
        }
        public void ratios_WriteDataBase()
        {
            if (ratios.Rows.Count > 0)
            {
                ratios.AcceptChanges();
                string SqlCmd = "CREATE TABLE [dbo].[" + ratios.TableName + "] ([EID] BIGINT NOT NULL, [RATIOTYPE] INT NOT NULL, [PARAM] NVARCHAR(50) NULL, [VALUE] FLOAT NOT NULL, [SOURCE] NCHAR(1) NOT NULL)";
                DBUtil.SaveTable(DataFile_Ratios, ratios, SqlCmd);
            }
        }
        public void ratios_ReadDataBase(string SymbolName, string SqlCmd)
        {
            if (DBUtil.CheckExistTable(DataFile_Ratios, SymbolName))
            {
                DataTable ratios = new DataTable();
                ratios = DBUtil.LoadTable(DataFile_Ratios, SymbolName, SqlCmd);
                ratios.TableName = SymbolName;
                ratios_FormatTable();
            }
            else
                ratios_InitTable(SymbolName);
        }
        public void ratios_ReadDataBase(string SymbolName)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("select * from [{0}] ORDER BY [EID] ASC;", SymbolName);
            ratios_ReadDataBase(SymbolName, s.ToString());
        }
        #endregion
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
}
