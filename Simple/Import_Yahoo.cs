using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shyu
{
    public class ImportYahoo
    {
        public static DataTable GetYahooEOD(string SymbolName, DateTime StartTime, DateTime StopTime)
        {
            DataTable YahooTable = new DataTable();
            YahooTable.TableName = SymbolName;
            uTable.AddColumn(YahooTable, "DATE", typeof(DateTime));
            uTable.AddColumn(YahooTable, "OPEN", typeof(double));
            uTable.AddColumn(YahooTable, "HIGH", typeof(double));
            uTable.AddColumn(YahooTable, "LOW", typeof(double));
            uTable.AddColumn(YahooTable, "CLOSE", typeof(double));
            uTable.AddColumn(YahooTable, "ADJ_CLOSE", typeof(double));
            uTable.AddColumn(YahooTable, "VOLUME", typeof(long));
            uTable.AddColumn(YahooTable, "ADJ_VOLUME", typeof(long));
            uTable.AddColumn(YahooTable, "DIVIDEND", typeof(float));
            uTable.AddColumn(YahooTable, "SPLIT", typeof(float));
            uTable.AddColumn(YahooTable, "ADJ", typeof(float));
            uTable.AddColumn(YahooTable, "ADJ_SPLIT", typeof(float));
            YahooTable.AcceptChanges();

            try
            {
                StringBuilder csvURL = new StringBuilder();
                csvURL.AppendFormat("http://ichart.finance.yahoo.com/table.csv?s={0}&g=d&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&ignore=.csv",
                    SymbolName, StartTime.Month - 1, StartTime.Day, StartTime.Year, StopTime.Month - 1, StopTime.Day, StopTime.Year);

                WebClient WebCSV = new WebClient();
                string csvString = WebCSV.DownloadString(csvURL.ToString());

                //File.WriteAllText(@"d:\"+ SymbolName + ".csv", csvString);

                StringReader ReadCsvBuffer = new StringReader(csvString);
                string line = ReadCsvBuffer.ReadLine();
                if (line.ToString().Replace("\n", "") == "Date,Open,High,Low,Close,Volume,Adj Close")
                {
                    double last_adj = 1;

                    while ((line = ReadCsvBuffer.ReadLine()) != null)
                    {
                        DataRow Row = YahooTable.NewRow();
                        string[] InputLineVal = Regex.Split(line, ",");
                        Row["DATE"] = DateTime.Parse(InputLineVal[0]);
                        double close = Convert.ToDouble(InputLineVal[4]);
                        double adj_close = Convert.ToDouble(InputLineVal[6]);
                        double adj = 1;
                        if (adj_close > 0) adj = close / adj_close;
                        Row["OPEN"] = Convert.ToDouble(InputLineVal[1]);
                        Row["HIGH"] = Convert.ToDouble(InputLineVal[2]);
                        Row["LOW"] = Convert.ToDouble(InputLineVal[3]);
                        Row["CLOSE"] = close;
                        Row["ADJ_CLOSE"] = adj_close;
                        long volume = Convert.ToInt64(InputLineVal[5]);
                        Row["ADJ_VOLUME"] = volume;
                        Row["DIVIDEND"] = 0;
                        Row["SPLIT"] = 1;
                        Row["ADJ"] = adj;
                        Row["ADJ_SPLIT"] = 1;
                        YahooTable.Rows.Add(Row);
                        last_adj = adj;
                    }
                }
                YahooTable.AcceptChanges();

                double last_adj_split = 1;
                for (int i = 1; i < YahooTable.Rows.Count; i++)
                {
                    double adj = uTable.LoadCell(YahooTable, i, "ADJ").Result;
                    double next_close = uTable.LoadCell(YahooTable, i - 1, "CLOSE").Result;
                    double next_adj = uTable.LoadCell(YahooTable, i - 1, "ADJ").Result;
                    double real_adj = adj - next_adj + 1;
                    double next_dividend = next_close * real_adj - next_close;

                    last_adj_split = uTable.LoadCell(YahooTable, i - 1, "ADJ_SPLIT").Result;
                    if (next_dividend >= next_close * 0.9)
                    {
                        double split = adj / next_adj;
                        YahooTable.Rows[i - 1]["SPLIT"] = split;
                        last_adj_split = split * last_adj_split;
                    }
                    YahooTable.Rows[i]["ADJ_SPLIT"] = last_adj_split;
                    YahooTable.Rows[i]["VOLUME"] = uTable.LoadCell(YahooTable, i, "ADJ_VOLUME").Result / last_adj_split;

                    double dividend = next_dividend / last_adj_split;
                    if (!(dividend > 0.001 && next_dividend < next_close * 0.9)) dividend = 0;
                    YahooTable.Rows[i - 1]["DIVIDEND"] = dividend;
                }
                last_adj_split = uTable.LoadCell(YahooTable, 0, "ADJ_SPLIT", 1).Result;
                double last_adj_volume = uTable.LoadCell(YahooTable, 0, "ADJ_VOLUME", 0).Result;
                YahooTable.Rows[0]["VOLUME"] = last_adj_volume / last_adj_split;
                YahooTable.AcceptChanges();
            }
            catch { }

            return YahooTable;
        }

        public DataTable CleanUpYahoo(DataTable Input)
        {



            return Input;
        }

        /*
        public void AppendYahooEOD(string SymbolName)
        {
            DateTime CurrentDate = DateTime.Now;
            long CurrentEID = uConv.TimeToEID(CurrentDate);
            long LastEID = EODLastEID(SymbolName);
            DateTime LastEODDate = uConv.EIDToTime(LastEID);





            ReadDataBase_EOD(SymbolName);
            ReadDataBase_Ratios(SymbolName);



        }
        */
    }
}
