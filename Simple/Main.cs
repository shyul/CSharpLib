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
using System.Threading;
using Shyu.Finance;

namespace Shyu
{
    public partial class SimpleMainForm : Form
    {
        TechDataBaseQM tdb = new TechDataBaseQM();

        public SimpleMainForm()
        {
            InitializeComponent();
            tdb = new TechDataBaseQM();
            tdb.LoadDataWorker = new BackgroundWorker();
            tdb.Message = Message;
            tdb.LoadDataWorker.WorkerReportsProgress = true;
            tdb.LoadDataWorker.WorkerSupportsCancellation = true;
            tdb.LoadDataWorker.DoWork += new DoWorkEventHandler(tdb.ImportData_DoWork);
            tdb.LoadDataWorker.ProgressChanged += new ProgressChangedEventHandler(this.LoadDataWorker_ProgressChanged);
            tdb.LoadDataWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LoadDataWorker_RunWorkerCompleted);
            MessageWorker.RunWorkerAsync();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                DialogFileOpen.DefaultExt = "csv";
                DialogFileOpen.Filter = "EOD Data Set|*.csv";
                DialogFileOpen.Title = "Open EOD Data Set";
                if (DialogFileOpen.ShowDialog() == DialogResult.OK && DialogFileOpen.FileName != string.Empty)// && DialogFileOpen.DialogResult == DialogResult.OK)
                {
                    Status.Clear();
                    tdb.EODInputFile = new FileInfo(DialogFileOpen.FileName);
                    PrintInfo("Load new file " + tdb.EODInputFile.FullName);
                    tdb.CancelPending = false;
                    tdb.LoadDataWorker.RunWorkerAsync();
                    btnStart.Text = "Stop";
                }
            }
            else if (btnStart.Text == "Stop")
            {
                tdb.CancelPending = true;
                PrintInfo("Cancelled");
                File.WriteAllText(@"d:\temp.txt", Status.Text);
                btnStart.Text = "Start";
            }
            else
            {
                tdb.CancelPending = true;
                btnStart.Text = "Start";
            }
        }

        private void btnEOD_Click(object sender, EventArgs e)
        {
            tdb.Table_EOD = HistoricalData.ReadDataBase_EOD(tdb.F.DataFile_EOD, tbSymbolName.Text);
            GridForm g = new GridForm();
            g.Grid.DataSource = tdb.Table_EOD;
            g.Show();
        }

        private void btnRatios_Click(object sender, EventArgs e)
        {
            tdb.rt = HistoricalData.ReadDataBase_Ratios(tdb.F.DataFile_Ratios, tbSymbolName.Text);
            GridForm g = new GridForm();
            g.Grid.DataSource = tdb.rt;
            g.Show();
        }

        private void SimpleMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tdb.LoadDataWorker.CancelAsync();
            MessageWorker.CancelAsync();

        }

        private void LoadDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void LoadDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            tdb.CancelPending = true;
            btnStart.Text = "Start";
            PrintInfo("Done.");
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string SymbolName = tbSymbolName.Text;
            DateTime StartTime = uConv.EIDToTime(HistoricalData.EODLastEID(tdb.F.DataFile_EOD, SymbolName));
            StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day + 1);

            DateTime StopTime = DateTime.Now;

            int days = uConv.Round((float)(StopTime - StartTime).TotalDays);

            bool NeedsUpdate = false;

            for (int i = 0; i < days; i++)
            {
                NeedsUpdate = uConv.IsTradingDate(new DateTime(StartTime.Year, StartTime.Month, StartTime.Day + i));
                if (NeedsUpdate) break;
            }
            DataTable YahooTable = new DataTable();
            if (NeedsUpdate)
            {
                YahooTable = ImportYahoo.GetYahooEOD(SymbolName, StartTime, StopTime);

                if (YahooTable.Rows.Count > 0)
                {
                    tdb.Table_EOD = HistoricalData.ReadDataBase_EOD(tdb.F.DataFile_EOD, SymbolName);
                    tdb.rt = HistoricalData.ReadDataBase_Ratios(tdb.F.DataFile_Ratios, SymbolName);
                    for (int i = 0; i < YahooTable.Rows.Count; i++)
                    {
                        bool Valid = true;
                        DataRow Row = tdb.Table_EOD.NewRow();
                        long eid = 0;

                        try
                        {
                            DateTime Date = DateTime.Parse(YahooTable.Rows[i]["DATE"].ToString());
                            eid = uConv.TimeToEID(Date);
                            Row["EID"] = eid;
                        }
                        catch
                        {
                            Valid = false;
                        }

                        DataRow FindEIDRow = tdb.Table_EOD.Rows.Find(eid);
                        if (FindEIDRow != null) Valid = false;

                        if (Valid)
                        {
                            Row["OPEN"] = YahooTable.Rows[i]["OPEN"];
                            Row["LOW"] = YahooTable.Rows[i]["LOW"];
                            Row["HIGH"] = YahooTable.Rows[i]["HIGH"];
                            Row["CLOSE"] = YahooTable.Rows[i]["CLOSE"];
                            Row["VOLUME"] = YahooTable.Rows[i]["VOLUME"];
                            Row["SOURCE"] = 'Y';
                            tdb.Table_EOD.Rows.Add(Row);

                            float dividend = Convert.ToSingle(YahooTable.Rows[i]["DIVIDEND"]);
                            if (dividend > 0 && HistoricalData.Load_Ratios(tdb.F.DataFile_Ratios, SymbolName, RatioType.Dividend, eid).Rows.Count == 0)
                            {
                                Row = tdb.rt.NewRow();
                                Row["EID"] = eid;
                                Row["RATIOTYPE"] = (int)RatioType.Dividend;
                                Row["PARAM"] = "MRQ";
                                Row["VALUE"] = dividend;
                                Row["SOURCE"] = 'Y';
                                tdb.rt.Rows.Add(Row);
                            }

                            float split = Convert.ToSingle(YahooTable.Rows[i]["SPLIT"]);
                            if (split != 1 && HistoricalData.Load_Ratios(tdb.F.DataFile_Ratios, SymbolName, RatioType.Split, eid).Rows.Count == 0)
                            {
                                Row = tdb.rt.NewRow();
                                Row["EID"] = eid;
                                Row["RATIOTYPE"] = (int)RatioType.Split;
                                Row["VALUE"] = split;
                                Row["SOURCE"] = 'Y';
                                tdb.rt.Rows.Add(Row);
                            }
                        }
                    }
                    HistoricalData.WriteDataBase_EOD(tdb.F.DataFile_EOD, tdb.Table_EOD);
                    HistoricalData.WriteDataBase_Ratios(tdb.F.DataFile_Ratios, tdb.rt);
                }
                else
                {
                    MessageBox.Show("Nothing to update even it seems need update");
                }
            }
            else
            {
                MessageBox.Show("Nothing to update");
            }

            GridForm g = new GridForm();
            g.Grid.DataSource = YahooTable;// tdb.Table_EOD;
            g.Show();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string SymbolName = tbSymbolName.Text;
            DataTable t = tdb.Load_EOD(SymbolName, new DateTime(1980, 12, 12), new DateTime(2015, 5, 8), AdjLevel.SplitDividend);

            GridForm g = new GridForm();
            g.Grid.DataSource = t;// tdb.Table_EOD;
            g.Show();
        }
    }
}
