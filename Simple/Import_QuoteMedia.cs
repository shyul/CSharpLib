using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shyu.Finance
{
    public partial class TechDataBase
    {
        public FinancialDataInfo F = new FinancialDataInfo(FinancialDataType.US_STK);

        public DataTable StockInfoTable, Table_EOD, Table_Ratios;

        private void WriteDataBase()
        {
            if (!Table_EOD.TableName.Contains('_'))
            {
                PrintInfo("Writing Table: " + Table_EOD.TableName + ", Size: " + Table_EOD.Rows.Count);

                HistoricalData.WriteDataBase_EOD(F.DataFile_EOD, Table_EOD);
                HistoricalData.WriteDataBase_Ratios(F.DataFile_Ratios, Table_Ratios);

                if (!StockInfoTable.Rows.Contains(Table_EOD.TableName))
                {
                    DataRow Row = StockInfoTable.NewRow();
                    Row["SymbolName"] = Table_EOD.TableName;
                    Row["LastUpdatedEID"] = 0;
                    StockInfoTable.Rows.Add(Row);
                }
            }
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

        public BackgroundWorker LoadDataWorker;
        public Queue<string> Message;

        private void PrintInfo(string Text)
        {
            if (Message.Count < 100) Message.Enqueue(" " + Text + "\n");
        }


        public void ImportData_DoWork(object sender, DoWorkEventArgs e)
        {
            StockInfoTable = StockInformationList.Init_StockInfoTable();
            PrintInfo("Start...");
            DBUtil.ResetDataBaseFile(F.DataFile_EOD);
            DBUtil.ResetDataBaseFile(F.DataFile_Ratios);
            int lineCount = File.ReadLines(EODInputFile.FullName).Count();
            PrintInfo("TotalLines: " + lineCount.ToString());

            StreamReader FileCSV_read = new StreamReader(EODInputFile.FullName);

            int p = 0;
            string line = string.Empty;

            //Symbol,Date,Open,High,Low,Close,Volume,Dividend,Split,Adj_Open,Adj_High,Adj_Low,Adj_Close,Adj_Volume
            //A,1999-11-18,45.5,50.0,40.0,44.0,44739900.0,0.0,1.0,29.92820636107177,32.88813885832062,26.3105110866565,28.94156219532215,44739900.0

            string LastSymbolName = string.Empty;

            //while (p < lineCount)
            while (!FileCSV_read.EndOfStream)
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
                            Table_EOD = HistoricalData.ReadDataBase_EOD(F.DataFile_EOD, CurrentSymbolName);
                            Table_Ratios = HistoricalData.ReadDataBase_Ratios(F.DataFile_Ratios, CurrentSymbolName);
                        }

                        bool Valid = true;
                        DataRow Row = Table_EOD.NewRow();
                        long eid = 0;

                        try
                        {
                            DateTime Date = DateTime.Parse(val[1]);
                            eid = uConv.TimeToEID(Date);
                            Row["EID"] = eid;
                        }
                        catch
                        {
                            Valid = false;
                        }

                        float close = 0;

                        try
                        {
                            close = Convert.ToSingle(val[5]);
                            Row["CLOSE"] = close;
                        } catch
                        {
                            Valid = false;
                        }

                        try { Row["OPEN"] = Convert.ToSingle(val[2]); } catch { Row["OPEN"] = close; }
                        try { Row["HIGH"] = Convert.ToSingle(val[3]); } catch { Row["HIGH"] = close; }
                        try { Row["LOW"] = Convert.ToSingle(val[4]); } catch { Row["LOW"] = close; }
                        try { Row["VOLUME"] = (long)Convert.ToDouble(val[6]); } catch { Row["VOLUME"] = 0; }

                        if (Valid)
                        {
                            Row["SOURCE"] = 'Q';
                            Table_EOD.Rows.Add(Row);

                            float dividend = Convert.ToSingle(val[7]);
                            if (dividend > 0)
                            {
                                Row = Table_Ratios.NewRow();
                                Row["EID"] = eid;
                                Row["RATIOTYPE"] = (int)RatioType.Dividend;
                                Row["PARAM"] = "MRQ";
                                Row["VALUE"] = dividend;
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
            StockInfoTable.AcceptChanges();
            StockInfoTable.WriteXml(F.DataFile_Info.FullName);
        }
    }

    /*select TOP 1 * from [AAPL] ORDER BY [EID] DESC;
    select * from [AAPL] ORDER BY [EID] ASC
    select * from [AAPL] where [NAME] = 'DIVIDEND' ORDER BY [EID] DESC;
    select [EID], [VALUE] from [AAPL] where [RATIOTYPE] = 1 ORDER BY [EID] DESC;*/
}
